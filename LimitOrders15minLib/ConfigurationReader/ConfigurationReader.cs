using System;
using System.Configuration;
using ViertelStdTool.Log;

namespace ViertelStdTool.ConfigurationHandler
{
    public class ConfigurationReader : IConfigurationReader
    {
        private readonly INLogger logger = new NLogger();

        #region Constructor to read in the configuration.
        /// <summary>
        /// Constructor to read in the configuration.
        /// </summary>       
        public ConfigurationReader()
        {
            // Tool runs in TEST or PROD mode.
            Config.mode = ConfigurationManager.AppSettings["Mode"];

            //Credentials sFTP limit-file
            Config.hostSftpLimitFile = ConfigurationManager.AppSettings["HostSftpLimitFile"];
            Config.userSftpLimitFile = ConfigurationManager.AppSettings["UserSftpLimitFile"];
            Config.passwordSftpLimitFile = ConfigurationManager.AppSettings["PasswordSftpLimitFile"];
            Config.portSftpLimitFile = Convert.ToInt32(ConfigurationManager.AppSettings["PortSftpLimitFile"]);
            Config.sshKeySftpLimitFile = ConfigurationManager.AppSettings["SshKeySftpLimitFile"];

            //Credentials sFTP load-file
            Config.hostSftpLoadFile = ConfigurationManager.AppSettings["HostSftpLoadFile"];
            Config.userSftpLoadFile = ConfigurationManager.AppSettings["UserSftpLoadFile"];
            Config.passwordSftpLoadFile = ConfigurationManager.AppSettings["PasswordSftpLoadFile"];
            Config.portSftpLoadFile = Convert.ToInt32(ConfigurationManager.AppSettings["PortSftpLoadFile"]);
            Config.sshKeySftpLoadFile = ConfigurationManager.AppSettings["SshKeySftpLoadFile"];

            // Remote Paths for sFTPs
            Config.remotePathLimitFileMvvSftp = ConfigurationManager.AppSettings["RemotePathLimitFileMvvSftp"];            
            
            // Mode is PROD
            if (Config.mode.Equals("PROD"))
            {
                // Remote Path for sFTP
                Config.remotePathLoadFileMvvSftp = ConfigurationManager.AppSettings["RemotePathLoadFileMvvSftpPROD"];            

                //Credentials Likron sFTP
                Config.hostLikronSftp = ConfigurationManager.AppSettings["HostLikronSftpPROD"];
                Config.userLikronSftp = ConfigurationManager.AppSettings["UserLikronSftpPROD"];
                Config.passwordLikronSftp = ConfigurationManager.AppSettings["PasswordLikronSftpPROD"];
                Config.portLikronSftp = Convert.ToInt32(ConfigurationManager.AppSettings["PortLikronSftpPROD"]);
                Config.sshKeyLikronSftp = ConfigurationManager.AppSettings["SshKeyLikronSftpPROD"];
                // Comtrader export file.
                Config.pathComtraderExport = ConfigurationManager.AppSettings["PathComtraderExportPROD"];
                // Filename credentials Aligne 
                Config.fileNameCredentialsAligne = ConfigurationManager.AppSettings["CredetialsFileNamePROD"];
                // Import Server Aligne
                Config.importServerAligne = ConfigurationManager.AppSettings["ImportServerPROD"];
                // Database Connection String Base
                Config.connectionString = ConfigurationManager.AppSettings["ConnectionStringPROD"];
                // Database Connection String STT
                Config.connectionStringStt = ConfigurationManager.AppSettings["ConnectionStringSttPROD"];
                // E-Mail addresses
                Config.mailInCaseOfError = ConfigurationManager.AppSettings["MailInCaseOfErrorPROD"];
                // Remote Paths for sFTPs                
                Config.remotePathLikronSftp = ConfigurationManager.AppSettings["RemotePathLikronSftpPROD"];
                Config.remotePathLikronSftpSmaller30Min = ConfigurationManager.AppSettings["RemotePathLikronSftpSmaller30MinPROD"];
                // Do the logging.
                logger.WriteInfo("Init: Config read for PROD.");                
            }
            // Mode is not PROD means TEST
            else
            {
                // Remote Path for sFTP
                Config.remotePathLoadFileMvvSftp = ConfigurationManager.AppSettings["RemotePathLoadFileMvvSftpTEST"];                 
                              
                //Credentials Likron sFTP
                Config.hostLikronSftp = ConfigurationManager.AppSettings["HostLikronSftpTEST"];
                Config.userLikronSftp = ConfigurationManager.AppSettings["UserLikronSftpTEST"];
                Config.passwordLikronSftp = ConfigurationManager.AppSettings["PasswordLikronSftpTEST"];
                Config.portLikronSftp = Convert.ToInt32(ConfigurationManager.AppSettings["PortLikronSftpTEST"]);
                Config.sshKeyLikronSftp = ConfigurationManager.AppSettings["SshKeyLikronSftpTEST"];
                // Comtrader export file.
                Config.pathComtraderExport = ConfigurationManager.AppSettings["PathComtraderExportTEST"];
                // Filename credentials Aligne 
                Config.fileNameCredentialsAligne = ConfigurationManager.AppSettings["CredetialsFileNameTEST"];
                // Import Server Aligne
                Config.importServerAligne = ConfigurationManager.AppSettings["ImportServerTEST"];
                // Database Connection String
                Config.connectionString = ConfigurationManager.AppSettings["ConnectionStringTEST"];
                // Database Connection String STT
                Config.connectionStringStt = ConfigurationManager.AppSettings["ConnectionStringSttTEST"];
                // E-Mail addresses
                Config.mailInCaseOfError = ConfigurationManager.AppSettings["MailInCaseOfErrorTEST"];
                // Remote Paths for sFTPs                
                Config.remotePathLikronSftp = ConfigurationManager.AppSettings["RemotePathLikronSftpDEV"];
                Config.remotePathLikronSftpSmaller30Min = ConfigurationManager.AppSettings["RemotePathLikronSftpSmaller30MinDEV"];
                // Do the logging.
                logger.WriteInfo("Init: Config read for TEST.");
            }                 
        }
        #endregion

        #region Getter for variable config.
        /// <summary>
        /// Getter for variable config.
        /// </summary>
        public ConfigurationStructure Config { get; } = new ConfigurationStructure();
        #endregion
    }
}
