using System.Data;

namespace ViertelStdTool.Database
{
    public interface IDatabaseConnector
    {
        /// <summary>
        /// Close SQL connection.
        /// </summary>
        /// <returns></returns>
        void CloseConnection();
        /// <summary>
        /// Open database connection if not opned before.
        /// </summary>
        /// <param name="connectionstring"></param>
        void OpenConnection(string connectionstring);
        /// <summary>
        /// Get database name as string.
        /// </summary>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        void GetDatabaseName(ref string databaseName);
        /// <summary>
        /// Read data table from database.
        /// </summary>
        /// <param name="querySelect"></param>
        /// <param name="table"></param>
        /// <param name="numberOfEntries"></param>
        void DoSelect(string querySelect, ref DataTable table, ref int numberOfEntries);
    }
}