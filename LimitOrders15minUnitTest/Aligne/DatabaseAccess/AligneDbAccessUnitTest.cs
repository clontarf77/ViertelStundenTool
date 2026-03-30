using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using ViertelStdToolLib.Aligne.DataBaseRequests;

namespace UnitTestAligneDbAccess
{
    [TestClass]
    public class AligneDbAccessUnitTest
    {
        #region Read booked VSK Customer Trades from PROD3 Base - These are the trades already booked from the tool.
        [TestMethod]
        public void ReadBookedVSKCustomerTradesFromProd3Base()
        {
            string retValue = string.Empty;
            DataTable vskTradesResult = new DataTable();
            int numberOfRows = 0;
            string date = DateTime.Now.ToShortDateString(); 

            // Connection string for Database PROD3 Base
            const string connectionString = "Data Source=\"(DESCRIPTION = (ADDRESS = (PROTOCOL = tcp)(HOST = hvkhsprdb)(PORT = 1521))(CONNECT_DATA = (SID = khsprdb)))\";User ID=VTool;Password=XSjqmGd/KYvERtXDzSVO6EKuaMZUtdQt8kYAdPxnHHw=";

            try
            {
                IDatabaseAccess dbAccess = new DatabaseAccess();
                retValue = dbAccess.ReadBookedVskTradesFromAligne(ref vskTradesResult, ref numberOfRows, connectionString, date);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }

            Assert.IsTrue(retValue.Equals("OK"), "Error occured during reading VSK trades.");
        }
        #endregion       

        #region Read booked VSK Customer Trades from DEV3 Base - These are the trades already booked from the tool.  
        [TestMethod]
        public void ReadBookedVSKCustomerTradesFromDev3Base()
        {
            string retValue = string.Empty;
            DataTable vskTradesResult = new DataTable();
            int numberOfRows = 0;
            string date = DateTime.Now.ToShortDateString();

            // Connection string for Database DEV3 Base
            const string connectionString = "Data Source=\"(DESCRIPTION = (ADDRESS = (PROTOCOL = tcp)(HOST = VKHST2B)(PORT = 1521))(CONNECT_DATA = (SID = khst2b)))\";User ID=VTool;Password=ZCc4BG9ZcaK+ihLrHwtPiT6JFte7hOBrYbvE7tetti4=";

            try
            {
                IDatabaseAccess dbAccess = new DatabaseAccess();
                retValue = dbAccess.ReadBookedVskTradesFromAligne(ref vskTradesResult, ref numberOfRows, connectionString, date);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }

            Assert.IsTrue(retValue.Equals("OK"), "Error occured during reading VSK trades.");
        }
        #endregion

        #region Read received VSK Trades from PROD3 Base - These are the trades generated in Likron received via EPEX interface.
        [TestMethod]
        public void ReadReceivedVSKTradesFromProd3Base()
        {
            string retValue = string.Empty;
            DataTable vskTradesResult = new DataTable();
            int numberOfRows = 0;
            string date = DateTime.Now.ToShortDateString();

            // Connection string for Database PROD3 Base
            const string connectionString = "Data Source=\"(DESCRIPTION = (ADDRESS = (PROTOCOL = tcp)(HOST = hvkhsprdb)(PORT = 1521))(CONNECT_DATA = (SID = khsprdb)))\";User ID=VTool;Password=XSjqmGd/KYvERtXDzSVO6EKuaMZUtdQt8kYAdPxnHHw=";

            try
            {
                IDatabaseAccess dbAccess = new DatabaseAccess();
                retValue = dbAccess.ReadReceivedVskTradesFromAligne(ref vskTradesResult, ref numberOfRows, connectionString, date);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }

            Assert.IsTrue(retValue.Equals("OK"), "Error occured during reading VSK trades.");
        }
        #endregion       

        #region Read received VSK Trades from DEV3 Base - These are the trades generated in Likron received via EPEX interface. 
        [TestMethod]
        public void ReadReceivedVSKTradesFromDev3Base()
        {
            string retValue = string.Empty;
            DataTable vskTradesResult = new DataTable();
            int numberOfRows = 0;
            string date = DateTime.Now.ToShortDateString();

            // Connection string for Database DEV3 Base
            const string connectionString = "Data Source=\"(DESCRIPTION = (ADDRESS = (PROTOCOL = tcp)(HOST = VKHST2B)(PORT = 1521))(CONNECT_DATA = (SID = khst2b)))\";User ID=VTool;Password=ZCc4BG9ZcaK+ihLrHwtPiT6JFte7hOBrYbvE7tetti4=";

            try
            {
                IDatabaseAccess dbAccess = new DatabaseAccess();
                retValue = dbAccess.ReadReceivedVskTradesFromAligne(ref vskTradesResult, ref numberOfRows, connectionString, date);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }

            Assert.IsTrue(retValue.Equals("OK"), "Error occured during reading VSK trades.");
        }
        #endregion

        #region Read not booked VSK Customer Trades from PROD3 Base - These are the missing customer trades which have to be booked via the tool.
        [TestMethod]
        public void ReadNotBookedVSKTradesFromProd3Base()
        {
            string retValue = string.Empty;
            DataTable vskTradesResult = new DataTable();
            int numberOfRows = 0;
            string date = DateTime.Now.ToShortDateString();

            // Connection string for Database PROD3 Base
            const string connectionString = "Data Source=\"(DESCRIPTION = (ADDRESS = (PROTOCOL = tcp)(HOST = hvkhsprdb)(PORT = 1521))(CONNECT_DATA = (SID = khsprdb)))\";User ID=VTool;Password=XSjqmGd/KYvERtXDzSVO6EKuaMZUtdQt8kYAdPxnHHw=";

            try
            {
                IDatabaseAccess dbAccess = new DatabaseAccess();
                retValue = dbAccess.ReadNotBookedVskTrades(ref vskTradesResult, ref numberOfRows, connectionString, date);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }

            Assert.IsTrue(retValue.Equals("OK"), "Error occured during reading VSK trades.");
        }
        #endregion

        #region Read not booked VSK Customer Trades from DEV3 Base - These are the missing customer trades which have to be booked via the tool. 
        [TestMethod]
        public void ReadNotBookedVSKTradesFromDev3Base()
        {
            string retValue = string.Empty;
            DataTable vskTradesResult = new DataTable();
            int numberOfRows = 0;
            string date = DateTime.Now.ToShortDateString();

            // Connection string for Database DEV3 Base
            const string connectionString = "Data Source=\"(DESCRIPTION = (ADDRESS = (PROTOCOL = tcp)(HOST = VKHST2B)(PORT = 1521))(CONNECT_DATA = (SID = khst2b)))\";User ID=VTool;Password=ZCc4BG9ZcaK+ihLrHwtPiT6JFte7hOBrYbvE7tetti4=";

            try
            {
                IDatabaseAccess dbAccess = new DatabaseAccess();
                retValue = dbAccess.ReadNotBookedVskTrades(ref vskTradesResult, ref numberOfRows, connectionString, date);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }

            Assert.IsTrue(retValue.Equals("OK"), "Error occured during reading VSK trades.");
        }
        #endregion
                                  
        #region Read data for not booked VSK Customer Trade from PROD3 AIF STT - Get trade XML for missing customer trades. 
        [TestMethod]
        public void ReadDataNotBookedVSKTradesFromProd3AIFSTT()
        {
            string retValue = string.Empty;
            string tradeXML = string.Empty;
            // This is the tradeId for zkey base = 16367115
            string tradeId = "1245912686";

            // Connection string for Database PROD3 STT
            const string connectionStringSTT = "Data Source=\"(DESCRIPTION = (ADDRESS = (PROTOCOL = tcp)(HOST = hVKHSPrdNB)(PORT = 1521))(CONNECT_DATA = (SID = khsprdnb)))\";User ID=VTool;Password=GF2VhOL6AbBXJOjIwzU2ffWB5TbaCK7FHR9AxRvk3bU=";

            try
            {
                IDatabaseAccess dbAccess = new DatabaseAccess();
                retValue = dbAccess.ReadDataNotBookedVskCustomerTrades(ref tradeXML, connectionStringSTT, tradeId);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }

            Assert.IsTrue(retValue.Equals("OK"), "Error occured during reading VSK trades.");
        }
        #endregion
               
        #region Read data for not booked VSK Customer Trade from DEV3 AIF STT - Get trade XML for missing customer trades. 
        [TestMethod]
        public void ReadDataNotBookedVSKTradesFromDev3AIFSTT()
        {
            string retValue = string.Empty;
            string tradeXML = string.Empty;
            string tradeId = "1246585420";

            // Connection string for Database PROD3 STT 
            const string connectionStringSTT = "Data Source=\"(DESCRIPTION = (ADDRESS = (PROTOCOL = tcp)(HOST = VKHST2NB)(PORT = 1521))(CONNECT_DATA = (SID = khst2nb)))\";User ID=VTool;Password=qVjm5ItwzbkJ4pSnut4yu9eBWHubmeNuE7W00yrNDTM=";  
            try
            {
                IDatabaseAccess dbAccess = new DatabaseAccess();
                retValue = dbAccess.ReadDataNotBookedVskCustomerTrades(ref tradeXML, connectionStringSTT, tradeId);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }

            Assert.IsTrue(retValue.Equals("OK"), "Error occured during reading VSK trades.");
        }
        #endregion
    }
}


