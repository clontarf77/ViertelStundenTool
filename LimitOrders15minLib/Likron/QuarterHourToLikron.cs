using System;
using System.IO;
using System.Reflection;
using ViertelStdTool.ConfigurationHandler;
using ViertelStdTool.Log;
using ViertelStdTool.SFTPManager;

namespace ViertelStdTool.Likron
{
    public class QuarterHourToLikron : IQuarterHourToLikron
    {
        private readonly INLogger logger = new NLogger();
               
        private OrderDataGenerationData likronData = new OrderDataGenerationData();

        private ISFTP sFTP_limitFile;
        private ISFTP sFTP_loadFile;
        private ISFTP sFTP_Likron;

        private ConfigurationStructure config;
        private IGenerateOrderData likron;

        #region Do the init.       
        /// <summary>
        /// Do the init.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns>Ok in case of success.</returns>
        public string Init(ConfigurationStructure configuration)
        {
            string retValue = "OK";

            config = new ConfigurationStructure();
            likron = new GenerateOrderData();           

            try
            {
                // Keep configuration in object.
                config = configuration;

                // Init sFTP limit-file.
                sFTP_limitFile = new SFTP(config.hostSftpLimitFile, config.portSftpLimitFile, config.userSftpLimitFile, config.passwordSftpLimitFile, config.sshKeySftpLimitFile);

                // Init sFTP load-file.
                sFTP_loadFile = new SFTP(config.hostSftpLoadFile, config.portSftpLoadFile, config.userSftpLoadFile, config.passwordSftpLoadFile, config.sshKeySftpLoadFile);

                // Init MVV VSK sFTP.
                string passphrasePrivateKey = "Ze7lX9XbBmzG9yvpcm/irb3hOLREd6Kgs07yi6ZkWFE=";
                string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(QuarterHourToLikron)).CodeBase);
                #region Workaround if started with long path. If not substituted path will exceed 260 chars and exception will be thrown.
                if (pathExecutable.StartsWith(@"file:\konzern\dfs\firmen"))
                {
                    pathExecutable = pathExecutable.Replace(@"file:\konzern\dfs\firmen", "T: for EOD");
                    logger.WriteInfo("ITA Evo: Replaces path 'file:\\konzern\\dfs\\firmen' with 'T:' for EOD");
                }
                else if (pathExecutable.StartsWith("file:\\"))
                {
                    pathExecutable = pathExecutable.Remove(0, 6);
                }
                else if (pathExecutable.StartsWith(@"\\konzern\dfs\Firmen"))
                {
                    pathExecutable = pathExecutable.Replace(@"\\konzern\dfs\Firmen", "T: for EOD");
                    logger.WriteInfo("ITA Evo: Replaces path '\\\\konzern\\dfs\\Firmen' with 'T:' for EOD");
                }
                #endregion
                string pathPrivateKey = pathExecutable + "\\SFTP\\PrivateKey\\privateKeyLikron.ppk";
                sFTP_Likron = new SFTP(config.hostLikronSftp, config.portLikronSftp, config.userLikronSftp, config.passwordLikronSftp, config.sshKeyLikronSftp, pathPrivateKey, passphrasePrivateKey);                
            }                                                   
            catch (Exception exception)
            {
                // Exception set info.
                retValue = "NOK - " + exception.Message.ToString();
                // Do the logging.
                logger.WriteError("Exception in : " + (MethodBase.GetCurrentMethod().Name) + "(): " + exception.Message.ToString() + ".");
            }
            return retValue;
        }
        #endregion

        #region Write the quarter hour orders to Likron.
        /// <summary>
        /// Write the quarter hour orders to Likron.
        /// </summary>
        /// <param name="lType"></param>
        /// <param name="loadLimit"></param> 
        /// <param name="sellLimit"></param>
        /// <param name="buyLimit"></param>
        /// <param name="fileStartsWith"></param>
        /// <param name="results"></param>
        /// <returns>Ok in case of success.</returns>
        public string WriteQuarterHourOrderToLikron(LimitType lType, string loadLimit, string sellLimit, string buyLimit, string fileStartsWith, ref ResultsQuarterHourToLikron results)
        {
            string retValue = "OK";
            results.nameOfLoadedFile = string.Empty;

            try
            {
                //Download latest file (if not loaded before) from sFTP server which starts with given filter name.
                retValue = sFTP_loadFile.DownloadLatesFileUseFilter(config.remotePathLoadFileMvvSftp, config.ownPath, fileStartsWith, ref results.nameOfLoadedFile);

                // Just read limit file if new Load-File was read before.
                if (results.nameOfLoadedFile.StartsWith(fileStartsWith))
                {
                    //Read Limit file from SFTP.               
                    retValue = sFTP_limitFile.DownloadFile(config.remotePathLimitFileMvvSftp, config.ownPath);

                    // If a new file exists create order data for likron and transfer file to likron.
                    // Just do it if a new load file exists on sFTP and limit file was read.
                    if (retValue.Equals("OK"))
                    {
                        likronData = new OrderDataGenerationData()
                        {
                            pathLoadCSV = config.ownPath,
                            pathOrderLikron = config.ownPath,
                            pathLimitExcel = config.ownPath + "Limit_VSK_15Min.xlsx",
                            tableName = "Limit_VSK_15Min",
                            sumQuantityIsZero = false,
                            limitType = lType,
                            loadLimit = loadLimit,
                            sellLimit = sellLimit,
                            buyLimit = buyLimit,
                            validityEnd = ValidityEnd.empty
                        };

                        // Load files are uploaded to sFTP each 15min (02,17,32,47).
                        // If file was already read file is ignored.
                        likronData.pathLoadCSV += results.nameOfLoadedFile;

                        // Generate Order Data for Likron                    
                        likron.GenerateCsvForLikron(ref likronData); 
                        results.nameOfGeneratedFile = likronData.nameOfGeneratedFile;
                        results.nameOfGeneratedFileSmaller30Min = likronData.nameOfGeneratedFileSmaller30Min; 
                        results.likronDataTable = likronData.likronDataTableAll;
                        results.limitDataTable = likronData.limitDataTable;
                        results.loadDataTable = likronData.loadDataTable;

                        // upload order data to likron for (this is for leadtimes >= 30 min)
                        if (File.Exists(likronData.pathOrderLikron))
                        {
                            retValue = sFTP_Likron.UploadFile(likronData.pathOrderLikron, config.remotePathLikronSftp);
                        }

                        // upload order data to likron for (this is for leadtimes < 30 min)
                        if (File.Exists(likronData.pathOrderLikronSmaller30Min))
                        {
                            retValue = sFTP_Likron.UploadFile(likronData.pathOrderLikronSmaller30Min, config.remotePathLikronSftpSmaller30Min);
                        }

                        if (retValue.Equals("OK"))
                        {
                            // Likron SFTP Upload did work.

                            // Set time of last execution with new data.
                            results.lastExecutionTimeWithNewData = DateTime.Now.ToLongTimeString();                           
                        }                       

                        // Set time of last execution in general.
                        results.lastExecutionTime = DateTime.Now.ToLongTimeString();
                    }
                }
                else
                {
                    // Set time of last execution in general.
                    results.lastExecutionTime = DateTime.Now.ToLongTimeString();
                }
            }
            catch (Exception exception)
            {
                // Exception set info.
                retValue = "NOK - " + exception.Message.ToString();
                // Do the logging.
                logger.WriteError("Exception in : " + (MethodBase.GetCurrentMethod().Name) + "(): " + exception.Message.ToString() + ".");                
            }
            return retValue;
        }
        #endregion

        #region Write the quarter hour orders to Likron for TuD.
        /// <summary>
        /// Write the quarter hour orders to Likron for TuD.
        /// </summary>
        /// <param name="lType"></param>
        /// <param name="loadLimit"></param>  
        /// <param name="sellLimit"></param>
        /// <param name="buyLimit"></param>
        /// <param name="fileStartsWith"></param>
        /// <param name="results"></param>
        /// <returns>Ok in case of success.</returns>
        public string WriteQuarterHourOrderToLikronTuD(LimitType lType, string loadLimit, string sellLimit, string buyLimit, string fileStartsWith, ref ResultsQuarterHourToLikron results)
        {
            // TODO adapt to WriteQuarterHourOrderToLikron 

            string retValue = "OK";
            results.nameOfLoadedFile = string.Empty;

            try
            {
                //Download latest file (if not loaded before) from sFTP server which starts with given filter name..
                retValue = sFTP_loadFile.DownloadLatesFileUseFilter(config.remotePathLoadFileMvvSftp, config.ownPath, fileStartsWith, ref results.nameOfLoadedFile);

                // Just read limit file if new Load-File was read before.
                if (results.nameOfLoadedFile.StartsWith(fileStartsWith))
                {
                    //Read Limit file from SFTP.               
                    retValue = sFTP_limitFile.DownloadFile(config.remotePathLimitFileMvvSftp, config.ownPath);

                    // If a new file exists create order data for likron and transfer file to likron.
                    // Just do it if a new load file exists on sFTP and limit file was read.
                    if (retValue.Equals("OK"))
                    {
                        likronData = new OrderDataGenerationData()
                        {
                            pathLoadCSV = config.ownPath,
                            pathOrderLikron = config.ownPath,
                            pathLimitExcel = config.ownPath + "Limit_VSK_15Min.xlsx",
                            tableName = "Limit_VSK_15Min",
                            sumQuantityIsZero = false,
                            limitType = lType,
                            loadLimit = loadLimit,
                            sellLimit = sellLimit,
                            buyLimit = buyLimit,
                            validityEnd = ValidityEnd.empty
                        };

                        // Load files are upload to sFTP each 15min (02,17,32,47).
                        // If file was already read file is ignored.
                        likronData.pathLoadCSV += results.nameOfLoadedFile;

                        // Generate Order Data for Likron TuD                   
                        likron.GenerateCsvForLikronTuD(ref likronData);
                        results.nameOfGeneratedFile = likronData.nameOfGeneratedFile;
                        results.likronDataTable = likronData.likronDataTable;
                        results.limitDataTable = likronData.limitDataTable;
                        results.loadDataTable = likronData.loadDataTable;

                        //upload order data to likron
                        retValue = sFTP_Likron.UploadFile(likronData.pathOrderLikron, config.remotePathLikronSftp);  // TODO 2 times      

                        if (retValue.Equals("OK"))
                        {
                            // Likron SFTP Upload did work.

                            // Set time of last execution with new data.
                            results.lastExecutionTimeWithNewData = DateTime.Now.ToLongTimeString();                            
                        }

                        // Set time of last execution in general.
                        results.lastExecutionTime = DateTime.Now.ToLongTimeString();                       
                    }                   
                }
                else
                {
                    // Set time of last execution in general.
                    results.lastExecutionTime = DateTime.Now.ToLongTimeString();
                }
            }
            catch (Exception exception)
            {
                // Exception set info.
                retValue = "NOK - " + exception.Message.ToString();
                // Do the logging.
                logger.WriteError("Exception in : " + (MethodBase.GetCurrentMethod().Name) + "(): " + exception.Message.ToString() + ".");
            }
            return retValue;
        }
        #endregion

        #region Get range of given contract.
        /// <summary>
        /// Get range of given contract.
        /// </summary>
        /// <param name="contract"></param>
        /// <param name="contractRange"></param>
        /// <param name="additionalRangeInfo"></param>
        /// <returns>Ok if successful.</returns>
        public string GetContractRange(string contract, ref Range contractRange, ref string additionalRangeInfo)
        {
            string retValue = "NOK";
            int hour = 0;
            string hourString = string.Empty;
            int quarter = 0;
            int minuteNow = DateTime.Now.Minute;
            int hourNow = DateTime.Now.Hour;
            DateTime timeNow = DateTime.Now;
            // Just for init, timeNow never needed.
            DateTime contractDeliveryStartTime = timeNow; // tuDStopTime
            DateTime tuDStartTime = timeNow;           

            try
            {
                string[] split = contract.Split('Q', 'q', '_');
                if ((split.Length >= 2) && (split.Length <= 3))
                {
                    // Hold in temp. string variable.
                    hourString = split[0];
                    
                    if (hourString.StartsWith("T"))
                    {
                        // If string starts with T (means Contract will start next day) - char 'T' will be removed. 
                        hourString = hourString.Remove(0, 1);
                    }

                    hour = Convert.ToInt16(hourString);
                    quarter = Convert.ToInt16(split[1]);
                    if ((quarter >=1) & (quarter <=4))
                    {
                        if ((hour >= 0) & (hour <= 23))
                        {
                            // Determine the Tud start time.  
                            switch (quarter)
                            {
                                case 1:
                                    contractDeliveryStartTime = Convert.ToDateTime(hour + ":00");
                                    break;
                                case 2:
                                    contractDeliveryStartTime = Convert.ToDateTime(hour + ":15");
                                    break;
                                case 3:
                                    contractDeliveryStartTime = Convert.ToDateTime(hour + ":30");
                                    break;
                                case 4:
                                    contractDeliveryStartTime = Convert.ToDateTime(hour + ":45"); 
                                    break;
                            }
                            tuDStartTime = contractDeliveryStartTime.AddMinutes(-30);                            

                            // Determine if we are in normal range.  
                            // >0 means tuDStartTime is after DateTimeNow
                            if (DateTime.Compare(tuDStartTime, timeNow) > 0)
                            {
                                // normal range
                                contractRange = Range.normal;
                                additionalRangeInfo = "Time left in normal range: " + (tuDStartTime - timeNow).ToString();
                            }
                            else
                            {
                                // >= TuD Start Time
                                // If not in normal range check if TuD range or delivery range.

                                // >=0 means (contractDeliveryStartTime - 5min) is equal/after DateTimeNow
                                if (DateTime.Compare(contractDeliveryStartTime.AddMinutes(-5), timeNow) >= 0)
                                {
                                    // delivery range
                                    contractRange = Range.delivery;
                                    additionalRangeInfo = "Delivery already started.";
                                }
                                else
                                {
                                    // TuD range
                                    contractRange = Range.TuD;
                                    additionalRangeInfo = "Time left in TuD range: " + (contractDeliveryStartTime.AddMinutes(-5) - timeNow).ToString();
                                }
                            }

                            retValue = "OK";
                        }
                        else
                        {
                            retValue = "NOK - Wrong product. Wrong hour.";
                        }
                    }
                    else
                    {
                        retValue = "NOK - Wrong product. Wrong quarter.(Hour not checked.)";
                    }
                }
                else
                {
                    retValue = "NOK - Wrong product. Could not be separated in hour and quarter.";
                }                              
            }
            catch (Exception exception)
            {
                // Exception set info.
                retValue = "NOK - " + exception.Message.ToString();
                // Do the logging.
                logger.WriteError("Exception in : " + (MethodBase.GetCurrentMethod().Name) + "(): " + exception.Message.ToString() + ".");
            }

            return retValue;            
        }
        #endregion
    }
}


