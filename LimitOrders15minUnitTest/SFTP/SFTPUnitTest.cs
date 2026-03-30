using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Reflection;
using ViertelStdTool.SFTPManager;

namespace UnitTestSFTP
{
    [TestClass]
    public class SFTPUnitTest
    {
        private readonly string pathExecutable = (Path.GetDirectoryName(Assembly.GetAssembly(typeof(SFTPUnitTest)).CodeBase)).Remove(0, 6);

        #region Download File (Limit_VSK_15Min) from SFTP (VSK_Trading_VTD).
        /// <summary>
        /// Download File (Limit_VSK_15Min) from SFTP (VSK_Trading_VTD).
        /// </summary>
        [TestMethod]
        public void DownloadLimit15MinFileFromSFTP()
        {
            string result = string.Empty;
            string remotePath = "/Handel/Trade_Konfig/Limit_VSK_15Min.xlsx";
            string localPath = pathExecutable + "\\SFTP\\";

            string host = "ma-sftp";
            int port = 22; 
            string user = "VSK_Trading_VTD"; 
            string password = "xk0DklN/eyOsB6MY5Oh10Q=="; 
            string sshKey = "ssh-rsa 4096 pMyo+bz46CGLp3b73t6lh+d2QJkj95DiujHLiCwQwKY="; 
            ISFTP sFTP_VSK_Trading_VTD = new SFTP(host, port , user, password, sshKey);

            try
            {
                result = sFTP_VSK_Trading_VTD.DownloadFile(remotePath, localPath);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(result.Equals("OK"), "Download was not successful");
        }
        #endregion

        #region Download latest load file (if not loaded before) starting with FromVPPToComTrader from SFTP (VSK_emsys_Trading).
        /// <summary>
        /// Download latest load (if not loaded before) file starting with FromVPPToComTrader from SFTP (VSK_emsys_Trading).
        /// </summary>
        [TestMethod]
        public void DownloadLatesLoadFileFromProdSFTP()
        {
            string result = string.Empty;
            string remoteDirectoryPath = "/Handel/Intraday_qh";
            string localPath = pathExecutable + "\\SFTP\\";
            string fileStartsWith = "FromVPPToComTrader";
            string nameOfLoadedFile = string.Empty;

            string host = "ma-sftp";
            int port = 22;
            string user = "VSK_emsys_Trading";
            string password = "neFx8UkFWhIjSFaE9F+/1Q=="; 
            string sshKey = "ssh-rsa 4096 pMyo+bz46CGLp3b73t6lh+d2QJkj95DiujHLiCwQwKY="; 
            ISFTP sFTP_VSK_emsys_Trading = new SFTP(host, port, user, password, sshKey);

            try
            {
                result = sFTP_VSK_emsys_Trading.DownloadLatesFileUseFilter(remoteDirectoryPath, localPath, fileStartsWith, ref nameOfLoadedFile);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(result.StartsWith("OK"), "Download was not successful");
            if (result.Equals("OK"))
            {
                Assert.IsTrue(nameOfLoadedFile.StartsWith(fileStartsWith), "Download was not successful");
            }
            if (result.Equals("OK - Latest file was already downloaded. No further download was done."))
            {
                Assert.IsTrue(nameOfLoadedFile.Length == 0, "Download was not successful");
            }
        }
        #endregion

        #region Upload File to SFTP Likron Test - Obsolte old Likron SFTP
        /// <summary>
        /// Upload File to Test SFTP Likron - Obsolte old Likron SFTP
        /// </summary>
        [Ignore] // Set to ignore because file should not be uploaded every time.
        [TestMethod]
        public void UploadFiletoSFTPLikronTest_Obsolete()
        {
            string result = string.Empty;
            string remotePath = "/Mvv/in/Orderimport/VPP_QH/Deal_Import_Active_Timestamp_Test.csv";            

            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(SFTPUnitTest)).CodeBase);
            string localPath = pathExecutable.Remove(0, 6) + @"\SFTP\TestData\Deal_Import_Active_Timestamp_Test.csv";


            string host = "sftp.likron.com";
            int port = 2222;
            string user = "mvv-test";                               
            string password = "+q2L+xxT8uE8TxuaQcTVOybliav2biAlkcnFa+JbqEM=";                              
            string sshKey = "ecdsa-sha2-nistp256 256 3a:6b:5e:99:13:fc:5c:8c:01:b8:eb:99:ff:27:06:38"; 
            ISFTP sFTP_Likron = new SFTP(host, port, user, password, sshKey);

            try
            {
                result = sFTP_Likron.UploadFile(localPath, remotePath); 
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(result.Equals("OK"), "Upload was not successful");
        }
        #endregion

        #region Upload File to SFTP Likron PROD - Obsolte old Likron SFTP
        /// <summary>
        /// Upload File to Test SFTP Likron - Obsolte old Likron SFTP
        /// </summary>
        [Ignore] // This is PROD !!!
        [TestMethod]
        public void UploadFiletoSFTPLikronProd_Obsolete()
        {
            string result = string.Empty;
            string remotePath = "/Mvv/in/Orderimport/VPP_QH/Deal_Import_Active_Timestamp_Test.csv";            

            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(SFTPUnitTest)).CodeBase);
            string localPath = pathExecutable.Remove(0, 6) + @"\SFTP\TestData\Deal_Import_Active_Timestamp_Test.csv";

            string host = "sftp.likron.com";
            int port = 2222;
            string user = "mvv-prod";
            string password = "yeTix0HdI9vuN/wEAULwTHtvMdvHQzfy0YtSOa+DsEc=";
            string sshKey = "ecdsa-sha2-nistp256 256 3a:6b:5e:99:13:fc:5c:8c:01:b8:eb:99:ff:27:06:38";
            ISFTP sFTP_Likron = new SFTP(host, port, user, password, sshKey);

            try
            {
                result = sFTP_Likron.UploadFile(localPath, remotePath);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(result.Equals("OK"), "Download was not successful");
        }
        #endregion

        #region Upload File to SFTP Likron PROD
        /// <summary>
        /// Upload File to Test SFTP Likron
        /// </summary>
        [Ignore] // This is PROD !!!
        [TestMethod]
        public void UploadFiletoSFTPLikronProd()
        {
            string result = string.Empty;
            string remotePath = "/Mvv/in/Orderimport/VPP_QH/Deal_Import_Active_Timestamp_Test.csv";

            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(SFTPUnitTest)).CodeBase);
            string localPath = pathExecutable.Remove(0, 6) + @"\SFTP\TestData\Deal_Import_Active_Timestamp_Test.csv";

            string host = "sftp-prod.ffm1.likron.com";
            int port = 22;
            string user = "mvv-prod";
            string password = "yeTix0HdI9vuN/wEAULwTHtvMdvHQzfy0YtSOa+DsEc=";
            string sshKey = "ssh-rsa 2048 e1:b4:f6:26:c1:c3:cb:05:b2:46:b0:4d:54:10:9c:45";

            string passphrasePrivateKey = "Ze7lX9XbBmzG9yvpcm/irb3hOLREd6Kgs07yi6ZkWFE=";
            string pathPrivateKey = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\LimitOrders15minLib\\SFTP\\PrivateKey\\privateKeyLikron.ppk"));

            ISFTP sFTP_Likron = new SFTP(host, port, user, password, sshKey, pathPrivateKey, passphrasePrivateKey);

            try
            {
                result = sFTP_Likron.UploadFile(localPath, remotePath);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(result.Equals("OK"), "Download was not successful");
        }
        #endregion

        #region Upload File to SFTP Likron Test - Physically this is the Prod Server but uploaded to an unsed folder
        /// <summary>
        /// Upload File to Test SFTP Likron - Physically this is the Prod Server but uploaded to an unsed folder
        /// </summary>
        [Ignore] // Set to ignore because file should not be uploaded every time.
        [TestMethod]
        public void UploadFiletoSFTPLikronTest()
        {
            string result = string.Empty;
            string remotePath = "/Mvv/in/Orderimport/VPP_QH_TEST/Deal_Import_Active_Timestamp_Test.csv";

            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(SFTPUnitTest)).CodeBase);
            string localPath = pathExecutable.Remove(0, 6) + @"\SFTP\TestData\Deal_Import_Active_Timestamp_Test.csv";
            
            string host = "sftp-prod.ffm1.likron.com";
            int port = 22;
            string user = "mvv-prod";
            string password = "yeTix0HdI9vuN/wEAULwTHtvMdvHQzfy0YtSOa+DsEc=";
            string sshKey = "ssh-rsa 2048 e1:b4:f6:26:c1:c3:cb:05:b2:46:b0:4d:54:10:9c:45";

            string passphrasePrivateKey = "Ze7lX9XbBmzG9yvpcm/irb3hOLREd6Kgs07yi6ZkWFE=";
            string pathPrivateKey = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\LimitOrders15minLib\\SFTP\\PrivateKey\\privateKeyLikron.ppk"));
                    
            ISFTP sFTP_Likron = new SFTP(host, port, user, password, sshKey, pathPrivateKey, passphrasePrivateKey);

            try
            {
                result = sFTP_Likron.UploadFile(localPath, remotePath);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(result.Equals("OK"), "Upload was not successful");
        }
        #endregion
    }
}


