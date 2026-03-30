using PasswordEnDeCrypter;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ViertelStdTool.Log;
using ViertelStdToolLib.Aligne.Importer.DealStructure;

namespace ViertelStdTool.AligneImporter
{
    public class AligneImporter : IAligneImporter
    {
        private INLogger logger = new NLogger();
        private readonly string setUser = string.Empty;
        private readonly string setPassword = string.Empty;
        private readonly string setServer = string.Empty;
       
        #region Constructor - Init object to import files to Aligne.
        /// <summary>
        /// Init object to import files to Aligne.
        /// </summary>
        /// <param name="server"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        public AligneImporter(string server, string user, string password)
        {
            setUser = user;
            setPassword = password;
            setServer = server;            
        }
        #endregion

        #region Import Deal to Aligne use CSV/YML as import format.
        /// <summary>
        /// ImportDeal to Algne. 
        /// </summary>                
        /// <param name="fileName"></param> 
        /// <param name="path"></param>    
        /// <param name="importFileType"></param> 
        /// <param name="encryptionMode"></param>
        /// <param name="zKey"></param> 
        /// <param name="importerSecurityTurnedOn">We need different arguments if importer security is turned on.</param> 
        /// <returns>String 'success' if OK</returns>   
        public string ImportDeal(string fileName, string path, ImportFileType importFileType, PwEnCrypton encryptionMode, ref string zKey, bool importerSecurityTurnedOn = false)
        {
            string retValue = string.Empty;
            string source = "";
            string port = "25000";
            string password = string.Empty;
            bool passwordValid = false;
            string dateVarCsv = DateTime.Now.Date.ToString("dd'/'MM'/'yyyy");
            string pathExecutable = path + @"\" + "Convert2XML.exe";
            int timeout = 3000; //ms

            switch (encryptionMode)
            {
                case PwEnCrypton.PW_MANAGER:
                    try
                    {
                        // Decode password.
                        password = Encoder.DecryptString(setPassword, "PESTRASIRUSE");
                        //password = encrypted ? Encoder.DecryptString(setPassword, "PESTRASIRUSE") : setPassword;
                        passwordValid = true;
                    }
                    catch (Exception)
                    {
                        // Catch exception password could not be decoded.
                        retValue = "Password could not be decoded";
                        passwordValid = false;
                    }

                    break;

                case PwEnCrypton.SIMPLE_PASS:
                    password = "*" + setPassword;
                    passwordValid = true;
                    break;

                case PwEnCrypton.PLAIN:
                    password = setPassword;
                    passwordValid = true;
                    break;
            }

            if (passwordValid)
            {
                try
                {
                    // Use ProcessStartInfo class to start exe
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        FileName = pathExecutable,
                        WindowStyle = ProcessWindowStyle.Hidden                        
                    };

                    switch (importFileType)
                    {
                        case ImportFileType.XML:
                            if (importerSecurityTurnedOn)
                            {
                                //This needed if importer security is turned on i.e. IMPSECON gloc = Y. 
                                startInfo.Arguments = "-x, -dDD/MM/YYYY -h" + setServer + " -p" + port + " -impLogin" + setUser + " -impPcrypt" + password + " -l" + path + "\\" + fileName + ".log " + path + "\\" + fileName + ".xml";
                            }
                            else
                            {
                                startInfo.Arguments = "-x, -dDD/MM/YYYY -h" + setServer + " -p" + port + " -impUser" + setUser + " -impPcrypt" + password + " -l" + path + "\\" + fileName + ".log " + path + "\\" + fileName + ".xml";
                            }
                            break;

                        case ImportFileType.CSV:
                            if (importerSecurityTurnedOn)
                            {
                                //This needed if importer security is turned on i.e. IMPSECON gloc = Y. 
                                startInfo.Arguments = "-dDD/MM/YYYY -S" + dateVarCsv + " -h" + setServer + " -p" + port + " -s" + source + " -impLogin" + setUser + " -impPcrypt" + password + " -l" + path + "\\" + fileName + ".log " + path + "\\" + fileName + ".csv";
                            }
                            else
                            {
                                startInfo.Arguments = "-dDD/MM/YYYY -S" + dateVarCsv + " -h" + setServer + " -p" + port + " -s" + source + " -impUser" + setUser + " -impPcrypt" + password + " -l" + path + "\\" + fileName + ".log " + path + "\\" + fileName + ".csv";
                            }
                            break;
                    }                  
                    
                    // Init process.
                    Process exeProcess = new Process
                    {
                        StartInfo = startInfo
                    };

                    // Start the process.
                    exeProcess.Start();

                    // Creates task to wait for process exit using timeout
                    Task waitForExeProcess = WaitForExeProcessAsync(exeProcess, timeout);

                    // Wait till process is done in own Thread.
                    waitForExeProcess.Wait();

                    // Set return Value if finished.
                    retValue = waitForExeProcess.Status.ToString();

                    if (waitForExeProcess.IsCompleted)
                    {
                        // Read result of trade import.
                        retValue = ReadLogDataImport(path, fileName, ref zKey);
                    }
                }
                catch (Exception exception)
                {
                    exception.Data.Add("ImportDeal", "Exception in " + (MethodBase.GetCurrentMethod().Name) + "(): ");
                    logger.WriteError("Exception in : " + (MethodBase.GetCurrentMethod().Name) + "(): " + exception.Message.ToString());
                    retValue = "NOK - Exception in : " + (MethodBase.GetCurrentMethod().Name) + "(): " + exception.Message.ToString(); 
                    //throw exception;
                }
            }

            return retValue;
        }
        #endregion     

        #region Read log data for trade import.
        /// <summary>
        /// This method returns data read from the log as result of the trade import.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <param name="zKey"></param>
        /// <returns>success if OK.</returns>
        private string ReadLogDataImport(string path, string fileName, ref string zKey)
        {
            string pathToLog = string.Empty;
            string logData = string.Empty;
            string retValue = string.Empty;

            fileName = fileName.Replace(".csv", "") + ".log";
            pathToLog = Path.Combine(path, fileName);

            Task fileReady = IsFileReadyAsync(pathToLog);
            fileReady.Wait(30000); // Just wait for 30 seconds.

            var lastLineLogData = File.ReadLines(pathToLog).Last();

            string[] tokens = lastLineLogData.Split(',');
            retValue = tokens[4];
            
            if (retValue.Equals("success"))
            {        
                foreach(string part in tokens)
                {
                    if(part.StartsWith("ZKEY="))
                    {
                        zKey = part.Replace("ZKEY=", "");
                        break;
                    }
                }
            }    
            else if (retValue.Equals("general failure"))
            {
                retValue = tokens[5].Remove(0, 10);
                if (tokens.Length == 6)
                {
                    retValue = retValue.Substring(0, retValue.Length - 1);
                }
                else if (tokens.Length == 7)
                {
                    retValue = retValue + tokens[6];
                    retValue = retValue.Substring(0, retValue.Length - 1);
                }
                //ZKEY could not be read. Return empty string.
                zKey = string.Empty;
            }
            else 
            {
                //ZKEY could not be read. Return empty string.
                zKey = string.Empty;
            }

            return retValue;
        }
        #endregion

        #region Wait in separate thread for process to finish.
        /// <summary>
        /// Sets the period of time to wait for the associated process to exit,
        /// and blocks the current thread of execution until the time has elapsed or the process has exited. 
        /// Thus the wait is started in separate thread. It's still blocking, but it runs on a background thread. 
        /// </summary>
        /// <param name="process"></param>
        /// <param name="timeout">in ms</param>
        /// <returns>Task which is waiting.</returns>
        private static async Task WaitForExeProcessAsync(Process process, int timeout)
        {
            // Do the wait in a async. Separate task.
            await Task.Run(() => process.WaitForExit(timeout));
        }
        #endregion

        #region Wait in separate thread till file becomes accessible.
        /// <summary>
        ///  Wait in separate thread till file becomes accessible.
        /// </summary>
        /// <param name="pathToLog"></param>
        /// <returns>Task which is waiting.</returns>
        private static async Task IsFileReadyAsync(string pathToLog)
        {
            await Task.Run(() =>
            {
                if (!File.Exists(pathToLog))
                {
                    throw new IOException("File does not exist!");
                }

                bool isReady = false;

                while (!isReady)
                {
                    // If the file can be opened for exclusive access it means that the file
                    // is no longer locked by another process.
                    try
                    {
                        using (FileStream inputStream = File.Open(pathToLog, FileMode.Open, FileAccess.Read, FileShare.None))
                        {
                            isReady = inputStream.Length > 0;
                        }
                    }
                    catch (Exception e)
                    {
                        // Check if the exception is related to an IO error.
                        if (e.GetType() == typeof(IOException))
                        {
                            isReady = false;
                        }
                        else
                        {
                            // Rethrow the exception as it's not an exclusively-opened-exception.
                            throw;
                        }
                    }
                }
            });
        }
        #endregion
    }
}

