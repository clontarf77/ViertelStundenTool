namespace ViertelStdTool.ConfigurationHandler
{
    #region structures
    public class ConfigurationStructure
    {
        // Tool runs in TEST or PROD mode.
        public string mode;

        // Remote Paths for sFTPs
        public string remotePathLimitFileMvvSftp;
        public string remotePathLoadFileMvvSftp;
        public string remotePathLikronSftp;
        public string remotePathLikronSftpSmaller30Min;

        //Path for local folder.       
        public string ownPath;
       
        //Credentials sFTP limit-file
        public string hostSftpLimitFile;
        public string userSftpLimitFile;
        public string passwordSftpLimitFile;
        public int portSftpLimitFile;
        public string sshKeySftpLimitFile;

        //Credentials sFTP load-file
        public string hostSftpLoadFile;
        public string userSftpLoadFile;
        public string passwordSftpLoadFile;
        public int portSftpLoadFile;
        public string sshKeySftpLoadFile;

        //Credentials Likron sFTP
        public string hostLikronSftp;
        public string userLikronSftp;
        public string passwordLikronSftp;
        public int portLikronSftp;
        public string sshKeyLikronSftp;

        // Path to comtrader import file.
        public string pathComtraderExport;

        // Database Connection String Base
        public string connectionString;

        // Database Connection String STT
        public string connectionStringStt;

        // E-Mail addresses
        public string mailInCaseOfError;

        // Filename credentials Aligne 
        public string fileNameCredentialsAligne;

        // Import Server Aligne.
        public string importServerAligne;
    }
    #endregion
}
