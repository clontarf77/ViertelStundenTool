using PasswordEnDeCrypter;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using ViertelStdTool.Log;
using WinSCP;

namespace ViertelStdTool.SFTPManager
{
    public class SFTP : ISFTP
    {
        private INLogger logger = new NLogger();

        private SessionOptions sessionOptions = new SessionOptions();
        private Session session = new Session();

        #region Constructor
        /// <summary>
        /// Constructor without private key authentication
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="sshKey"></param>
        public SFTP(string host, int port, string user, string password, string sshKey)
        {
            // Setup session options
            sessionOptions.Protocol = Protocol.Sftp;
            sessionOptions.HostName = host;
            sessionOptions.PortNumber = port;
            sessionOptions.UserName = user;
            //Use encrypted password.
            sessionOptions.Password = Encoder.DecryptString(password, "PESTRASIRUSE");
            sessionOptions.SshHostKeyFingerprint = sshKey;
            // Do the logging.
            logger.WriteInfo("Init: SFTP session initialized for host " + sessionOptions.HostName + " with user " + sessionOptions.UserName + ".");
        }
        /// <summary>
        /// Constructor with private key authentication
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="sshKey"></param>
        /// <param name="pathPrivateKey"></param>
        /// <param name="passphrasePrivateKey"></param>
        public SFTP(string host, int port, string user, string password, string sshKey, string pathPrivateKey, string passphrasePrivateKey)
        {
            // Setup session options
            sessionOptions.Protocol = Protocol.Sftp;
            sessionOptions.HostName = host;
            sessionOptions.PortNumber = port;
            sessionOptions.UserName = user;
            //Use encrypted password.
            sessionOptions.Password = Encoder.DecryptString(password, "PESTRASIRUSE");
            sessionOptions.SshHostKeyFingerprint = sshKey;
            // Use private key authentication with passphrase.

            // Decode password.
            sessionOptions.PrivateKeyPassphrase = Encoder.DecryptString(passphrasePrivateKey, "PESTRASIRUSE"); 
            sessionOptions.SshPrivateKeyPath = pathPrivateKey;
            // Do the logging.
            logger.WriteInfo("Init: SFTP session with private key auth. initialized for host " + sessionOptions.HostName + " with user " + sessionOptions.UserName + ".");
        }
        #endregion

        #region Download file from sFTP server
        /// <summary>
        /// Download file from sFTP server.
        /// </summary>
        /// <param name="remotePath"></param>
        /// <param name="localPath"></param>
        /// <returns></returns>
        public string DownloadFile(string remotePath, string localPath)
        {
            TransferOperationResult transferResult;
            string retValue = "NOK - SFTP Download from '" + remotePath + "' did NOT work.";

            try
            {
                if (!session.Opened)
                {
                    // Connect - Just reconnect if not connected.
                    session.Open(sessionOptions);
                    // Do the logging.
                    logger.WriteInfo("Order thread: Opened SFTP session host " + sessionOptions.HostName + " with user " + sessionOptions.UserName + ".");
                }

                // Upload files
                TransferOptions transferOptions = new TransferOptions
                {
                    TransferMode = TransferMode.Binary
                };

                transferResult = session.GetFiles(remotePath, localPath, false, transferOptions);

                if (transferResult.IsSuccess)
                {
                    // Transfer worked.
                    retValue = "OK";
                    // Do the logging.
                    logger.WriteInfo("Order thread: Downloaded file from remote path " + remotePath + " on host " + sessionOptions.HostName + ".");
                }

                // Disconnect. 
                session.Close();
                logger.WriteInfo("Order thread: Closed SFTP session host " + sessionOptions.HostName + " with user " + sessionOptions.UserName + ".");
            }
            catch (Exception exception)
            {
                // Exception set info.
                retValue = "NOK - " + exception.Message.ToString();
                // Do the logging.
                logger.WriteError("Order thread: Exception in : " + (MethodBase.GetCurrentMethod().Name) + "(): " + exception.Message.ToString() + ".");
                //throw exception;
            }

            return retValue;
        }
        #endregion

        #region Upload file to sFTP server
        /// <summary>
        /// Upload file to sFTP server.
        /// </summary>
        /// <param name="remotePath"></param>
        /// <param name="localPath"></param>
        /// <returns></returns>
        public string UploadFile(string localPath, string remotePath)
        {
            // For Likron SFTP an new connect and disconnect is used before after putfile.
            // If not connection is not stable.

            string retValue = "NOK";
            TransferOperationResult transferResult;

            try
            {
                // Upload files
                TransferOptions transferOptions = new TransferOptions
                {
                    TransferMode = TransferMode.Binary,
                    PreserveTimestamp = false,
                    FilePermissions = null
                };

                // Connect. 
                session.Open(sessionOptions);
                // Do the logging.
                logger.WriteInfo("Order thread: Opened SFTP session host " + sessionOptions.HostName + " with user " + sessionOptions.UserName + ".");

                transferResult = session.PutFiles(localPath, remotePath, false, transferOptions);

                if (transferResult.IsSuccess)
                {
                    // Transfer worked.
                    retValue = "OK";
                    logger.WriteInfo("Order thread: Uploaded file to remote path " + remotePath + " on host " + sessionOptions.HostName + ".");
                }

                // Disconnect. 
                session.Close();
                logger.WriteInfo("Order thread: Closed SFTP session host " + sessionOptions.HostName + " with user " + sessionOptions.UserName + ".");
            }
            catch (Exception exception)
            {
                // Exception set info.
                retValue = "NOK - " + exception.Message.ToString();
                // Do the logging.
                logger.WriteError("Order thread: Exception in : " + (MethodBase.GetCurrentMethod().Name) + "(): " + exception.Message.ToString());
                //throw exception;
            }

            return retValue;
        }
        #endregion      

        #region Download latest file (if not loaded before) from sFTP server which starts with given filter name.
        /// <summary>
        /// Download latest file (if not loaded before) from sFTP server which starts with given filter name.
        /// Check if file already exists on local destination.
        /// </summary>       
        /// <param name="remoteDirectoryPath"></param>
        /// <param name="localDestination"></param>      
        /// <param name="fileStartsWith"></param>
        /// <param name="ref nameOfDownloadedFile"></param>
        /// <returns></returns>
        public string DownloadLatesFileUseFilter(string remoteDirectoryPath, string localDestination, string fileStartsWith, ref string nameOfDownloadedFile)
        {
            string retValue = "NOK";
            RemoteDirectoryInfo directoryInfo;

            try
            {
                if (!session.Opened)
                {
                    // Connect - Just reconnect if not connected.
                    session.Open(sessionOptions);
                    // Do the logging.
                    logger.WriteInfo("Order thread: Opened SFTP session host " + sessionOptions.HostName + " with user " + sessionOptions.UserName + ".");
                }

                // Get list of files in the directory
                directoryInfo = session.ListDirectory(remoteDirectoryPath);

                // Select the most recent file
                RemoteFileInfo latestFile =
                   directoryInfo.Files
                       .Where(file => !file.IsDirectory && file.Name.StartsWith(fileStartsWith))
                       .OrderByDescending(file => file.LastWriteTime)
                       .FirstOrDefault();

                if (latestFile != null)
                {
                    //Check if file has been downloaded already
                    if (!File.Exists(localDestination + latestFile.Name))
                    {
                        // Download latest file if not loaded before.
                        // Logging is done in DownloadFile
                        retValue = DownloadFile(remoteDirectoryPath + "/" + latestFile.Name, localDestination);

                        if (retValue.Equals("OK"))
                        {
                            nameOfDownloadedFile = latestFile.Name;
                        }
                    }
                    else
                    {
                        logger.WriteInfo("Order thread: Latest file already downloaded from remote path " + remoteDirectoryPath + " on host " + sessionOptions.HostName + ".");
                        logger.WriteInfo("Order Thread: Already downloaded file to local path '" + localDestination + latestFile.Name + "'.");
                        retValue = "OK - Latest file was already downloaded. No further download was done.";
                    }
                }
                else
                {
                    logger.WriteInfo("Order thread: No latest file found on remote path " + remoteDirectoryPath + " on host " + sessionOptions.HostName + ".");
                }
            }
            catch (Exception exception)
            {
                // Exception set info.
                retValue = "NOK - " + exception.Message.ToString();
                // Do the logging.
                logger.WriteError("Order thread: Exception in : " + (MethodBase.GetCurrentMethod().Name) + "(): " + exception.Message.ToString() + ".");
                //throw exception;
            }

            return retValue;
        }
        #endregion    
    }
}
