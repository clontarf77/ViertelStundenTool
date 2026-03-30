using ViertelStdTool.ConfigurationHandler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestConfigurationReader
{
    [TestClass]
    public class ConfigurationReaderUnitTest
    {
        #region Read configuration in TEST mode
        /// <summary>
        /// Read configuration in TEST mode
        /// </summary>
        [Ignore] // - set to Ignore if mode is PROD in App.config
        [TestMethod]
        public void ReadConfigInTestMode()
        {
            IConfigurationReader configReader;
            ConfigurationStructure readConfig = new ConfigurationStructure();
            ConfigurationStructure templateConfig = new ConfigurationStructure
            {

                // Tool runs in TEST mode.
                mode = "TEST",

                // Remote Paths for sFTPs
                remotePathLimitFileMvvSftp = "/Handel/Trade_Konfig/Limit_VSK_15Min.xlsx",
                remotePathLoadFileMvvSftp = "/Handel/Intraday_qh/",
                remotePathLikronSftp = "/Mvv/in/Orderimport/VPP_QH_TEST/",
                remotePathLikronSftpSmaller30Min = "/Mvv/in/Orderimport/VPP_QH_TUD_TEST/", 

                //Credentials sFTP limit-file
                hostSftpLimitFile = "ma-sftp",
                userSftpLimitFile = "VSK_Trading_VTD",
                passwordSftpLimitFile = "xk0DklN/eyOsB6MY5Oh10Q==",
                portSftpLimitFile = 22,
                sshKeySftpLimitFile = "ssh-rsa 4096 pMyo+bz46CGLp3b73t6lh+d2QJkj95DiujHLiCwQwKY=",

                //Credentials sFTP load-file
                hostSftpLoadFile = "ma-sftp",
                userSftpLoadFile = "VSK_emsys_Trading",
                passwordSftpLoadFile = "neFx8UkFWhIjSFaE9F+/1Q==",
                portSftpLoadFile = 22,
                sshKeySftpLoadFile = "ssh-rsa 4096 pMyo+bz46CGLp3b73t6lh+d2QJkj95DiujHLiCwQwKY=",

                //Credentials Likron sFTP
                hostLikronSftp = "sftp.power.trading.volue.com",
                userLikronSftp = "mvv-prod",
                passwordLikronSftp = "yeTix0HdI9vuN/wEAULwTHtvMdvHQzfy0YtSOa+DsEc=",
                portLikronSftp = 22,
                sshKeyLikronSftp = "ssh-rsa 2048 e1:b4:f6:26:c1:c3:cb:05:b2:46:b0:4d:54:10:9c:45",

                // Path to comtrader export file.
                pathComtraderExport = "T:\\24-7-Trading\\Intraday-Trading\\001_Strom_Handel_Export\\eex_deals_export.csv",

                // Database Connection String
                connectionString = "Data Source=\"(DESCRIPTION = (ADDRESS = (PROTOCOL = tcp)(HOST = VKHST2B)(PORT = 1521))(CONNECT_DATA = (SID = khst2b)))\";User ID=VTool;Password=ZCc4BG9ZcaK+ihLrHwtPiT6JFte7hOBrYbvE7tetti4=",

                // E-Mail addresses
                mailInCaseOfError = "marc.fischer@mvv.de",

                // Filename credentials Aligne 
                fileNameCredentialsAligne = "UserSettingsDEV",

                // Import Server Aligne.
                importServerAligne = "MA-TR-KHST41"
            };

            try
            {
                // Read config
                configReader = new ConfigurationReader();
                readConfig = configReader.Config;
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(CompareConfigs(readConfig, templateConfig), "Read configuration is not equal to template.");
        }
        #endregion

        #region Read configuration in PROD mode
        /// <summary>
        /// Read configuration in PROD mode
        /// </summary>
        //[Ignore] // Configuration is set to TEST Mode so this would cause an error.
        [TestMethod]
        public void ReadConfigInProdMode()
        {
            IConfigurationReader configReader;
            ConfigurationStructure readConfig = new ConfigurationStructure();
            ConfigurationStructure templateConfig = new ConfigurationStructure
            {
                // Tool runs in PROD mode.
                mode = "PROD",

                // Remote Paths for sFTPs
                remotePathLimitFileMvvSftp = "/Handel/Trade_Konfig/Limit_VSK_15Min.xlsx",
                remotePathLoadFileMvvSftp = "/Handel/Intraday_qh/",
                remotePathLikronSftp = "/Mvv/in/Orderimport/VPP_QH/",
                remotePathLikronSftpSmaller30Min = "/Mvv/in/Orderimport/VPP_QH_TUD/",

                //Credentials sFTP limit-file
                hostSftpLimitFile = "ma-sftp",
                userSftpLimitFile = "VSK_Trading_VTD",
                passwordSftpLimitFile = "xk0DklN/eyOsB6MY5Oh10Q==",
                portSftpLimitFile = 22,
                sshKeySftpLimitFile = "ssh-rsa 4096 pMyo+bz46CGLp3b73t6lh+d2QJkj95DiujHLiCwQwKY=",

                //Credentials sFTP load-file
                hostSftpLoadFile = "ma-sftp",
                userSftpLoadFile = "VSK_emsys_Trading",
                passwordSftpLoadFile = "neFx8UkFWhIjSFaE9F+/1Q==",
                portSftpLoadFile = 22,
                sshKeySftpLoadFile = "ssh-rsa 4096 pMyo+bz46CGLp3b73t6lh+d2QJkj95DiujHLiCwQwKY=",

                //Credentials Likron sFTP
                hostLikronSftp = "sftp.power.trading.volue.com",
                userLikronSftp = "mvv-prod",
                passwordLikronSftp = "yeTix0HdI9vuN/wEAULwTHtvMdvHQzfy0YtSOa+DsEc=",
                portLikronSftp = 22,
                sshKeyLikronSftp = "ssh-rsa 2048 e1:b4:f6:26:c1:c3:cb:05:b2:46:b0:4d:54:10:9c:45",

                // Path to comtrader export file.
                pathComtraderExport = "T:\\24-7-Trading\\Intraday-Trading\\001_Strom_Handel_Export\\eex_deals_export.csv",

                // Database Connection String
                connectionString = "Data Source=\"(DESCRIPTION = (ADDRESS = (PROTOCOL = tcp)(HOST = hvkhsprdb)(PORT = 1521))(CONNECT_DATA = (SID = khsprdb)))\";User ID=VTool;Password=XSjqmGd/KYvERtXDzSVO6EKuaMZUtdQt8kYAdPxnHHw=",

                // E-Mail addresses
                mailInCaseOfError = "operations@mvv.de",

                // Filename credentials Aligne 
                fileNameCredentialsAligne = "UserSettingsPROD",

                // Import Server Aligne.
                importServerAligne = "MA-TR-KHSP41"
            };

            try
            {
                // Read config
                configReader = new ConfigurationReader();
                readConfig = configReader.Config;
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(CompareConfigs(readConfig, templateConfig), "Read configuration is not equal to template.");
        }
        #endregion

        #region Compare Configs
        /// <summary>
        ///  Compare Configs
        /// </summary>
        /// <param name="config1"></param>
        /// <param name="config2"></param>
        /// <returns>bool 1 if equal</returns>
        private bool CompareConfigs(ConfigurationStructure config1, ConfigurationStructure config2)
        {
            bool result = true;

            // Tool runs in mode.
            if (!config1.mode.Equals(config2.mode))
            {
                result = false;
            }
            // Remote Paths for sFTPs
            if (!config1.remotePathLimitFileMvvSftp.Equals(config2.remotePathLimitFileMvvSftp))
            {
                result = false;
            }           
            if (!config1.remotePathLoadFileMvvSftp.Equals(config2.remotePathLoadFileMvvSftp))
            {
                result = false;
            }
            if (!config1.remotePathLikronSftp.Equals(config2.remotePathLikronSftp))
            {
                result = false;
            }
            if (!config1.remotePathLikronSftpSmaller30Min.Equals(config2.remotePathLikronSftpSmaller30Min))
            {
                result = false;
            }
            //Credentials sFTP limit-file
            if (!config1.hostSftpLimitFile.Equals(config2.hostSftpLimitFile))
            {
                result = false;
            }
            if (!config1.userSftpLimitFile.Equals(config2.userSftpLimitFile))
            {
                result = false;
            }
            if (!config1.passwordSftpLimitFile.Equals(config2.passwordSftpLimitFile))
            {
                result = false;
            }
            if (!config1.portSftpLimitFile.Equals(config2.portSftpLimitFile))
            {
                result = false;
            }
            if (!config1.sshKeySftpLimitFile.Equals(config2.sshKeySftpLimitFile))
            {
                result = false;
            }
            //Credentials sFTP load-file
            if (!config1.hostSftpLoadFile.Equals(config2.hostSftpLoadFile))
            {
                result = false;
            }
            if (!config1.userSftpLoadFile.Equals(config2.userSftpLoadFile))
            {
                result = false;
            }
            if (!config1.passwordSftpLoadFile.Equals(config2.passwordSftpLoadFile))
            {
                result = false;
            }
            if (!config1.portSftpLoadFile.Equals(config2.portSftpLoadFile))
            {
                result = false;
            }
            if (!config1.sshKeySftpLoadFile.Equals(config2.sshKeySftpLoadFile))
            {
                result = false;
            }
            //Credentials Likron sFTP
            if (!config1.hostLikronSftp.Equals(config2.hostLikronSftp))
            {
                result = false;
            }
            if (!config1.userLikronSftp.Equals(config2.userLikronSftp))
            {
                result = false;
            }
            if (!config1.passwordLikronSftp.Equals(config2.passwordLikronSftp))
            {
                result = false;
            }
            if (!config1.portLikronSftp.Equals(config2.portLikronSftp))
            {
                result = false;
            }
            if (!config1.sshKeyLikronSftp.Equals(config2.sshKeyLikronSftp))
            {
                result = false;
            }
            // Comtrader export file.
            if (!config1.pathComtraderExport.Equals(config2.pathComtraderExport))
            {
                result = false;
            }
            // Database Connection String.
            if (!config1.connectionString.Equals(config2.connectionString))
            {
                result = false;
            }
            // E-Mail addresses
            if (!config1.mailInCaseOfError.Equals(config2.mailInCaseOfError))
            {
                result = false;
            }
            // Filename credentials Aligne 
            if (!config1.fileNameCredentialsAligne.Equals(config2.fileNameCredentialsAligne))
            {
                result = false;
            }
            // Import Server Aligne.
            if (!config1.importServerAligne.Equals(config2.importServerAligne))
            {
                result = false;
            }

            return result;
        }
        #endregion
    }
}

