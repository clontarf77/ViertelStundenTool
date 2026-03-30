using LimitOrders15minGUI.ViewModels;
using MahApps.Metro.Controls;
using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows;
using ViertelStdTool.ConfigurationHandler;
using ViertelStdTool.Log;
using ViertelStdTool.Likron;
using System.Reflection;
using System.IO;
using System.Threading;
using ViertelStdToolLib.Comtrader;
using System.Data;
using System.Windows.Media;
using ViertelStdTool.CredentialsHandler;
using ViertelStdToolLib.BookTradesAligne;
using System.Collections.Generic;
using ViertelStdToolLib.Aligne.DataBaseRequests;
using ViertelStdTool.Mail;
using System.Linq;
using System.ComponentModel;
using ViertelStdTool.Views;

namespace LimitOrders15minGUI.Views
{
    /// <summary>
    /// Interaktionslogik für AppViewxaml.xaml
    /// </summary>
    public partial class AppView : MetroWindow
    {
        #region global variables
        // ViewModel
        private readonly AppViewModel viewModel;
        // View
        private UserInfoEmergencySoftware userInfoEmergencySoftware;
        // Time settings.
        private const int circulationTimeOrderThread = 180; // seconds
        private const int circulationTimeBookingThread = 30; // seconds
        // Timer
        private readonly DispatcherTimer Timer = new DispatcherTimer();
        // Countdown for Threads
        private int timespanNextExecOrderThread = circulationTimeOrderThread; 
        private int timespanNextExecBookingThread = circulationTimeBookingThread;
        private bool countdownOrderThreadStarted = false;
        private bool countdownBookingThreadStarted = false;       
        // ConfigurationReader
        private readonly IConfigurationReader configReader;
        private readonly ConfigurationStructure config = new ConfigurationStructure();
        // CredentialsReader
        private readonly ICredentialsReader credentialsReader;
        private readonly CredentialsStructureAligne credentials = new CredentialsStructureAligne();
        // Logger
        private readonly INLogger logger = new NLogger();
        // Email
        private readonly ISendMail sendMail = new SendMail();
        private readonly string sSMTPHost = "smtp-ma.konzern.mvvcorp.de";
        private readonly string sender = "ViertelStdTool@mvv.de";
        private readonly List<string> receiver = new List<string>();
        private readonly string title = "ViertelStdTool - Error - " + DateTime.Now.ToString() ;        
        // Quarterly Hour To Likron
        private readonly IQuarterHourToLikron toLikron;
        // Read trades from comtrader export.
        private readonly IReadExport comtrader;
        // Book traedes to Aligne.
        private readonly IBookTradesAligne bookTrades;
        // Filter for sFTP.
        private const string fileStartsWith = "FromVPPToComTrader";
        // Results of QuarterHourToLikron
        private ResultsQuarterHourToLikron results = new ResultsQuarterHourToLikron();
        // Process Read trades in own thread.
        private Thread readAndBookTradesThread;
        // Process for main functionality.
        private Thread orderThread;
        // Use flags to cancel threads.
        private bool setFlagToCancelOrderThread = false;
        private bool setFlagToCancelReadAndBookTradesThread = false;
        // Use flags to see if thread is stopped.
        private bool orderThreadStopped = false;
        private bool readAndBookTradesThreadStopped = false;
        // Use flags to see if thread is sleeping.
        private bool orderThreadSleeping = false;
        private bool readAndBookTradesThreadSleeping = false;
        // Flag to see if init was done
        private readonly bool initDone = false;
        #endregion

        #region Do the application init. - This is the Constructor.
        public AppView()
        {
            string resultInit = string.Empty;
            string pathExecutable = string.Empty;

            // Do the logging.
            logger.WriteInfo("------------------------------------------------------------------------------------------------------------------------------");
            logger.WriteInfo("                                     ViertelStdTool started in version " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + ".");
            logger.WriteInfo("------------------------------------------------------------------------------------------------------------------------------");
            try
            {
                // Set Eventhandler for timer used for clock.
                Timer.Tick += new EventHandler(Timer_Click);
                Timer.Interval = new TimeSpan(0, 0, 1);
                Timer.Start();
                
                //GUI-Model
                DataContext = new AppViewModel();
                viewModel = (AppViewModel)DataContext;

                // Do basic initialization.
                InitializeComponent();

                // Init done.
                initDone = true;

                //Read configuration from App.config
                configReader = new ConfigurationReader();
                config = configReader.Config;

                // Add mail receiver in case of error to list.
                receiver.Add(config.mailInCaseOfError);
                viewModel.AppModel.MailInCaseOfError = config.mailInCaseOfError; 

                try
                {
                    //Show Start window
                    userInfoEmergencySoftware = new UserInfoEmergencySoftware();                   
                    userInfoEmergencySoftware.ShowDialog();
                    
                    //Read configuration from settings.xml from user's document folder.
                    credentialsReader = new CredentialsReader(config.fileNameCredentialsAligne);
                    credentials = credentialsReader.Credentials;
                }
                catch (Exception exception)
                {
                    // Write error to log.
                    logger.WriteError("Credentials for importing trades to Aligne could not be read: Error: " + exception.Message.ToString());
                    // // Write error to aligne booking page.
                    viewModel.AppModel.NumberBookedTradesToAligne = "Error - Credentials to book trades to Aligne not set. For more info see log.";
                    // Write error to main page.
                    viewModel.AppModel.ErrorDescription = "Error - Credentials to book trades to Aligne not set. For more info see log.";
                    NumberOfBookedTradesToAligne.Foreground = Brushes.Red;

                    if (viewModel.AppModel.MailInCaseOfErrorEnabled)
                    {
                        // Send mail with error information - if enabled.
                        sendMail.SendErrorMailviaSMTP(sSMTPHost, sender, receiver, title, "Credentials to book trades to Aligne not set.");
                    }
                }
                // Create diretory 
                Directory.CreateDirectory("LocalFiles");
                pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(AppView)).CodeBase);
                #region Workaround if started with long path. If not substituted path will exceed 260 chars and exception will be thrown.
                if (pathExecutable.StartsWith(@"file:\konzern\dfs\firmen"))
                {
                    pathExecutable = pathExecutable.Replace(@"file:\konzern\dfs\firmen", "T:");
                    logger.WriteInfo("Replaced path 'file:\\konzern\\dfs\\firmen' with 'T:'");
                }
                else if (pathExecutable.StartsWith("file:\\"))
                {
                    pathExecutable = pathExecutable.Remove(0, 6);
                }
                else if (pathExecutable.StartsWith(@"\\konzern\dfs\Firmen"))
                {
                    pathExecutable = pathExecutable.Replace(@"\\konzern\dfs\Firmen", "T:");
                    logger.WriteInfo("Replaced path '\\\\konzern\\dfs\\Firmen' with 'T:'");
                }
                #endregion
                config.ownPath = pathExecutable + "\\LocalFiles\\";
                logger.WriteInfo("Init: Set path for own files: " + config.ownPath + ".");
                // Save config data in model
                viewModel.AppModel.Config = config;
                // Save credentials data in model
                viewModel.AppModel.CredentialsAligne = credentials;

                toLikron = new QuarterHourToLikron();
                resultInit = toLikron.Init(viewModel.AppModel.Config);
                if (resultInit.Equals("OK"))
                {
                    logger.WriteInfo("Init: Initialization of quarterly hour to Likron was successful.");
                }
                else
                {
                    logger.WriteInfo("Init: Initialization of quarterly hour to Likron was NOT successful.");
                }

                // No init thus no log entry here.
                comtrader = new ReadExport();

                // No init thus no log entry here.
                bookTrades = new BookTradesAligne();               
            }
            catch (Exception exception)
            {
                // Write error to log.
                logger.WriteError("Error during start of applikation: " + exception.Message.ToString());
                if (viewModel.AppModel.MailInCaseOfErrorEnabled)
                {
                    // Send mail with error information - if enabled.
                    sendMail.SendErrorMailviaSMTP(sSMTPHost, sender, receiver, title, "Error during start of applikation: " + exception.Message.ToString());
                }
            }
        }
        #endregion

        #region Timer per second
        /// <summary>
        /// Timer for clock.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Click(object sender, EventArgs e)
        {
            DateTime dateTime = DateTime.Now;
            DateTimeFormatInfo fmt = (new CultureInfo("de-DE")).DateTimeFormat;
            LabelTime.Content = dateTime.ToString("HH:mm:ss");
            LabelDate.Content = dateTime.ToString("d", fmt);

            if(countdownOrderThreadStarted)
            {
                // Countdown for order thread if started.
                timespanNextExecOrderThread--;
                TimeSpan result = TimeSpan.FromSeconds(timespanNextExecOrderThread);
                LabelCountdownOrderThread.Content = result.ToString();
            }
            else
            {               
                LabelCountdownOrderThread.Content = "placing in Likron";               
            }

            if (countdownBookingThreadStarted)
            {
                // Countdown for booking thread if started.
                timespanNextExecBookingThread--;
                TimeSpan result = TimeSpan.FromSeconds(timespanNextExecBookingThread);
                LabelCountdownBookingThread.Content = result.ToString();
            }
            else
            {
                LabelCountdownBookingThread.Content = "booking to Aligne";
            }
        }
        #endregion

        #region Tab Selection - todo check when to update view for likron and alignen trades
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Just do this if event is fired from a tabcontrol.
            if (e.Source is TabControl)
            {
                if (Tab_LoadData.IsSelected)
                {
                    // disable label and number of trades from comtrader export 
                    LabelNumberReadTradesFromLikron.Visibility = Visibility.Hidden;
                    NumberOfReadTradesFromLikron.Visibility = Visibility.Hidden;
                    // disable label and number of trades booked from VSK to Aligne.
                    LabelNumberBookedTradesToAligne.Visibility = Visibility.Hidden;
                    NumberOfBookedTradesToAligne.Visibility = Visibility.Hidden;                   
                }
                if (Tab_LimitData.IsSelected)
                {
                    // disable label and number of trades from comtrader export 
                    LabelNumberReadTradesFromLikron.Visibility = Visibility.Hidden;
                    NumberOfReadTradesFromLikron.Visibility = Visibility.Hidden;
                    // disable label and number of trades booked from VSK to Aligne.
                    LabelNumberBookedTradesToAligne.Visibility = Visibility.Hidden;
                    NumberOfBookedTradesToAligne.Visibility = Visibility.Hidden;
                }
                if (Tab_OrderData.IsSelected)
                {
                    // disable label and number of trades from comtrader export 
                    LabelNumberReadTradesFromLikron.Visibility = Visibility.Hidden;
                    NumberOfReadTradesFromLikron.Visibility = Visibility.Hidden;
                    // disable label and number of trades booked from VSK to Aligne.
                    LabelNumberBookedTradesToAligne.Visibility = Visibility.Hidden;
                    NumberOfBookedTradesToAligne.Visibility = Visibility.Hidden;                  
                }
                if (Tab_ExecutedTradesLikron.IsSelected)
                {
                    // enable label and number of trades from comtrader export 
                    LabelNumberReadTradesFromLikron.Visibility = Visibility.Visible;
                    NumberOfReadTradesFromLikron.Visibility = Visibility.Visible;
                    // disable label and number of trades booked from VSK to Aligne.
                    LabelNumberBookedTradesToAligne.Visibility = Visibility.Hidden;
                    NumberOfBookedTradesToAligne.Visibility = Visibility.Hidden;                   
                }
                if (Tab_BookedTradesAligne.IsSelected)
                {
                    // disable label and number of trades from comtrader export 
                    LabelNumberReadTradesFromLikron.Visibility = Visibility.Hidden;
                    NumberOfReadTradesFromLikron.Visibility = Visibility.Hidden;
                    // enable label and number of trades booked from VSK to Aligne.
                    LabelNumberBookedTradesToAligne.Visibility = Visibility.Visible;
                    NumberOfBookedTradesToAligne.Visibility = Visibility.Visible;                    
                }
            }
        }
        #endregion

        #region Combobox - Select Limit Type - Percent/MWh
        private void ComboBox_LimitType_Changed(object sender, SelectionChangedEventArgs e)
        {
            // Get string from current selected item.
            string selectedItem = ((sender as ComboBox).SelectedItem).ToString();

            if(initDone)
            {

                switch ((LimitType)Enum.Parse(typeof(LimitType), selectedItem))
                {
                    case LimitType.NoLimit:
                        Quanitity_in_MWh.Visibility = Visibility.Hidden;
                        Quanitity_in_Percent.Visibility = Visibility.Hidden;
                        TextBox_LoadLimit.Visibility = Visibility.Hidden;
                        SkipLoadLimit.Visibility = Visibility.Hidden;
                        SetLoadLimit.Visibility = Visibility.Hidden;
                        viewModel.AppModel.SelectedLoadLimitValue = 0;
                        TextBox_LoadLimit.Text = "";
                        viewModel.AppModel.SelectedLimitType = LimitType.NoLimit;
                        break;
                    case LimitType.MWh:
                        Quanitity_in_MWh.Visibility = Visibility.Visible;
                        Quanitity_in_Percent.Visibility = Visibility.Hidden;
                        TextBox_LoadLimit.Visibility = Visibility.Visible;
                        SkipLoadLimit.Visibility = Visibility.Hidden;
                        SetLoadLimit.Visibility = Visibility.Hidden;
                        viewModel.AppModel.SelectedLoadLimitValue = 0;
                        TextBox_LoadLimit.Text = "";
                        break;
                    case LimitType.Percent:
                        Quanitity_in_MWh.Visibility = Visibility.Hidden;
                        Quanitity_in_Percent.Visibility = Visibility.Visible;
                        TextBox_LoadLimit.Visibility = Visibility.Visible;
                        SkipLoadLimit.Visibility = Visibility.Hidden;
                        SetLoadLimit.Visibility = Visibility.Hidden;
                        viewModel.AppModel.SelectedLoadLimitValue = 0;
                        TextBox_LoadLimit.Text = "";
                        break;
                    default:
                        // Should not happen
                        break;
                }
            }
        }
        #endregion

        #region Button - Set Load Limit Click
        private void SetLoadLimit_Click(object sender, RoutedEventArgs e)
        {
            viewModel.AppModel.SelectedLoadLimitValue = Convert.ToDecimal(TextBox_LoadLimit.Text);
            SkipLoadLimit.Visibility = Visibility.Visible;
            SetLoadLimit.Visibility = Visibility.Hidden;

            if (viewModel.AppModel.SelectedLoadLimitValue == 0)
            {
                TextBox_LoadLimit.Text = "";
                viewModel.AppModel.SelectedLimitType = LimitType.NoLimit;
                ComboBox_LimitType.SelectedValue = LimitType.NoLimit.ToString();
            }

            if ((viewModel.AppModel.SelectedLoadLimitValue >= 100) && (viewModel.AppModel.SelectedLimitType == LimitType.Percent))
            {
                TextBox_LoadLimit.Text = "";
                viewModel.AppModel.SelectedLimitType = LimitType.NoLimit;
                ComboBox_LimitType.SelectedValue = LimitType.NoLimit.ToString();
            }
        }
        #endregion

        #region Button - Set Sell Limit Click
        private void SetSellLimit_Click(object sender, RoutedEventArgs e)
        {
            viewModel.AppModel.SelectedSellLimitValue = Convert.ToDecimal(TextBox_SellLimit.Text);
            SkipSellLimit.Visibility = Visibility.Visible;
            SetSellLimit.Visibility = Visibility.Hidden;

            if (viewModel.AppModel.SelectedSellLimitValue == 0)
            {
                TextBox_SellLimit.Text = "";               
            }           
        }
        #endregion

        #region Button - Set Buy Limit Click
        private void SetBuyLimit_Click(object sender, RoutedEventArgs e)
        {
            viewModel.AppModel.SelectedBuyLimitValue = Convert.ToDecimal(TextBox_BuyLimit.Text);
            SkipBuyLimit.Visibility = Visibility.Visible;
            SetBuyLimit.Visibility = Visibility.Hidden;

            if (viewModel.AppModel.SelectedBuyLimitValue == 0)
            {
                TextBox_BuyLimit.Text = "";               
            }
        }
        #endregion

        #region Button - Skip Load Limit Click
        private void SkipLoadLimit_Click(object sender, RoutedEventArgs e)
        {
            viewModel.AppModel.SelectedLoadLimitValue = 0;
            TextBox_LoadLimit.Text = "";
            viewModel.AppModel.SelectedLimitType = LimitType.NoLimit;
            ComboBox_LimitType.SelectedValue = LimitType.NoLimit.ToString();
            SkipLoadLimit.Visibility = Visibility.Hidden;            
        }
        #endregion

        #region Button - Skip Sell Limit Click
        private void SkipSellLimit_Click(object sender, RoutedEventArgs e)
        {
            viewModel.AppModel.SelectedSellLimitValue = 0;
            TextBox_SellLimit.Text = "";
            SkipSellLimit.Visibility = Visibility.Hidden;
        }
        #endregion

        #region Button - Skip Buy Limit Click
        private void SkipBuyLimit_Click(object sender, RoutedEventArgs e)
        {
            viewModel.AppModel.SelectedBuyLimitValue = 0;
            TextBox_BuyLimit.Text = "";
            SkipBuyLimit.Visibility = Visibility.Hidden;
        }
        #endregion

        #region Button - StartApplication
        private void StartApplication_Click(object sender, RoutedEventArgs e)
        {
            StartApplication.Visibility = Visibility.Hidden;
            StopApplication.Visibility = Visibility.Visible;
            viewModel.AppModel.AppStatus = "App running";
            AppStatus.Foreground = Brushes.LimeGreen;

            logger.WriteInfo("START button pressed.");
                       
            //Process Read trades in own thread.
            orderThread = new Thread(OrderProcess);
            orderThread.Start();
            // Set default proccess is started no stop request is set.
            setFlagToCancelOrderThread = false;
            // Set flag to false cause thread is started.
            orderThreadStopped = false;
            logger.WriteInfo("Order Thread: Started.");                      
            
            //Process Read trades from comtrader export and book to aligne in own thread.
            readAndBookTradesThread = new Thread(ReadAndBookTradesProcess);                
            readAndBookTradesThread.Start();
            // Set default proccess is started no stop request is set.
            setFlagToCancelReadAndBookTradesThread = false;
            // Set flag to false cause thread is started.
            readAndBookTradesThreadStopped = false;
            logger.WriteInfo("ReadAndBookTrades Thread: Started.");            

            // Show Countdown for thtreads.
            LabelCountdownOrderThread.Visibility = Visibility.Visible;
            LabelCountdownBookingThread.Visibility = Visibility.Visible;
        }
        #endregion

        #region Button - StopApplication
        private void StopApplication_Click(object sender, RoutedEventArgs e)
        {
            StartApplication.Visibility = Visibility.Hidden;
            StopApplication.Visibility = Visibility.Hidden;
            viewModel.AppModel.AppStatus = "App stopping";
            AppStatus.Foreground = Brushes.Red;

            logger.WriteInfo("STOP button pressed.");
                       
            // If order thread is sleeping abort instantly.
            if (orderThreadSleeping)
            {
                // Abort thread instantly if sleping.
                orderThread.Abort();
                // Set flag to true cause thread is aborted.
                orderThreadStopped = true;
                logger.WriteInfo("Order Thread: Stopped - Thread was sleeping.");
            }
            else
            {
                // Set cancel flag to true.
                setFlagToCancelOrderThread = true;
                logger.WriteInfo("Order Thread: Requested to stop.");
            }            

            // If booking thread is sleeping abort instantly.
            if (readAndBookTradesThreadSleeping)
            {
                // Abort thread instantly if sleping.
                readAndBookTradesThread.Abort();
                // Set flag to true cause thread is aborted.
                readAndBookTradesThreadStopped = true;
                logger.WriteInfo("ReadAndBookTrades Thread: Stopped - Thread was sleeping.");
            }
            else
            {
                // Set cancel flag to true.
                setFlagToCancelReadAndBookTradesThread = true;
                logger.WriteInfo("ReadAndBookTrades Thread: Requested to stop.");
            }

            // App stopped, stop countdown for Threads.
            countdownOrderThreadStarted = false;
            LabelCountdownOrderThread.Visibility = Visibility.Hidden;
            countdownBookingThreadStarted = false;
            LabelCountdownBookingThread.Visibility = Visibility.Hidden;
        }
        #endregion

        #region TextBox TextBox_LoadLimit - enable/disable limit button
        private void ValueLoadLimitEntered_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = TextBox_LoadLimit.Text;

            if (input.Length > 0  && input.Length <= 10)
            {
                if (input.All(char.IsDigit))
                {
                    SetLoadLimit.Visibility = Visibility.Visible;
                }
                else
                {
                    SetLoadLimit.Visibility = Visibility.Hidden;
                }                
            }
            else
            {
                SetLoadLimit.Visibility = Visibility.Hidden;
            }
        }
        #endregion      

        #region TextBox TextBoxSell_Limit - enable/disable limit button
        private void ValueSellLimitEntered_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = TextBox_SellLimit.Text;

            if (input.Length > 0)
            {
                // Allow negative input.
                if (input.Substring(0, 1).Equals("-"))
                {
                    input = input.Remove(0, 1);
                }
            }

            if (input.Length > 0 && input.Length <= 4 && input.All(char.IsDigit))
            {
                if ((Convert.ToDecimal(input) > 0) && (Convert.ToDecimal(input) <= 2000))
                {
                    SetSellLimit.Visibility = Visibility.Visible;
                }
                else
                {
                    SetSellLimit.Visibility = Visibility.Hidden;
                }
            }
            else
            {
                SetSellLimit.Visibility = Visibility.Hidden;               
            }
        }
        #endregion      

        #region TextBox TextBox_BuyLimit - enable/disable limit button
        private void ValueBuyLimitEntered_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = TextBox_BuyLimit.Text;

            if (input.Length > 0)
            {
                // Allow negative input.
                if (input.Substring(0, 1).Equals("-"))
                {
                    input = input.Remove(0, 1);
                }
            }

            if (input.Length > 0 && input.Length <= 5 && input.All(char.IsDigit))
            {
                if ((Convert.ToDecimal(input) > 0) && (Convert.ToDecimal(input) <= 2000))
                {
                    SetBuyLimit.Visibility = Visibility.Visible;
                }
                else
                {
                    SetBuyLimit.Visibility = Visibility.Hidden;
                }
            }
            else
            {
                SetBuyLimit.Visibility = Visibility.Hidden;
            }
        }
        #endregion

        #region Order Process. - Runs in Order Thread.
        /// <summary>
        /// Order Process. - Runs in Order Thread.
        /// </summary>
        private void OrderProcess()
        {
            string resultOrderProcess = string.Empty;

            try
            {
                while (true)
                {
                    resultOrderProcess = ExecuteOrderProcessOnce(
                                                                    viewModel.AppModel.SelectedArea, 
                                                                    viewModel.AppModel.SelectedLimitType, 
                                                                    viewModel.AppModel.SelectedLoadLimitValue.ToString(),
                                                                    viewModel.AppModel.SelectedSellLimitValue.ToString(),
                                                                    viewModel.AppModel.SelectedBuyLimitValue.ToString(),
                                                                    fileStartsWith
                                                              );

                    // Log results.
                    if (resultOrderProcess.Equals("OK"))
                    {
                        viewModel.AppModel.ErrorDescription = string.Empty;
                        logger.WriteInfo("Order Thread: Read Load and Limits and write Order Data to Likron was successful.");
                    }
                    else if (resultOrderProcess.StartsWith("OK"))
                    {
                        viewModel.AppModel.ErrorDescription = string.Empty;
                        logger.WriteInfo("Order Thread: Order data was already imported to Likron. Nothing was done.");
                    }
                    else
                    {
                        // Show error in GUI.
                        viewModel.AppModel.ErrorDescription = "OrderData NOT transferred to Likron. For more info see log.";
                        // Write error to log.
                        logger.WriteError("Order Thread: Read Load/Limits and write Order Data to Likron was NOT successful: " + resultOrderProcess);
                        if (viewModel.AppModel.MailInCaseOfErrorEnabled)
                        {
                            // Send mail with error information - if enabled.
                            sendMail.SendErrorMailviaSMTP(sSMTPHost, sender, receiver, title, "Process read Load and Limits and write Order Data to Likron was NOT successful: " + resultOrderProcess);
                        }
                    }

                    logger.WriteInfo("-------------------------- ORDER THREAD to read load/limit data and write order to Likron finished --------------------------");

                    // Check if flag to abort order thread is set before going to sleep.
                    if (setFlagToCancelOrderThread)
                    {
                        // Abort thread.
                        orderThread.Abort();
                    }

                    // Set thread to sleep.
                    orderThreadSleeping = true;
                    // Start Countdowns for Order Thread. 
                    timespanNextExecOrderThread = circulationTimeOrderThread -8;
                    countdownOrderThreadStarted = true;
                    Thread.Sleep(TimeSpan.FromSeconds(circulationTimeOrderThread));
                    orderThreadSleeping = false;
                    // Thread is executed, stop countdown.
                    countdownOrderThreadStarted = false;
                }
            }
            catch (ThreadAbortException)
            {
                // Set flag to true order thread is aborted.
                orderThreadStopped = true;
                logger.WriteInfo("Order Thread: Stopped.");


                // Check if other thread is stopped. Now all threads are stopped. 
                if (readAndBookTradesThreadStopped)
                {
                    ChangeStartButtonVisibility(Visibility.Visible);
                    ChangeStopButtonVisibility(Visibility.Hidden);
                    viewModel.AppModel.AppStatus = "App stopped";
                }
            }
            catch (Exception exception)
            {
                // Show error in GUI.
                viewModel.AppModel.ErrorDescription = "Exception in process read Load and Limits and write Order Data to Likron.";
                // Write error to log.
                logger.WriteInfo("Exception in process read Load and Limits and write Order Data to Likron: " + exception.Message.ToString());
                if (viewModel.AppModel.MailInCaseOfErrorEnabled)
                {
                    // Send mail with error information - if enabled.
                    sendMail.SendErrorMailviaSMTP(sSMTPHost, sender, receiver, title, "Exception in process read Load and Limits and write Order Data to Likron: " + exception.Message.ToString());
                }
            }
        }
        #endregion

        #region ReadAndBookTrades Process. - Runs in ReadAndBookTrades Thread.
        /// <summary>
        /// ReadAndBookTrades Process. - Runs in ReadAndBookTrades Thread.
        /// </summary>
        private void ReadAndBookTradesProcess()
        {
            string resultReadAndBookTradesProcess = string.Empty;
            
            try
            {
                while (true)
                {
                    resultReadAndBookTradesProcess = ExecuteReadAndBookTradesProcessOnce(
                                                                                            viewModel.AppModel.Config.pathComtraderExport,
                                                                                            viewModel.AppModel.Config.importServerAligne,
                                                                                            viewModel.AppModel.CredentialsAligne                                                                                            
                                                                                        );
                    // Log results.
                    if (resultReadAndBookTradesProcess.Equals("OK"))
                    {
                        viewModel.AppModel.ErrorDescription = string.Empty;
                        logger.WriteInfo("ReadAndBookTrades Thread: Successful.");
                    }
                    else
                    {
                        viewModel.AppModel.ErrorDescription = "Trade(s) NOT booked in Aligne. For more info see log.";
                        // Write error to log.
                        logger.WriteError("ReadAndBookTrades Thread: NOT successful: " + resultReadAndBookTradesProcess);
                        if (viewModel.AppModel.MailInCaseOfErrorEnabled)
                        {
                            // Send mail with error information - if enabled.
                            sendMail.SendErrorMailviaSMTP(sSMTPHost, sender, receiver, title, "Error in thread reading Comtrader export and booking to Aligne. <br>" + resultReadAndBookTradesProcess + ".");
                        }                    
                    }                    

                    // Check if flag to abort booking thread is set before going to sleep.
                    if (setFlagToCancelReadAndBookTradesThread)
                    {
                        // Abort thread.
                        readAndBookTradesThread.Abort();                       
                    }

                    // Set thread to sleep.
                    readAndBookTradesThreadSleeping = true;
                    // Start Countdowns for Booking Thread.
                    timespanNextExecBookingThread = circulationTimeBookingThread;
                    countdownBookingThreadStarted = true;
                    Thread.Sleep(TimeSpan.FromSeconds(circulationTimeBookingThread));
                    readAndBookTradesThreadSleeping = false;
                    // Thread is executed, stop countdown.
                    countdownBookingThreadStarted = false;                    
                }
            }
            catch (ThreadAbortException)
            {
                // Set flag to true cause thread is aborted.
                readAndBookTradesThreadStopped = true;
                logger.WriteInfo("Thread ReadAndBookTrades: Stopped.");

                // Check if other thread is stopped. Now all threads are stopped. 
                if (orderThreadStopped)
                {
                    ChangeStartButtonVisibility(Visibility.Visible);
                    ChangeStopButtonVisibility(Visibility.Hidden);
                    viewModel.AppModel.AppStatus = "App stopped";
                }                             
            }
            catch (Exception exception)
            {
                // Show error in GUI.
                viewModel.AppModel.ErrorDescription = "Error in thread reading Comtrader export and booking to Aligne.";
                // Write error to log.
                logger.WriteError("Error in thread reading Comtrader export and booking to Aligne: " + exception.Message.ToString());
                if (viewModel.AppModel.MailInCaseOfErrorEnabled)
                {
                    // Send mail with error information - if enabled.
                    sendMail.SendErrorMailviaSMTP(sSMTPHost, sender, receiver, title, "Error in thread reading Comtrader export and booking to Aligne: " + exception.Message.ToString());
                }
            }
        }
        #endregion

        #region Execute the order process once. 
        /// <summary>
        /// Execute the order process once. 
        /// </summary>
        /// <param name="area"></param>
        /// <param name="lType"></param>
        /// <param name="loadLimit"></param>
        /// <param name="fileStartsWith"></param>
        /// <returns></returns>
        private string ExecuteOrderProcessOnce(ControlArea area, LimitType lType, string loadLimit, string sellLimit, string buyLimit, string fileStartsWith)
        {
            string resultProcess = string.Empty;
            results.nameOfGeneratedFile = string.Empty;

            Range contractRange = Range.normal;
            string contract = string.Empty; //"15Q4"
            string additionalRangeInfo = string.Empty;

            logger.WriteInfo("-------------------------- ORDER THREAD to read load/limit data and write order to Likron started ----------------------------");

            // Logger for LimitType.
            switch (lType)
            {
                case LimitType.NoLimit:
                    logger.WriteInfo("Order thread: Load Limit not set.");
                    break;
                case LimitType.MWh:
                    logger.WriteInfo("Order thread: Load Limit set to " + loadLimit + " MWh.");
                    break;
                case LimitType.Percent:
                    logger.WriteInfo("Order thread: Load Limit set to " + loadLimit + " %.");
                    break;
                default:
                    // Should not happen
                    break;
            }

            // Logger for buy limit.
            if ((null == buyLimit) || buyLimit.Equals("0"))
            {
                logger.WriteInfo("Order thread: Buy Limit not set.");
            }
            else
            {
                logger.WriteInfo("Order thread: Buy Limit set to " + buyLimit + " Euro.");
            }

            // Logger for sell limit.
            if ((null == sellLimit) || sellLimit.Equals("0"))
            {
                logger.WriteInfo("Order thread: Sell Limit not set.");
            }
            else
            {
                logger.WriteInfo("Order thread: Sell Limit set to -" + sellLimit + " Euro.");
            }

            // Logger for Control Area.
            switch (area)
            {
                case ControlArea.RWE_genettet:
                    logger.WriteInfo("Order thread: Set Control Area to: " + area);
                    resultProcess = toLikron.WriteQuarterHourOrderToLikron(lType, loadLimit, sellLimit, buyLimit, fileStartsWith, ref results); 
                    break;
                case ControlArea.Regelzonenscharf:
                    logger.WriteInfo("Order thread: Set Control Area to: " + area);
                    resultProcess = toLikron.WriteQuarterHourOrderToLikronTuD(lType, loadLimit, sellLimit, buyLimit, fileStartsWith, ref results); 
                    break;
                default:
                    // Should not happen
                    break;
            }

            // Do this if table is available
            if (null != results.likronDataTable)
            {
                // Extract contract from table.
                contract = results.likronDataTable.Rows[0]["Ctrct"].ToString();
                if (contract.Length > 0)
                {
                    // TODO verify that functionality.
                    toLikron.GetContractRange(contract, ref contractRange, ref additionalRangeInfo);
                    viewModel.AppModel.ContractRange = contractRange.ToString();
                }
            }

            // Set data for GUI.
            if (results.nameOfLoadedFile.StartsWith(fileStartsWith))
            {
                viewModel.AppModel.NameLatestLoadFile = results.nameOfLoadedFile;
                // Set both names of generated files
                viewModel.AppModel.NameLatestGeneratedFile = results.nameOfGeneratedFile + " + *_TUD.csv";
                viewModel.AppModel.LastExecutionTimeWithNewLoadFile = results.lastExecutionTimeWithNewData;
                viewModel.AppModel.LastExecutionTime = results.lastExecutionTime;
                viewModel.AppModel.TableLikronData = results.likronDataTable;
                viewModel.AppModel.TableLimitData = results.limitDataTable;
                viewModel.AppModel.TableLoadData = results.loadDataTable;
            }
            else
            {
                viewModel.AppModel.LastExecutionTime = results.lastExecutionTime;
            }

            return resultProcess;
        }
        #endregion

        #region Execute the ReadAndBookTrades process once. 
        /// <summary>
        /// Execute the order process once. 
        /// </summary>
        /// <param name="pathComtraderExport"></param>
        /// <param name="importServerAligne"></param>      
        /// <param name="aligneCredentials"></param>          
        /// <returns></returns>
        private string ExecuteReadAndBookTradesProcessOnce(string pathComtraderExport, string importServerAligne, CredentialsStructureAligne aligneCredentials)
        {
            string resultReadAndBookTradesProcess = string.Empty;
            bool importDone = false;
            ResultTable resultComtraderExport = new ResultTable();
            DataTable vskTradesResult = new DataTable();
            int numberOfRows = 0;
            string dateToday = string.Empty;
            string dateToStart = string.Empty;
            DateTime dateTimeToday = DateTime.Now;

            // Set a fix date to be sure that ReadTradesFromComtraderExport() and ReadTradesFromAligneDB() are executed from the same day.
            dateToday = dateTimeToday.ToShortDateString();

            // Comtrader-Export is used to detect incomimg trades from epex interface made from this tool.
            if (viewModel.AppModel.ComtraderExportAsSourceEnabled)
            {
                logger.WriteInfo("------------------ ReadAndBookTrades THREAD to read trades from ComtraderExport and book to Aligne started -------------------");

                // Read trades from Comtrader-Export. 
                resultReadAndBookTradesProcess = ReadTradesFromComtraderExport(viewModel.AppModel.Config.pathComtraderExport, viewModel.AppModel.Config.mode, dateToday, ref resultComtraderExport);
                if (resultReadAndBookTradesProcess.Equals("OK"))
                {
                    logger.WriteInfo("ReadAndBookTrades Thread: Read Comtrader export '" + resultReadAndBookTradesProcess + "'.");
                }
                else
                {
                    logger.WriteError("ReadAndBookTrades Thread: Read Comtrader export '" + resultReadAndBookTradesProcess + "'.");
                }
                
                // Read booked VSK trades from Aligne before booking to Aligne.
                dateToStart = dateTimeToday.AddDays(-2).ToShortDateString();
                resultReadAndBookTradesProcess = ReadTradesFromAligneDB(dateToStart, ref vskTradesResult, ref numberOfRows);

                if (resultReadAndBookTradesProcess.Equals("OK"))
                {
                    logger.WriteInfo("ReadAndBookTrades Thread: Read trades from Aligne before import '" + resultReadAndBookTradesProcess + "'.");
                }
                else
                {
                    logger.WriteError("ReadAndBookTrades Thread: Read trades from Aligne before import '" + resultReadAndBookTradesProcess + "'.");
                }

                // Book trades in Aligne if not booked before.
                resultReadAndBookTradesProcess = BookTradesToAligneComtraderExport(resultComtraderExport, ref importDone, importServerAligne, aligneCredentials, vskTradesResult);
                if (resultReadAndBookTradesProcess.Equals("OK"))
                {
                    logger.WriteInfo("ReadAndBookTrades Thread: Book trades to Aligne '" + resultReadAndBookTradesProcess + "'.");
                }
                else
                {
                    logger.WriteError("ReadAndBookTrades Thread: Book trades to Aligne '" + resultReadAndBookTradesProcess + "'.");
                }

                if (resultReadAndBookTradesProcess.Equals("OK") && importDone)
                {
                    // Read booked VSK trades from Aligne after booking to Aligne.
                    resultReadAndBookTradesProcess = ReadTradesFromAligneDB(dateToday, ref vskTradesResult, ref numberOfRows);
                }

                logger.WriteInfo("ReadAndBookTrades Thread: Read trades from Aligne after import '" + resultReadAndBookTradesProcess + "'.");

                logger.WriteInfo("------------------ ReadAndBookTrades THREAD to read trades from ComtraderExport and book to Aligne finished ------------------");
            } 
            // Aligne STT database is used to detect incomimg trades from epex interface made from this tool.
            else
            {
                logger.WriteInfo("------------------ ReadAndBookTrades THREAD to read trades from Aligne STT DB and book to Aligne started -------------------");

                // Set a fix date to be sure that ReadTradesFromComtraderExport() and ReadTradesFromAligneDB() are executed from the same day.
                dateToday = DateTime.Now.ToShortDateString();

                // Read all trades from Aligne DB which do not have a matching customer trade.
                resultReadAndBookTradesProcess = ReadNotBookedVskTradesFromAligneDB(ref vskTradesResult, ref numberOfRows, dateToday);
                logger.WriteInfo("ReadAndBookTrades Thread: Number of missing customer trades '" + numberOfRows + "'.");

                resultReadAndBookTradesProcess = BookTradesToAligneDatabase(ref importDone, importServerAligne, aligneCredentials, vskTradesResult);
                                                                                 
                if (resultReadAndBookTradesProcess.Equals("OK") && importDone)
                {
                    // Read booked VSK trades from Aligne after booking to Aligne.
                    logger.WriteInfo("ReadAndBookTrades Thread: Booked all customer trades to Aligne.");                    
                }
                else
                {
                    logger.WriteError("ReadAndBookTrades Thread: No all customer trades booked to Aligne.");
                }

                logger.WriteInfo("------------------ ReadAndBookTrades THREAD to read trades from Aligne STT DB and book to Aligne finished -------------------");
            }       
            return resultReadAndBookTradesProcess;
        }
        #endregion

        #region Read trades from comtrader export.        
        /// <summary>
        /// Read trades from comtrader export.    
        /// </summary>
        /// <param name="pathComtraderExport"></param>
        /// <param name="mode"></param>
        /// <param name="executionDate"></param>
        /// <param name="resultTable"></param>        
        /// <returns></returns>
        private string ReadTradesFromComtraderExport(string pathComtraderExport, string mode, string executionDate, ref ResultTable resultTable)
        {
            string resultProcess = string.Empty;
            results.nameOfGeneratedFile = string.Empty;
            // Set start row for eex_trade_export.csv.           
            const uint startRow = 2;            

            resultTable = new ResultTable()
            {
                numberOfRows = 0,
                table = new DataTable()
            };

            InputFilter filter = new InputFilter()
            {
                // Init Filter
                date = executionDate,                
                text = TextMemo2.VSK,
                textAlternative = TextMemo2.L_PArb                
            };

            if (mode.Equals("PROD"))
            {
                // Set Trader for Likron PROD.
                filter.traderId = TraderId.TRD010;
            }
            else
            {
                // Set Trader for Likron TEST if we are not in PROD mode.
                filter.traderId = TraderId.TRD001;
            }

            resultProcess = comtrader.ExportDealsComtrader(pathComtraderExport, filter, ref resultTable, startRow);

            // Set data for GUI.
            if (resultProcess.Equals("OK"))
            {
                viewModel.AppModel.TableBookedTradesFromLikron = resultTable.table;
                viewModel.AppModel.NumberBookedTradesFromLikron = resultTable.numberOfRows.ToString();                
                ChangeForeGroundNumberOfReadTradesFromLikron(Brushes.Gray);
            }
            else
            {
                // Process of reading trades from comtrader export did not work.
                viewModel.AppModel.NumberBookedTradesFromLikron = "Error - Trades from Likron could not be read. For more info see log. ";                
                ChangeForeGroundNumberOfReadTradesFromLikron(Brushes.Red);
            }
            return resultProcess;
        }
        #endregion

        #region Read trades from Aligne DB.        
        /// <summary>
        /// Read trades from Aligne DB.        
        /// </summary>
        /// <param name="vskTradesResult"></param>
        /// <param name="numberOfRows"></param>        
        /// <returns></returns>
        private string ReadTradesFromAligneDB(string executionDate, ref DataTable vskTradesResult, ref int numberOfRows)
        {
            string retValue = string.Empty;
            string connectionString = string.Empty;               
            
            // Read connection string from config.
            connectionString = viewModel.AppModel.Config.connectionString;
    
            IDatabaseAccess dbAccess = new DatabaseAccess();
            retValue = dbAccess.ReadBookedVskTradesFromAligne(ref vskTradesResult, ref numberOfRows, connectionString, executionDate);

            // Set data for GUI.
            if (retValue.Equals("OK"))
            {
                viewModel.AppModel.TableBookedTradesToAligne = vskTradesResult;
                viewModel.AppModel.NumberBookedTradesToAligne = numberOfRows.ToString();
                ChangeForeGroundNumberOfReadTradesToAligne(Brushes.Gray);
            }
            else
            {
                // Process of reading trades from comtrader export did not work.
                viewModel.AppModel.NumberBookedTradesToAligne = "Error - Trades from Aligne could not be read.For more info see log. ";
                ChangeForeGroundNumberOfReadTradesToAligne(Brushes.Red);
            }

            return retValue;
        }
        #endregion

        #region Read VSK trades from Aligne DB which do not have customer trade.        
        /// <summary>
        /// Read VSK trades from Aligne DB which do not have customer trade.        
        /// </summary>
        /// <param name="vskMissingCustomerTrades"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="executionDate"></param>
        /// <returns></returns>
        private string ReadNotBookedVskTradesFromAligneDB(ref DataTable vskMissingCustomerTrades, ref int numberOfRows, string executionDate)
        {
            string retValue = string.Empty;
            string connectionString = string.Empty;

            // Read connection string from config.
            connectionString = viewModel.AppModel.Config.connectionString;

            IDatabaseAccess dbAccess = new DatabaseAccess();
            retValue = dbAccess.ReadNotBookedVskTrades(ref vskMissingCustomerTrades, ref numberOfRows, connectionString, executionDate);

            // Set data for GUI.
            if (retValue.Equals("OK"))
            {
                viewModel.AppModel.TableBookedTradesToAligne = vskMissingCustomerTrades;
                viewModel.AppModel.NumberBookedTradesToAligne = numberOfRows.ToString();
                ChangeForeGroundNumberOfReadTradesToAligne(Brushes.Gray);
            }
            else
            {
                // Process of reading trades from comtrader export did not work.
                viewModel.AppModel.NumberBookedTradesToAligne = "Error - Trades from Aligne could not be read.For more info see log. ";
                ChangeForeGroundNumberOfReadTradesToAligne(Brushes.Red);
            }
            return retValue;
        }
        #endregion

        #region Book trades to Aligne - Reference is Comtrader-Export.        
        /// <summary>
        /// Book trades to Aligne - Reference is Comtrader-Export.
        /// </summary>
        /// <param name="resultComExport"></param>
        /// <param name="importDone"></param>
        /// <param name="importServer"></param>
        /// <param name="aligneCredentials"></param>
        /// <param name="vskTradesResult"></param>        
        /// <returns></returns>
        private string BookTradesToAligneComtraderExport(ResultTable resultComExport, ref bool importDone, string importServer, CredentialsStructureAligne aligneCredentials, DataTable vskTradesResult)
        {
            string resultProcess = string.Empty;                
                        
            InputDataComtraderExport input = new InputDataComtraderExport();
            List<OutputData> outputDataList = new List<OutputData>();           
                       
            input.date = DateTime.Today.ToShortDateString();
            input.trader = aligneCredentials.aligneUser;
            input.password = aligneCredentials.password;
            input.pwEnCrypton = ViertelStdToolLib.Aligne.Importer.DealStructure.PwEnCrypton.PLAIN;

            input.resultComtraderExport = resultComExport;
            input.server = importServer;
                        
            // Book trades via EXE. Ignore Trades already booked to Aligne.
            resultProcess = bookTrades.DetectTradesComTraderExportAndBookToAligne(input, ref outputDataList, ref importDone, vskTradesResult, true);
            
            return resultProcess;
        }
        #endregion       

        #region Book trades to Aligne - Reference is Database.        
        /// <summary>
        /// Book trades to Aligne - Reference is Database.
        /// </summary>       
        /// <param name="importDone"></param>
        /// <param name="importServer"></param>
        /// <param name="aligneCredentials"></param>
        /// <param name="vskTradesResult"></param>        
        /// <returns></returns>
        private string BookTradesToAligneDatabase(ref bool importDone, string importServer, CredentialsStructureAligne aligneCredentials, DataTable vskTradesResult)
        {
            string resultProcess = string.Empty;
            List<OutputData> outputDataList = new List<OutputData>();

            InputDataXmlAifStt input = new InputDataXmlAifStt()
            {
                date = DateTime.Today.ToShortDateString(),
                trader = aligneCredentials.aligneUser,
                password = aligneCredentials.password,
                pwEnCrypton = ViertelStdToolLib.Aligne.Importer.DealStructure.PwEnCrypton.PLAIN,
                server = importServer,
                connectionStringStt = viewModel.AppModel.Config.connectionStringStt,
                vskMissingCustomerTrades = vskTradesResult
            };
                       
            // Book trades via EXE. Ignore Trades already booked to Aligne.
            resultProcess = bookTrades.DetectTradesAifSttXmltAndBookToAligne(input, ref outputDataList, ref importDone, true);
           
            return resultProcess;
        }
        #endregion       

        #region Use Invoke to alter GUI element for trades made in Likron in other thread.
        /// <summary>
        /// Use Invoke to alter GUI element in other thread.
        /// </summary>
        /// <param name="color"></param>
        private void ChangeForeGroundNumberOfReadTradesFromLikron(Brush color)
        {
            Dispatcher.Invoke(() =>
            {
                NumberOfReadTradesFromLikron.Foreground = color;
            });
        }
        #endregion

        #region Use Invoke to alter GUI element for booked trades to Aligne in other thread.
        /// <summary>
        /// Use Invoke to alter GUI element in other thread.
        /// </summary>
        /// <param name="color"></param>
        private void ChangeForeGroundNumberOfReadTradesToAligne(Brush color)
        {
            Dispatcher.Invoke(() =>
            {
                NumberOfBookedTradesToAligne.Foreground = color;
            });
        }
        #endregion 

        #region Use Invoke to change visibility of start button.
        /// <summary>
        /// Use Invoke to change visibility of start button.
        /// </summary>
        /// <param name="visibility"></param>
        private void ChangeStartButtonVisibility(Visibility visibility)
        {
            Dispatcher.Invoke(() =>
            {
                StartApplication.Visibility = visibility;
            });
        }
        #endregion

        #region Use Invoke to change visibility of stop button.
        /// <summary>
        /// Use Invoke to change visibility of stop button.
        /// </summary>
        /// <param name="visibility"></param>
        private void ChangeStopButtonVisibility(Visibility visibility)
        {
            Dispatcher.Invoke(() =>
            {
                StopApplication.Visibility = visibility;
            });
        }
        #endregion

        #region AppView Closing
        void AppView_Closing(object sender, CancelEventArgs e)
        {
            // Log that application is closed.
            logger.WriteInfo("Application closed.");

            // Exit the application.
            Environment.Exit(Environment.ExitCode);
        }
        #endregion
    }
}


