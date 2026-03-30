using ViertelStdTool.AligneImporter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Reflection;
using ViertelStdToolLib.Aligne.Importer.DealStructure;
using ViertelStdToolLib.Aligne.Importer.Generate.Xml;
using System.Collections.Generic;

namespace UnitTestAligneImporter
{
    [TestClass]
    public class AligneImporterUnitTest
    {
        #region Import New Deal to DEV3 with user RFSR use CSV as input use plain password.
        [TestMethod]
        // Set ignore deal should not be imported by mistake. Also plain password is not saved here.
        [Ignore] 
        public void ImportNewDealDev3CsvPlain()
        {
            const string server = "MA-TR-KHST41";
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(AligneImporterUnitTest)).CodeBase);
            pathExecutable = pathExecutable.Remove(0, 6) + @"\Aligne\Importer\Uploader";
            const string fileName = "TestTrade";

            string user = "RFSR";
            // Use encrypted password from PasswordManager.
            string password = "add PW to do testing";
            
            string retValue = string.Empty;
            string zKey = string.Empty;

            try
            {
                IAligneImporter aligne = new AligneImporter(server, user, password);
                retValue = aligne.ImportDeal(fileName, pathExecutable, ImportFileType.CSV, PwEnCrypton.PLAIN, ref zKey, false);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }
            Assert.IsTrue(retValue.Equals("success"), "Upload.exe was not execuded.");
            Assert.IsTrue(zKey.Length == 8, "Zkey from imported file was not read.");
        }
        #endregion

        #region Import New Deal to DEV3 with user RFSR use CSV as input use password from PasswordManager.
        [TestMethod]
        // Set ignore deal should not be imported by mistake.
        [Ignore]
        public void ImportNewDealDev3CsvPwManager()
        {
            const string server = "MA-TR-KHST41";
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(AligneImporterUnitTest)).CodeBase);
            pathExecutable = pathExecutable.Remove(0, 6) + @"\Aligne\Importer\Uploader";
            const string fileName = "TestTrade";

            string user = "RFSR";
            // Use encrypted password from PasswordManager.
            string password = "VV0QDC7cFs08XiYkMLHIEg==";

            string retValue = string.Empty;
            string zKey = string.Empty;

            try
            {
                IAligneImporter aligne = new AligneImporter(server,user, password);
                retValue = aligne.ImportDeal(fileName, pathExecutable, ImportFileType.CSV, PwEnCrypton.PW_MANAGER, ref zKey, false);               
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }          
            Assert.IsTrue(retValue.Equals("success"), "Upload.exe was not execuded.");
            Assert.IsTrue(zKey.Length == 8, "Zkey from imported file was not read.");
        }
        #endregion

        #region Import New Deal to DEV3 with user RFSR use Xml as input use password from PasswordManager.
        [TestMethod]
        // Set ignore deal should not be imported by mistake.
        [Ignore]
        public void ImportNewDealDev3XmlPwManager()
        {
            const string server = "MA-TR-KHST41";
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(AligneImporterUnitTest)).CodeBase);
            pathExecutable = pathExecutable.Remove(0, 6) + @"\Aligne\Importer\Uploader";
            const string fileName = "TestTrade";

            string user = "RFSR";
            // Use encrypted password from PasswordManager.
            string password = "VV0QDC7cFs08XiYkMLHIEg==";

            string retValue = string.Empty;
            string zKey = string.Empty;

            try
            {
                IAligneImporter aligne = new AligneImporter(server, user, password);
                retValue = aligne.ImportDeal(fileName, pathExecutable, ImportFileType.XML, PwEnCrypton.PW_MANAGER, ref zKey, false);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }
            Assert.IsTrue(retValue.Equals("success"), "Upload.exe was not execuded.");
            Assert.IsTrue(zKey.Length == 8, "Zkey from imported file was not read.");
        }
        #endregion

        #region Import New Deal to DEV3 with user RFSR use XML (half hour shape) as input use password from PasswordManager.
        [TestMethod]
        // Set ignore deal should not be imported by mistake.
        [Ignore]
        public void ImportNewDealDev3XmlPwManagerHalfHourShape()
        {
            const string server = "MA-TR-KHST41";
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(AligneImporterUnitTest)).CodeBase);
            pathExecutable = pathExecutable.Remove(0, 6) + @"\Aligne\Importer\Uploader";
            const string fileName = "TestTradeBuyXmlHalfHour";

            string user = "RFSR";
            // Use encrypted password from PasswordManager.
            string password = "h96GdYuq9Ci0LXMQnoQqzg==";

            string retValue = string.Empty;
            string zKey = string.Empty;

            try
            {
                IAligneImporter aligne = new AligneImporter(server, user, password);
                retValue = aligne.ImportDeal(fileName, pathExecutable, ImportFileType.XML, PwEnCrypton.PW_MANAGER, ref zKey, false);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }
            Assert.IsTrue(retValue.Equals("success"), "Upload.exe was not execuded.");
            Assert.IsTrue(zKey.Length == 8, "Zkey from imported file was not read.");
        }
        #endregion

        #region Import New Deal to DEV3 with user RFSR use XML (hour shape) as input use password from PasswordManager.
        [TestMethod]
        // Set ignore deal should not be imported by mistake.
        [Ignore]
        public void ImportNewDealDev3XmlPwManagerHourShape()
        {
            const string server = "MA-TR-KHST41";
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(AligneImporterUnitTest)).CodeBase);
            pathExecutable = pathExecutable.Remove(0, 6) + @"\Aligne\Importer\Uploader";
            const string fileName = "TestTradeBuyXmlHour";

            string user = "RFSR";
            // Use encrypted password from PasswordManager.
            string password = "h96GdYuq9Ci0LXMQnoQqzg==";

            string retValue = string.Empty;
            string zKey = string.Empty;

            try
            {
                IAligneImporter aligne = new AligneImporter(server, user, password);
                retValue = aligne.ImportDeal(fileName, pathExecutable, ImportFileType.XML, PwEnCrypton.PW_MANAGER, ref zKey, false);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }
            Assert.IsTrue(retValue.Equals("success"), "Upload.exe was not execuded.");
            Assert.IsTrue(zKey.Length == 8, "Zkey from imported file was not read.");
        }
        #endregion

        #region Import New Deal to DEV3 with user RFSR use CSV as input use password from Simple Pass.
        [TestMethod]
        // Set ignore deal should not be imported by mistake.
        [Ignore]
        public void ImportNewDealDev3Csv()
        {
            const string server = "MA-TR-KHST41";
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(AligneImporterUnitTest)).CodeBase);
            pathExecutable = pathExecutable.Remove(0, 6) + @"\Aligne\Importer\Uploader";
            const string fileName = "TestTrade";

            string user = "RFSR";
            // Use encrypted password from SimplePass.
            string password = "duKpoObbV3YCaiWfxRdBcjYe";

            string retValue = string.Empty;
            string zKey = string.Empty;

            try
            {
                IAligneImporter aligne = new AligneImporter(server, user, password);
                retValue = aligne.ImportDeal(fileName, pathExecutable, ImportFileType.CSV, PwEnCrypton.SIMPLE_PASS, ref zKey, false);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }
            Assert.IsTrue(retValue.Equals("success"), "Upload.exe was not execuded.");
            Assert.IsTrue(zKey.Length == 8, "Zkey from imported file was not read.");
        }
        #endregion

        #region Import New Deal to DEV3 with user RFSR use CSV as input use password from SimplePass.
        [TestMethod]
        // Set ignore deal should not be imported by mistake.
        [Ignore]
        public void ImportNewDealDev3Xml()
        {
            const string server = "MA-TR-KHST41";
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(AligneImporterUnitTest)).CodeBase);
            pathExecutable = pathExecutable.Remove(0, 6) + @"\Aligne\Importer\Uploader";
            const string fileName = "TestTrade";

            string user = "RFSR";
            // Use encrypted password from SimplePass.
            string password = "duKpoObbV3YCaiWfxRdBcjYe";

            string retValue = string.Empty;
            string zKey = string.Empty;

            try
            {
                IAligneImporter aligne = new AligneImporter(server, user, password);
                retValue = aligne.ImportDeal(fileName, pathExecutable, ImportFileType.XML, PwEnCrypton.SIMPLE_PASS, ref zKey, false);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }
            Assert.IsTrue(retValue.Equals("success"), "Upload.exe was not execuded.");
            Assert.IsTrue(zKey.Length == 8, "Zkey from imported file was not read.");
        }
        #endregion

        #region Import New Deal (Buy) to DEV3 with user RFSR use CSV as input use password from SimplePass.
        [TestMethod]
        // Set ignore deal should not be imported by mistake.
        [Ignore]
        public void ImportNewDealBuyDev3Csv()
        {
            const string server = "MA-TR-KHST41";
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(AligneImporterUnitTest)).CodeBase);
            pathExecutable = pathExecutable.Remove(0, 6) + @"\Aligne\Importer\Uploader";
            const string fileName = "TestTradeBuyCsv";
           
            string user = "RFSR";
            // Use encrypted password
            string password = "duKpoObbV3YCaiWfxRdBcjYe";

            string retValue = string.Empty;
            string zKey = string.Empty;

            try
            {
                IAligneImporter aligne = new AligneImporter(server, user, password);
                retValue = aligne.ImportDeal(fileName, pathExecutable, ImportFileType.CSV, PwEnCrypton.SIMPLE_PASS, ref zKey, false);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }
            Assert.IsTrue(retValue.Equals("success"), "Upload.exe was not execuded.");
            Assert.IsTrue(zKey.Length == 8, "Zkey from imported file was not read.");
        }
        #endregion

        #region Import New Deal (Buy) to DEV3 with user RFSR use XML as input use password from SimplePass.
        [TestMethod]
        // Set ignore deal should not be imported by mistake.
        [Ignore]
        public void ImportNewDealBuyDev3Xml()
        {
            const string server = "MA-TR-KHST41";
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(AligneImporterUnitTest)).CodeBase);
            pathExecutable = pathExecutable.Remove(0, 6) + @"\Aligne\Importer\Uploader";
            const string fileName = "TestTradeBuyXml";

            string user = "RFSR";
            // Use encrypted password
            string password = "duKpoObbV3YCaiWfxRdBcjYe";

            string retValue = string.Empty;
            string zKey = string.Empty;

            try
            {
                IAligneImporter aligne = new AligneImporter(server, user, password);
                retValue = aligne.ImportDeal(fileName, pathExecutable, ImportFileType.XML, PwEnCrypton.SIMPLE_PASS, ref zKey, false);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }
            Assert.IsTrue(retValue.Equals("success"), "Upload.exe was not execuded.");
            Assert.IsTrue(zKey.Length == 8, "Zkey from imported file was not read.");
        }
        #endregion

        #region Import New Deal (Sell) to DEV3 with user RFSR use CSV as input use password from SimplePass.
        [TestMethod]
        // Set ignore deal should not be imported by mistake.
        [Ignore]
        public void ImportNewDealSellTest1Csv()
        {
            const string server = "MA-TR-KHST41";
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(AligneImporterUnitTest)).CodeBase);
            pathExecutable = pathExecutable.Remove(0, 6) + @"\Aligne\Importer\Uploader";
            const string fileName = "TestTradeSellCsv";

            string user = "RFSR";
            // Use encrypted password
            string password = "duKpoObbV3YCaiWfxRdBcjYe";

            string retValue = string.Empty;
            string zKey = string.Empty;

            try
            {
                IAligneImporter aligne = new AligneImporter(server, user, password);
                retValue = aligne.ImportDeal(fileName, pathExecutable, ImportFileType.CSV, PwEnCrypton.SIMPLE_PASS, ref zKey, false);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }
            Assert.IsTrue(retValue.Equals("success"), "Upload.exe was not execuded.");
            Assert.IsTrue(zKey.Length == 8, "Zkey from imported file was not read.");
        }
        #endregion

        #region Import New Deal (Sell) to DEV3 with user RFSR use XML as input use password from SimplePass.
        [TestMethod]
        // Set ignore deal should not be imported by mistake.
        [Ignore]
        public void ImportNewDealSellDev3Xml()
        {
            const string server = "MA-TR-KHST41";
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(AligneImporterUnitTest)).CodeBase);
            pathExecutable = pathExecutable.Remove(0, 6) + @"\Aligne\Importer\Uploader";
            const string fileName = "TestTradeSellXml";

            string user = "RFSR";
            // Use encrypted password
            string password = "duKpoObbV3YCaiWfxRdBcjYe";

            string retValue = string.Empty;
            string zKey = string.Empty;

            try
            {
                IAligneImporter aligne = new AligneImporter(server, user, password);
                retValue = aligne.ImportDeal(fileName, pathExecutable, ImportFileType.XML, PwEnCrypton.SIMPLE_PASS, ref zKey, false);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }
            Assert.IsTrue(retValue.Equals("success"), "Upload.exe was not execuded.");
            Assert.IsTrue(zKey.Length == 8, "Zkey from imported file was not read.");
        }
        #endregion

        #region Import New Deal (Buy) to DEV3 with user RFSR use generated XML as input use password from SimplePass.
        [TestMethod]
        // Set ignore deal should not be imported by mistake.
        [Ignore]
        public void ImportNewDealBuyDev3XmlGenerated()
        {
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(AligneImporterUnitTest)).CodeBase);
            pathExecutable = pathExecutable.Remove(0, 6) + @"\Aligne\Importer\Uploader";

            const string server = "MA-TR-KHST41";
            string user = "RFSR";
            // Use encrypted password
            string password = "duKpoObbV3YCaiWfxRdBcjYe";

            const string fileName = "TestTradeBuyXmlGenerated";
            string retValue = string.Empty;
            string zKey = string.Empty;

            // Generate List of structs.
            List<ParameterXmlGeneration> parameterList = new List<ParameterXmlGeneration>();
            ParameterXmlGeneration parameter = new ParameterXmlGeneration()
            {
                path = pathExecutable,
                fileNameGeneratedXml = fileName,
                buySell = BuySell.BUY,
                controlArea = ControlArea.AMP,
                trader = user,
                memo1 = "VSK-ImporterTestGeneratedXml",
                tradeDate = DateTime.Now.ToShortDateString(),
                contract = "13Q4",
                quantity = "13.1",
                price = "50.00"
            };
            parameterList.Add(parameter);

            try
            {
                IGenerateImporterXml xmlGenerator = new GenerateImporterXml();
                IAligneImporter aligne = new AligneImporter(server, user, password);

                // TODO user error handling in generateXmlForVSK()
                xmlGenerator.GenerateXmlForVsk(parameterList);
                retValue = aligne.ImportDeal(fileName, pathExecutable, ImportFileType.XML, PwEnCrypton.SIMPLE_PASS, ref zKey);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }
            Assert.IsTrue(retValue.Equals("success"), "Upload.exe was not execuded.");
            Assert.IsTrue(zKey.Length == 8, "Zkey from imported file was not read.");
        }
        #endregion

        #region Import New Deal (Sell) to DEV3 with user RFSR use generated XML as input use password from SimplePass.
        [TestMethod]
        // Set ignore deal should not be imported by mistake.
        [Ignore]
        public void ImportNewDealSellDev3XmlGenerated()
        {
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(AligneImporterUnitTest)).CodeBase);
            pathExecutable = pathExecutable.Remove(0, 6) + @"\Aligne\Importer\Uploader";

            const string server = "MA-TR-KHST41";
            string user = "RFSR";
            // Use encrypted password
            string password = "duKpoObbV3YCaiWfxRdBcjYe";

            const string fileName = "TestTradeSellXmlGenerated";
            string retValue = string.Empty;
            string zKey = string.Empty;

            // Generate List of structs.
            List<ParameterXmlGeneration> parameterList = new List<ParameterXmlGeneration>();

            ParameterXmlGeneration parameter = new ParameterXmlGeneration()
            {
                path = pathExecutable,
                fileNameGeneratedXml = fileName,
                buySell = BuySell.SELL,
                controlArea = ControlArea.AMP,
                trader = user,
                memo1 = "VSK-ImporterTestGeneratedXml",
                tradeDate = DateTime.Now.ToShortDateString(),
                contract = "13Q4",
                quantity = "13.1",
                price = "50.00"
            };

            parameterList.Add(parameter);

            try
            {
                IGenerateImporterXml xmlGenerator = new GenerateImporterXml();
                IAligneImporter aligne = new AligneImporter(server, user, password);

                // TODO user error handling in generateXmlForVSK()
                xmlGenerator.GenerateXmlForVsk(parameterList);
                retValue = aligne.ImportDeal(fileName, pathExecutable, ImportFileType.XML, PwEnCrypton.SIMPLE_PASS, ref zKey);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }
            Assert.IsTrue(retValue.Equals("success"), "Upload.exe was not execuded.");
            Assert.IsTrue(zKey.Length == 8, "Zkey from imported file was not read.");
        }
        #endregion

        #region Import New Deal to DEV3 with wrong server
        [TestMethod]
        // Set ignore deal should not be imported by mistake.
        [Ignore]
        public void ImportNewDealDev3WrongServer()
        {
            const string server = "MA-TR-KHST44";
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(AligneImporterUnitTest)).CodeBase);
            pathExecutable = pathExecutable.Remove(0, 6) + @"\Aligne\Importer\Uploader";
            const string fileName = "TestTrade";

            string user = "RFSR";
            // Use encrypted password
            string password = "VV0QDC7cFs08XiYkMLHIEg==";

            string retValue = string.Empty;
            string zKey = string.Empty;

            try
            {
                IAligneImporter aligne = new AligneImporter(server, user, password);
                retValue = aligne.ImportDeal(fileName, pathExecutable, ImportFileType.CSV, PwEnCrypton.PW_MANAGER, ref zKey, false);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }
            Assert.IsTrue(retValue.Equals(" No import server is running."), "Upload.exe does not return correct error message");
            Assert.IsTrue(zKey.Length == 0, "Zkey from imported file was not empty.");        
        }
        #endregion

        #region Import New Deal to DEV3 with wrong user
        [TestMethod]
        // Set ignore deal should not be imported by mistake.
        [Ignore]
        public void ImportNewDealDev3WrongUser()
        {
            const string server = "MA-TR-KHST41";
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(AligneImporterUnitTest)).CodeBase);
            pathExecutable = pathExecutable.Remove(0, 6) + @"\Aligne\Importer\Uploader";
            const string fileName = "TestTrade";

            string user = "RRRR";
            // Use encrypted password
            string password = "VV0QDC7cFs08XiYkMLHIEg==";

            string retValue = string.Empty;
            string zKey = string.Empty;

            try
            {
                IAligneImporter aligne = new AligneImporter(server, user, password);
                retValue = aligne.ImportDeal(fileName, pathExecutable, ImportFileType.CSV, PwEnCrypton.PW_MANAGER, ref zKey, false);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }           
            Assert.IsTrue(retValue.Equals("Invalid User Login or Password"), "Upload.exe does not return correct error message.");
            Assert.IsTrue(zKey.Length == 0, "Zkey from imported file was not empty.");
        }
        #endregion

        #region Import New Deal to DEV3 with wrong password
        [TestMethod]
        // Set ignore deal should not be imported by mistake.
        [Ignore]
        public void ImportNewDealDev3WrongPassword()
        {
            const string server = "MA-TR-KHST41";
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(AligneImporterUnitTest)).CodeBase);
            pathExecutable = pathExecutable.Remove(0, 6) + @"\Aligne\Importer\Uploader";
            const string fileName = "TestTrade";

            string user = "RFSR";
            // Use encrypted password - wrongPassword
            string password = "GbHRG9Wf/VwcIbWXkTV5aQ==";

            string retValue = string.Empty;
            string zKey = string.Empty;

            try
            {
                IAligneImporter aligne = new AligneImporter(server, user, password);
                retValue = aligne.ImportDeal(fileName, pathExecutable, ImportFileType.CSV, PwEnCrypton.PW_MANAGER, ref zKey, false);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }            
            Assert.IsTrue(retValue.Equals("Invalid User Id or Password"), "Upload.exe does not return correct error message.");
            Assert.IsTrue(zKey.Length == 0, "Zkey from imported file was not empty.");
        }
        #endregion

        #region Import New Deal to DEV3 with empty password
        [TestMethod]
        // Set ignore deal should not be imported by mistake.
        [Ignore]
        public void ImportNewDealDev3EmptyPassword()
        {
            const string server = "MA-TR-KHST41";
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(AligneImporterUnitTest)).CodeBase);
            pathExecutable = pathExecutable.Remove(0, 6) + @"\Aligne\Importer\Uploader";
            const string fileName = "TestTrade";

            string user = "RFSR";
            // Use empty password
            string password = "";

            string retValue = string.Empty;
            string zKey = string.Empty;

            try
            {
                IAligneImporter aligne = new AligneImporter(server, user, password);
                retValue = aligne.ImportDeal(fileName, pathExecutable, ImportFileType.CSV, PwEnCrypton.PW_MANAGER, ref zKey, false);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }
            Assert.IsTrue(retValue.Equals("Password could not be decoded"), "Upload.exe does not return correct error message.");
            Assert.IsTrue(zKey.Length == 0, "Zkey from imported file was not empty.");
        }
        #endregion

        #region Import New Deal to DEV3 with user RFSR give path of non existing CSV file.
        [TestMethod]
        [ExpectedException(typeof(System.Exception), AllowDerivedTypes = true)]
        // Set ignore deal should not be imported by mistake.
        [Ignore]
        public void ImportNewDealDev3NoCsvFile()
        {
            const string server = "MA-TR-KHST41";
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(AligneImporterUnitTest)).CodeBase);
            pathExecutable = pathExecutable.Remove(0, 6) + @"\Aligne\Importer\Uploader";
            const string fileName = "ThisFileDoesNotExist";

            string user = "RFSR";
            // Use encrypted password
            string password = "iXuhbxJIMaXJ+DUB97ESDg==";

            string retValue = string.Empty;
            string zKey = string.Empty;
                      
            IAligneImporter aligne = new AligneImporter(server, user, password);
            retValue = aligne.ImportDeal(fileName, pathExecutable, ImportFileType.CSV, PwEnCrypton.PW_MANAGER, ref zKey, false);
         
        }
        #endregion

        #region Import New Deal to DEV3 with user RFSR use CSV with wrong data.
        [TestMethod]
        [ExpectedException(typeof(System.Exception), AllowDerivedTypes = true)]
        // Set ignore deal should not be imported by mistake.
        [Ignore]
        public void ImportNewDealDev3WrongCsv()
        {
            const string server = "MA-TR-KHST01";
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(AligneImporterUnitTest)).CodeBase);
            pathExecutable = pathExecutable.Remove(0, 6) + @"\Aligne\Importer\Uploader";
            const string fileName = "TestTradeWrong";

            string user = "RFSR";
            // Use encrypted password
            string password = "iXuhbxJIMaXJ+DUB97ESDg==";

            string retValue = string.Empty;
            string zKey = string.Empty;
                      
            IAligneImporter aligne = new AligneImporter(server, user, password);
            retValue = aligne.ImportDeal(fileName, pathExecutable, ImportFileType.CSV, PwEnCrypton.PW_MANAGER, ref zKey);           
        }
        #endregion

        #region Generate XML template for importer. Use template for that.
        [TestMethod]      
        public void GenerateImporterXml()
        {
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(AligneImporterUnitTest)).CodeBase);
            pathExecutable = pathExecutable.Remove(0, 6) + @"\Aligne\Importer\Uploader";

            // Generate List of structs.
            List<ParameterXmlGeneration> parameterList = new List<ParameterXmlGeneration>();

            ParameterXmlGeneration parameter = new ParameterXmlGeneration()
            {
                path = pathExecutable,
                fileNameGeneratedXml = "GeneratedXml",
                buySell = BuySell.SELL,
                controlArea = ControlArea.AMP,
                trader = "RFSR",
                memo1 = "VSK-ImporterTestGeneratedXml",
                tradeDate = DateTime.Now.ToShortDateString(),
                contract = "13Q4",
                quantity ="13.1",
                price = "55.20"
            };

            parameterList.Add(parameter);

            try
            {
                IGenerateImporterXml xmlGenerator = new GenerateImporterXml();
                xmlGenerator.GenerateXmlForVsk(parameterList);
                
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }
            Assert.IsTrue(File.Exists(parameter.path + @"\" + parameter.fileNameGeneratedXml + ".xml"), "File does not exist.");                  
        }
        #endregion

    }
}


