using Oracle.ManagedDataAccess.Client;
using PasswordEnDeCrypter;
using System;
using System.Data;
using System.Reflection;
using ViertelStdTool.Log;

namespace ViertelStdTool.Database
{
    public class DatabaseConnector : IDatabaseConnector
    {
        private OracleConnection connection;
        private readonly INLogger logger = new NLogger();

        #region OpenConnection
        /// <summary>
        /// Open database connection if not opned before.
        /// </summary>
        /// <param name="connectionstring"></param>
        public void OpenConnection(string connectionString)
        {
            // Decrypt password in connection string.
            System.Data.SqlClient.SqlConnectionStringBuilder csb = new System.Data.SqlClient.SqlConnectionStringBuilder(connectionString);
            csb.Password = Encoder.DecryptString(csb.Password, "PESTRASIRUSE");

            try
            {
                if (connection == null)
                {
                    connection = new OracleConnection(csb.ToString());
                }
                // Open connection if closed.
                if (connection.State == ConnectionState.Closed)
                {
                    // Establish Oracle connection.
                    connection.Open();
                }
            }
            catch (Exception exception)
            {
                exception.Data.Add("OpenConnection", "Exception in " + (MethodBase.GetCurrentMethod().Name) + "(): ");
                logger.WriteError("Exception in : " + (MethodBase.GetCurrentMethod().Name) + "(): " + exception.Message.ToString());
                throw exception;
            }
        }
        #endregion

        #region CloseConnection
        /// <summary>
        /// Close SQL connection.
        /// </summary>
        /// <returns></returns>
        public void CloseConnection()
        {
            try
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            catch (Exception exception)
            {
                exception.Data.Add("CloseConnection", "Exception in " + (MethodBase.GetCurrentMethod().Name) + "(): ");
                logger.WriteError("Exception in : " + (MethodBase.GetCurrentMethod().Name) + "(): " + exception.Message.ToString());
                throw exception;
            }
        }
        #endregion

        #region GetDatabaseName
        /// <summary>
        /// Get database name as string.
        /// </summary>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        public void GetDatabaseName(ref string databaseName)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                databaseName = connection.DatabaseName;
            }
            catch (Exception exception)
            {
                exception.Data.Add("GetDatabaseName", "Exception in " + (MethodBase.GetCurrentMethod().Name) + "(): ");
                logger.WriteError("Exception in : " + (MethodBase.GetCurrentMethod().Name) + "(): " + exception.Message.ToString());
                throw exception;
            }
        }
        #endregion

        #region ReadData
        /// <summary>
        /// Read data table from database.
        /// </summary>
        /// <param name="querySelect"></param>
        /// <param name="table"></param>
        /// <param name="numberOfEntries"></param>
        public void DoSelect(string querySelect, ref DataTable table, ref int numberOfEntries)
        {
            try
            {
                querySelect = "SELECT " + querySelect;

                using (OracleCommand command = new OracleCommand(querySelect, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        table = new DataTable();
                        table.Load(reader);
                        numberOfEntries = table.Rows.Count;
                    }
                }
            }
            catch (Exception exception)
            {
                exception.Data.Add("DoSelect", "Exception in " + (MethodBase.GetCurrentMethod().Name) + "(): ");
                logger.WriteError("Exception in : " + (MethodBase.GetCurrentMethod().Name) + "(): " + exception.Message.ToString());
                throw exception;
            }
        }
        #endregion
    }
}

