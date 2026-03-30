using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using ViertelStdToolLib.Aligne.DataBaseRequests;
using ViertelStdToolLib.Aligne.Importer.DealStructure;
using ViertelStdToolLib.BookTradesAligne;


namespace UnitTestBookTradesAligneRestSttAif
{
    [TestClass]
    public class BookTradesAligneRestSttAifUnitTest
    {
        #region Get XML Deal Date from AIF STT, generate XML and import to Aligne. -- (Example DEV3) -- Use example XML 
        /// <summary>
        /// Get XML Deal Date from AIF STT, generate XML and import to Aligne. -- (Example DEV3)  -- Use example XML 
        /// </summary>
        [TestMethod]
        // Set ignore deal should not be imported by mistake.
        [Ignore]
        public void ExportTrade1AifSttGenerateXmlImportAligne()
        {
            // Connection string for Database DEV3 Base
            const string connectionString = "Data Source=\"(DESCRIPTION = (ADDRESS = (PROTOCOL = tcp)(HOST = VKHST2B)(PORT = 1521))(CONNECT_DATA = (SID = khst2b)))\";User ID=VTool;Password=ZCc4BG9ZcaK+ihLrHwtPiT6JFte7hOBrYbvE7tetti4=";
            IDatabaseAccess dbAccess = new DatabaseAccess();            
            int numberOfRows = 0;
            string date = DateTime.Now.ToShortDateString();
            
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(BookTradesAligneRestSttAifUnitTest)).CodeBase);
            string pathTestData = pathExecutable.Remove(0, 6) + @"\BookTradesInAligneRest\TestData\TradeXmlFromSttAif.xml";
            string errorInformation = string.Empty;
            bool importDone = false;

            List<OutputData> outputDataList = new List<OutputData>();

            InputDataXmlAifStt input = new InputDataXmlAifStt()
            {
                date = date,
                trader = "RFSR",
                password = "h96GdYuq9Ci0LXMQnoQqzg==",
                pwEnCrypton = PwEnCrypton.PW_MANAGER,
                server = "MA-TR-KHST41",
                connectionStringStt = "Data Source=\"(DESCRIPTION = (ADDRESS = (PROTOCOL = tcp)(HOST = VKHST2NB)(PORT = 1521))(CONNECT_DATA = (SID = khst2nb)))\";User ID=VTool;Password=qVjm5ItwzbkJ4pSnut4yu9eBWHubmeNuE7W00yrNDTM=",
                vskMissingCustomerTrades = new DataTable()
            };            

            try
            {
                IBookTradesAligne bookTrades = new BookTradesAligne();
                // Read not booked trades as table from base system.            
                dbAccess.ReadNotBookedVskTrades(ref input.vskMissingCustomerTrades, ref numberOfRows, connectionString, date);
                bookTrades.DetectTradesAifSttXmltAndBookToAligne(input, ref outputDataList, ref importDone, false);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

                // Set input data the right wys for testing.
             
                //Assert.IsTrue(outputDataList[0].result.Equals("success"), "Upload.exe was not execuded.");
                //Assert.IsTrue(outputDataList[0].zKey.Length == 8, "Zkey from imported file was not read.");
                //Assert.IsTrue(outputDataList[0].tradeNo.Equals("17695912"), "TradeNo/TradeID from imported file was not read.");
                //Assert.IsTrue(importDone, "Nothing was imported,");
        }
        #endregion
    }
}





