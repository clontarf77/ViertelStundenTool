using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ViertelStdTool.Database;

namespace UnitTestDatabaseConnector
{
    [TestClass]
    public class DatabaseConnectorUnitTest
    {
        #region ConnectToDB PROD3 Base
        // Connect to Database.
        [TestMethod]
        public void ConnectToDB_Prod3Base()
        {
            IDatabaseConnector connector;
            string databaseName = string.Empty;
            // Connection string for Database PROD3 Base
            const string connectionString = "Data Source=\"(DESCRIPTION = (ADDRESS = (PROTOCOL = tcp)(HOST = hvkhsprdb)(PORT = 1521))(CONNECT_DATA = (SID = khsprdb)))\";User ID=VTool;Password=XSjqmGd/KYvERtXDzSVO6EKuaMZUtdQt8kYAdPxnHHw=";
            string dataBaseNameExpected = "KHSPRDB";

            try
            {
                connector = new DatabaseConnector();
                {
                    connector.OpenConnection(connectionString);
                    connector.GetDatabaseName(ref databaseName);
                    connector.CloseConnection();
                }
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }

            Assert.AreEqual(databaseName, dataBaseNameExpected, "Name of database could not be read.");
        }
        #endregion

        #region ConnectToDB DEV3 Base
        // Connect to Database.
        [TestMethod]
        public void ConnectToDB_DEV3Base()
        {
            IDatabaseConnector connector;
            string databaseName = string.Empty;
            // Connection string for Database DEV3 Base
            const string connectionString = "Data Source=\"(DESCRIPTION = (ADDRESS = (PROTOCOL = tcp)(HOST = VKHST2B)(PORT = 1521))(CONNECT_DATA = (SID = khst2b)))\";User ID=VTool;Password=ZCc4BG9ZcaK+ihLrHwtPiT6JFte7hOBrYbvE7tetti4=";
            string dataBaseNameExpected = "KHST2B";

            try
            {
                connector = new DatabaseConnector();
                {
                    connector.OpenConnection(connectionString);
                    connector.GetDatabaseName(ref databaseName);
                    connector.CloseConnection();
                }
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }

            Assert.AreEqual(databaseName, dataBaseNameExpected, "Name of database could not be read.");
        }
        #endregion

        #region ConnectToDB PROD3 STT
        // Connect to Database STT.
        [TestMethod]
        public void ConnectToDB_Prod3STT()
        {
            IDatabaseConnector connector;
            string databaseName = string.Empty;
            // Connection string for Database PROD3 Base
            const string connectionString = "Data Source=\"(DESCRIPTION = (ADDRESS = (PROTOCOL = tcp)(HOST = hvkhsprdnb)(PORT = 1521))(CONNECT_DATA = (SID = khsprdnb)))\";User ID=VTool;Password=GF2VhOL6AbBXJOjIwzU2ffWB5TbaCK7FHR9AxRvk3bU=";
            string dataBaseNameExpected = "KHSPRDNB";

            try
            {
                connector = new DatabaseConnector();
                {
                    connector.OpenConnection(connectionString);
                    connector.GetDatabaseName(ref databaseName);
                    connector.CloseConnection();
                }
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }

            Assert.AreEqual(databaseName, dataBaseNameExpected, "Name of database could not be read.");
        }
        #endregion

        #region ConnectToDB DEV3 STT
        // Connect to Database STT.
        [TestMethod]
        public void ConnectToDB_DEV3STT()
        {
            IDatabaseConnector connector;
            string databaseName = string.Empty;
            // Connection string for Database DEV3 Base
            const string connectionString = "Data Source=\"(DESCRIPTION = (ADDRESS = (PROTOCOL = tcp)(HOST = VKHST2NB)(PORT = 1521))(CONNECT_DATA = (SID = khst2nb)))\";User ID=VTool;Password=qVjm5ItwzbkJ4pSnut4yu9eBWHubmeNuE7W00yrNDTM=";
            string dataBaseNameExpected = "KHST2NB";

            try
            {
                connector = new DatabaseConnector();
                {
                    connector.OpenConnection(connectionString);
                    connector.GetDatabaseName(ref databaseName);
                    connector.CloseConnection();
                }
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }

            Assert.AreEqual(databaseName, dataBaseNameExpected, "Name of database could not be read.");
        }
        #endregion

        #region Try to read database name without connection.
        // Connect to Database.
        [TestMethod]
        [ExpectedException(typeof(System.Exception), AllowDerivedTypes = true)]
        public void ReadDBNameWithoutConnection()
        {
            IDatabaseConnector connector;
            string databaseName = string.Empty;
            string dataBaseNameExpected = string.Empty;

            try
            {
                connector = new DatabaseConnector();
                {
                    connector.GetDatabaseName(ref databaseName);
                }
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }

            Assert.AreEqual(databaseName, dataBaseNameExpected, "Name of database should be empty.");
        }
        #endregion

        #region Try to close database without connection.
        // Connect to Database.
        [TestMethod]
        [ExpectedException(typeof(System.Exception), AllowDerivedTypes = true)]
        public void CloseDBWithoutConnection()
        {
            IDatabaseConnector connector;

            try
            {
                connector = new DatabaseConnector();
                {
                    connector.CloseConnection();
                }
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }
        }
        #endregion
    }
}



