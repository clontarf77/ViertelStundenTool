using Prism.Mvvm;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Security.Principal;
using ViertelStdTool.ConfigurationHandler;
using ViertelStdTool.CredentialsHandler;
using ViertelStdTool.Likron;
using ViertelStdTool.Log;

namespace LimitOrders15minGUI.Models
{
    class AppModel : BindableBase
    {
        // Selected values in combobox
        private LimitType selectedLimitType = new LimitType();
        private ControlArea area = new ControlArea();
        // Inserted Limit
        private decimal selectedLoadLimitValue = 0;
        private decimal selectedSellLimitValue = 0;
        private decimal selectedBuyLimitValue = 0;
        private ConfigurationStructure config = new ConfigurationStructure();

        // Name of latest data for GUI.
        private string nameLatestLoadFile = "no new file read";
        private string nameLatestGeneratedFile = "no new file generated";
        private string lastExecutionTimeWithNewLoadFile = "no new load file detected";
        private string lastExecutionTime = "not yet executed - runs every 3 minutes";
        private string numberBookedTradesFromLikron = string.Empty;
        private string numberBookedTradesToAligne = string.Empty;
        private string errorDescription = string.Empty;
        private string contractRange = string.Empty;
        private string appStatus = "App stopped";
        private bool comtraderExportAsSourceEnabled = true;           
        private bool mailInCaseOfErrorEnabled = true;
        private DataTable tableLoadData = new DataTable();
        private DataTable tableLimitData = new DataTable();
        private DataTable tableLikronData = new DataTable();
        private DataTable tableBookedTradesFromLikron = new DataTable();
        private DataTable tableBookedTradesToAligne = new DataTable();

        // Logger
        private readonly INLogger logger = new NLogger();

        #region Constructor
        // Constructor
        public AppModel()
        {
            ControlAreaList.Add(ControlArea.RWE_genettet.ToString());
            ControlAreaList.Add(ControlArea.Regelzonenscharf.ToString());

            LimitTypeList.Add(LimitType.NoLimit.ToString());
            LimitTypeList.Add(LimitType.MWh.ToString());
            LimitTypeList.Add(LimitType.Percent.ToString());            
        }
        #endregion

        #region Getter for windowTitle
        public string WindowTitle { get; private set; } = "ViertelStdTool " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + "        " + WindowsIdentity.GetCurrent().Name.Replace("KONZERN\\", "");
        #endregion

        #region Getter for UserIdentifierWinLogin
        public string UserIdentifierWinLogin { get; } = WindowsIdentity.GetCurrent().Name.Replace("KONZERN\\", "");
        #endregion       

        #region Getter for AligneUserCredentialsFile
        public string UserIdentifierCredentials => CredentialsAligne.adUser;
        #endregion

        #region Getter for AlignePasswordCredentialsFile
        public string AlignUserCredentials => CredentialsAligne.aligneUser;

        #region Getter for AlignePasswordCredentialsFile
        public string AlignePasswordCredentials => CredentialsAligne.password;
        #endregion

        #region Getter for Path of Aligne Credetials File
        public string NameAligneCredentialFile => config.fileNameCredentialsAligne;
        #endregion
        #endregion

        #region Getter for SetLoadLimit
        public string SetLoadLimit { get; private set; } = "No Limit";
        #endregion

        #region Getter for SetSellLimit
        public string SetSellLimit { get; private set; } = "LimitData used";
        #endregion

        #region Getter for SetBuyLimit
        public string SetBuyLimit { get; private set; } = "LimitData used";
        #endregion

        #region Getter for control area list.
        public List<string> ControlAreaList { get; } = new List<string>();
        #endregion

        #region Getter for limit type list.
        public List<string> LimitTypeList { get; } = new List<string>();
        #endregion

        #region Getter/Setter for config.
        public ConfigurationStructure Config
        {
            get => config;
            set
            {
                config = value;
                WindowTitle = "ViertelStdTool " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + "        " + WindowsIdentity.GetCurrent().Name.Replace("KONZERN\\", "") + "        " + config.mode;

                RaisePropertyChanged("WindowTitle");
            }
         }
        #endregion

        #region Getter/Setter for credentials.
        public CredentialsStructureAligne CredentialsAligne { get; set; } = new CredentialsStructureAligne();
        #endregion

        #region Getter/Setter for number of booked trades from Likron to Aligne.
        public string NumberBookedTradesFromLikron
        {
            get => numberBookedTradesFromLikron;
            set
            {
                SetProperty(ref numberBookedTradesFromLikron, value);
            }
        }
        #endregion

        #region Getter/Setter for number of booked trades from VSK to Aligne.
        public string NumberBookedTradesToAligne
        {
            get => numberBookedTradesToAligne;
            set
            {
                SetProperty(ref numberBookedTradesToAligne, value);
            }
        }
        #endregion

        #region Getter/Setter for selected limit type.
        public LimitType SelectedLimitType
        {
            get => selectedLimitType;
            set
            {
                SetProperty(ref selectedLimitType, value);               
            }
        }
        #endregion

        #region Getter/Setter for selected load limit value.
        public decimal SelectedLoadLimitValue
        {
            get => selectedLoadLimitValue;
            set
            {
                SetProperty(ref selectedLoadLimitValue, value);

                if (selectedLoadLimitValue > 0)
                {
                    switch (selectedLimitType)
                    {
                        case LimitType.MWh:
                            SetLoadLimit = selectedLoadLimitValue.ToString() + " MWh";
                            break;
                        case LimitType.Percent:
                            SetLoadLimit = selectedLoadLimitValue.ToString() + " %";
                            break;
                    }
                }
                else
                {
                    SetLoadLimit = "No Limit";
                }
               
                RaisePropertyChanged("SetLoadLimit");
            }
        }
        #endregion

        #region Getter/Setter for selected sell limit value.
        public decimal SelectedSellLimitValue
        {
            get => selectedSellLimitValue;
            set
            {
                SetProperty(ref selectedSellLimitValue, value);

                if (selectedSellLimitValue != 0)
                {
                    SetSellLimit = selectedSellLimitValue.ToString() + " Euro";
                }
                else
                {
                    SetSellLimit = "LimitData used";
                }

                RaisePropertyChanged("SetSellLimit");
            }
        }
        #endregion

        #region Getter/Setter for selected buy limit value.
        public decimal SelectedBuyLimitValue
        {
            get => selectedBuyLimitValue;
            set
            {
                SetProperty(ref selectedBuyLimitValue, value);

                if (selectedBuyLimitValue != 0)
                {
                    SetBuyLimit = selectedBuyLimitValue.ToString() + " Euro";
                }
                else
                {
                    SetBuyLimit = "LimitData used";
                }

                RaisePropertyChanged("SetBuyLimit");
            }
        }
        #endregion

        #region Getter/Setter for name of latest load file.
        public string NameLatestLoadFile
        {
            get => nameLatestLoadFile;
            set
            {
                SetProperty(ref nameLatestLoadFile, value);               
            }
        }
        #endregion

        #region Getter/Setter for name of latest generated file.
        public string NameLatestGeneratedFile
        {
            get => nameLatestGeneratedFile;
            set
            {
                SetProperty(ref nameLatestGeneratedFile, value);
            }            
        }
        #endregion

        #region Getter/Setter for lates excution time with new load file.
        public string LastExecutionTimeWithNewLoadFile
        {
            get => lastExecutionTimeWithNewLoadFile;
            set
            {
                SetProperty(ref lastExecutionTimeWithNewLoadFile, value);
            }
        }
        #endregion

        #region Getter/Setter for lates excution time in general.
        public string LastExecutionTime
        {
            get => lastExecutionTime;
            set
            {
                SetProperty(ref lastExecutionTime, value);
            }
        }
        #endregion

        #region Setter for table with latest load data.
        public DataTable TableLoadData
        {
            set
            {
                string temp = string.Empty;
                tableLoadData = value;

                // If column nane has char / remove it.
                foreach (DataColumn column in tableLoadData.Columns)
                {
                    if (column.ColumnName.Contains("/"))
                    {
                        temp = column.ColumnName;
                        temp = temp.Replace('/', '_');
                        column.ColumnName = temp;
                        break;
                    }
                }
                
                // Tell view that ViewLoadData has been changed.
                RaisePropertyChanged("ViewLoadData");
            }
        }
        #endregion

        #region Setter for table with latest limit data.
        public DataTable TableLimitData
        {
            set
            {
                tableLimitData = value;
                // Tell view that ViewLimitData has been changed.
                RaisePropertyChanged("ViewLimitData");
            }
        }
        #endregion

        #region Setter for table with latest likron/order data.
        public DataTable TableLikronData
        {
            set
            {
                tableLikronData = value;
                string temp = string.Empty;

                // If column nane has char / remove it.
                foreach (DataColumn column in tableLikronData.Columns)
                {
                    if (column.ColumnName.Contains("/"))
                    {
                        temp = column.ColumnName;
                        temp = temp.Replace('/', '_');
                        column.ColumnName = temp;
                        break;
                    }
                }

                // Tell view that ViewLikronData has been changed.
                RaisePropertyChanged("ViewLikronData");
            }
        }
        #endregion

        #region Setter for table with booked trades from Likron.
        public DataTable TableBookedTradesFromLikron
        {
            set
            {
                tableBookedTradesFromLikron = value;
                string temp = string.Empty;

                // If column nane has char / remove it.
                foreach (DataColumn column in tableBookedTradesFromLikron.Columns)
                {
                    if (column.ColumnName.Contains("/"))
                    {
                        temp = column.ColumnName;
                        temp = temp.Replace('/', '_');
                        column.ColumnName = temp;
                        break;
                    }
                }

                // Tell view that ViewLikronData has been changed.
                RaisePropertyChanged("ViewBookedTradesFromLikron");
            }
        }
        #endregion

        #region Setter for table with booked trades to Aligne.
        public DataTable TableBookedTradesToAligne
        {
            set
            {
                tableBookedTradesToAligne = value;
                string temp = string.Empty;

                // If column nane has char / remove it.
                foreach (DataColumn column in tableBookedTradesToAligne.Columns)
                {
                    if (column.ColumnName.Contains("/"))
                    {
                        temp = column.ColumnName;
                        temp = temp.Replace('/', '_');
                        column.ColumnName = temp;
                        break;
                    }
                }

                // Tell view that ViewLikronData has been changed.
                RaisePropertyChanged("ViewBookedTradesToAligne");
            }
        }
        #endregion    

        #region Getter for dataview with latest load data.
        public DataView ViewLoadData
        {
            get => tableLoadData.DefaultView;
        }
        #endregion

        #region Getter for dataview with latest limit data.
        public DataView ViewLimitData
        {
            get => tableLimitData.DefaultView;
        }
        #endregion

        #region Getter for dataview with latest likron/order data.
        public DataView ViewLikronData
        {
            get => tableLikronData.DefaultView;
        }
        #endregion

        #region Getter for dataview with booked from Likron.
        public DataView ViewBookedTradesFromLikron
        {
            get => tableBookedTradesFromLikron.DefaultView;
        }
        #endregion

        #region Getter for dataview with booked to Aligne.
        public DataView ViewBookedTradesToAligne
        {
            get => tableBookedTradesToAligne.DefaultView;
        }
        #endregion

        #region Getter/Setter for selected control area.
        public ControlArea SelectedArea
        {
            get => area;
            set
            {
                SetProperty(ref area, value);
            }           
        }
        #endregion

        #region Getter/Setter for error description.
        public string ErrorDescription
        {
            get => errorDescription;
            set
            {
                SetProperty(ref errorDescription, value);
            }
        }
        #endregion

        #region Getter/Setter for product range.
        public string ContractRange
        {
            get => contractRange;

            set
            {
                SetProperty(ref contractRange, value);
            }
        }
        #endregion

        #region Getter/Setter for application status.
        public string AppStatus
        {
            get => appStatus;

            set
            {
                SetProperty(ref appStatus, value);
            }
        }
        #endregion

        #region This is the mail address wer in cas of an error mails are sent.
        public string MailInCaseOfError { get; set; } = string.Empty;
        #endregion       

        #region This is the mail address were in case of an error mails are sent.
        public bool MailInCaseOfErrorEnabled
        {
            get => mailInCaseOfErrorEnabled;

            set
            {
                SetProperty(ref mailInCaseOfErrorEnabled, value);

                if (mailInCaseOfErrorEnabled)
                {
                    logger.WriteInfo("Mail in case of an error enabled.");
                }
                else
                {
                    logger.WriteInfo("Mail in case of an error disabled.");
                }
            }
        }
        #endregion

        #region This is the source of the Epex-Trade detection.
        public bool ComtraderExportAsSourceEnabled
        {
            get => comtraderExportAsSourceEnabled;

            set
            {
                SetProperty(ref comtraderExportAsSourceEnabled, value);

                if (comtraderExportAsSourceEnabled)
                {
                    logger.WriteInfo("Epex trade detection - set source Comtrader-Export.");
                }
                else
                {
                    logger.WriteInfo("Epex trade detection - set source Aligne STT Database.");
                }
            }
        }
        #endregion
    }
}




