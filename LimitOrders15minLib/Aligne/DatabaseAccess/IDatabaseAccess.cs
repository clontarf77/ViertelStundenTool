using System.Data;

namespace ViertelStdToolLib.Aligne.DataBaseRequests
{
    public interface IDatabaseAccess
    {
        /// <summary>
        /// Read all trades with memo VSK. These trades are booked from ViertelstundenTool via Importer to Aligne.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="connectionString"></param>
        /// <param name="date"></param>        
        /// <returns></returns>
        string ReadBookedVskTradesFromAligne(ref DataTable table, ref int numberOfRows, string connectionString, string date);

        /// <summary>
        /// Read all trades with memo2 VSK. These trades are received in Aligne via EPEX Interface. 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="connectionString"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        string ReadReceivedVskTradesFromAligne(ref DataTable table, ref int numberOfRows, string connectionString, string date);

        /// <summary>
        /// Read all trades with memo2 VSK and memo VSK. Check which trades with memo VSK are missing. Which meams find trades where customer trade was not booked.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="connectionString"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        string ReadNotBookedVskTrades(ref DataTable table, ref int numberOfRows, string connectionString, string date);

        /// <summary>
        /// Read trade XML from AIF STT for given trade id.
        /// </summary>
        /// <param name="tradeXML"></param>                
        /// <param name="connectionStringSTT"></param>
        /// <param name="tradeId"></param>
        /// <returns></returns>
        string ReadDataNotBookedVskCustomerTrades(ref string tradeXML, string connectionStringSTT, string tradeId);
    }
}