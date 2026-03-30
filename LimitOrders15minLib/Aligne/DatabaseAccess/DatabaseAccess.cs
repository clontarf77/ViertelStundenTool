using System;
using System.Data;
using System.Reflection;
using ViertelStdTool.Database;
using ViertelStdTool.Log;

namespace ViertelStdToolLib.Aligne.DataBaseRequests
{
    public class DatabaseAccess : IDatabaseAccess
    {
        private readonly INLogger logger = new NLogger();

        #region Read all trades with memo VSK. These trades are booked from ViertelstundenTool via Importer to Aligne.
        /// <summary>
        /// Read all trades with memo VSK. These trades are booked from ViertelstundenTool via Importer to Aligne.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="connectionString"></param>
        /// <param name="date"></param>        
        /// <returns></returns>
        public string ReadBookedVskTradesFromAligne(ref DataTable table, ref int numberOfRows, string connectionString, string date)
        {
            string retVal = "OK";

            IDatabaseConnector connector;
            string querySelect = string.Empty;
            // Empty data before process begins.
            table.Clear();
            numberOfRows = 0;                      

            querySelect = "TO_CHAR(trade_tdate,'yyyy/MM/dd') as TradeDate, audit_atime0 as TradeTime, " +
                            "audit_zkey as ZKEY, " +
                            "TRIM(trade_memo) as MEMO, " +
                            "TRIM(trade_memo2) as MEMO2 " +
                            "from risk.tpow " +
                            "where " +
                            "audit_active = 1 " +
                            "and trade_tdate >= " + "'" + date + "' " +
                            "and trade_memo = 'VSK' " +
                            "and audit_util2 NOT IN ('7502', '6666', '7500', '8000') " +
                            "order by audit_zkey desc";

            try
            {
                // Connection string ODP.NET with TNS --> Used for Aligne Aligne Base
                connector = new DatabaseConnector();
                {
                    connector.OpenConnection(connectionString);
                    connector.DoSelect(querySelect, ref table, ref numberOfRows);
                    logger.WriteInfo("ReadAndBookTrades Thread: Read trades with memo VSK from Aligne DB for " + date);
                    connector.CloseConnection();
                }
            }
            catch (Exception exception)
            {
                logger.WriteError("Exception in : " + (MethodBase.GetCurrentMethod().Name) + "(): " + exception.Message.ToString());
                retVal = "Error " + exception.Message.ToString();
            }

            return retVal;
        }
        #endregion

        #region Read all trades with memo2 VSK. These trades are received in Aligne via EPEX Interface. 
        /// <summary>
        /// Read all trades with memo2 VSK. These trades are received in Aligne via EPEX Interface. 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="connectionString"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public string ReadReceivedVskTradesFromAligne(ref DataTable table, ref int numberOfRows, string connectionString, string date)
        {
            string retVal = "OK";

            IDatabaseConnector connector;
            string querySelect = string.Empty;
            // Empty data before process begins.
            table.Clear();
            numberOfRows = 0;

            querySelect = "substr(import_xval_xstring,1,instr(import_xval_xstring, '|', 1)-1) as tradeID, " +
                            "audit_zkey, audit_adate0 as TradeDate, audit_atime0 as TradeTime " +
                            "from risk.import_xval, risk.tpow " +                         
                            "where " +
                            "audit_active = 1" +
                            "and audit_xkey = import_xval_xkey " +
                            "and audit_util2 = 7500  " +
                            "and trade_tdate = " + "'" + date + "' " +
                            "and trade_memo2 = 'VSK' " +                           
                            "order by audit_zkey desc";

            try
            {
                // Connection string ODP.NET with TNS --> Used for Aligne Aligne Base
                connector = new DatabaseConnector();
                {
                    connector.OpenConnection(connectionString);
                    connector.DoSelect(querySelect, ref table, ref numberOfRows);
                    logger.WriteInfo("ReadAndBookTrades Thread: Read trades with memo VSK from Aligne DB for " + date);
                    connector.CloseConnection();
                }
            }
            catch (Exception exception)
            {
                logger.WriteError("Exception in : " + (MethodBase.GetCurrentMethod().Name) + "(): " + exception.Message.ToString());
                retVal = "Error " + exception.Message.ToString();
            }

            return retVal;
        }
        #endregion

        #region Read all trades with memo2 VSK and memo VSK. Check which trades with memo VSK are missing. Which meams find trades where customer trade was not booked.
        /// <summary>
        /// Read all trades with memo2 VSK and memo VSK. Check which trades with memo VSK are missing. Which meams find trades where customer trade was not booked.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="connectionString"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public string ReadNotBookedVskTrades(ref DataTable table, ref int numberOfRows, string connectionString, string date)
        {
            string retVal = "OK";

            IDatabaseConnector connector;
            string querySelect = string.Empty;
            // Empty data before process begins.
            table.Clear();
            numberOfRows = 0;

            querySelect = "TRIM(substr(import_xval_xstring,1,instr(import_xval_xstring, '|', 1)-1)) as tradeID " +
                            "from risk.tpow p, risk.import_xval v " +
                            "where " +
                            "p.trade_tdate = " + "'" + date + "' " +
                            "and p.trade_memo2 = 'VSK' " +
                            "and p.audit_active = 1 " +
                            "and p.audit_xkey = v.import_xval_xkey " +
                            "and p.audit_util2 = 7500 " +                           
                            "MINUS " +
                            "select TRIM(substr(trade_memo2, 1, instr(trade_memo2, '-')-1)) as tradeID " +
                            "from risk.tpow p " +
                            "where " +
                            "p.audit_active = 1 " +
                            "and p.trade_tdate = " + "'" + date + "' " +
                            "and p.trade_memo = 'VSK' " +
                            "and p.audit_util2 NOT IN('7502', '6666', '7500', '8000')";

            try
            {
                // Connection string ODP.NET with TNS --> Used for Aligne Aligne Base
                connector = new DatabaseConnector();
                {
                    connector.OpenConnection(connectionString);
                    connector.DoSelect(querySelect, ref table, ref numberOfRows);
                    logger.WriteInfo("ReadAndBookTrades Thread: Read trades with memo VSK from Aligne DB for " + date);
                    connector.CloseConnection();
                }
            }
            catch (Exception exception)
            {
                logger.WriteError("Exception in : " + (MethodBase.GetCurrentMethod().Name) + "(): " + exception.Message.ToString());
                retVal = "Error " + exception.Message.ToString();
            }

            return retVal;
        }
        #endregion

        #region Read trade XML from AIF STT for given trade id. 
        /// <summary>
        /// Read trade XML from AIF STT for given trade id. 
        /// </summary>
        /// <param name="tradeXML"></param>                 
        /// <param name="connectionStringSTT"></param>
        /// <param name="tradeid"></param>
        /// <returns></returns>
        public string ReadDataNotBookedVskCustomerTrades(ref string tradeXML, string connectionStringSTT, string tradeId)
        {
            string retVal = "OK";

            IDatabaseConnector connector;
            string querySelect = string.Empty;            
            int numberOfRows = 0;
            DataTable table = new DataTable();
           
            try
            {           
                // Connection string ODP.NET with TNS --> Used for Aligne STT
                connector = new DatabaseConnector();
                {                    
                    // Get XString from STT System.
                    connector.OpenConnection(connectionStringSTT);
                    
                    querySelect = "m1.msg_uid, m1.source_msg_id, m3.xml_data ,m2.value, m1.create_date " + "" +
                                     "from AIF.MSG m1, AIF.MSG_HEADER m2, AIF.MSG_DETAIL m3, risk.EXCHANGE_TRADER_TRADES ett " + 
                                     "where m1.source_msg_id = m2.source_msg_id " + 
                                     "and m1.msg_uid = m3.msg_id " + "" +
                                     "and m2.value = ett.exchange_trade_id || '|' || ett.client_order_id || '|SIEEX|' || ett.buy_sell " +
                                     "and ett.exchange_trade_id = " + tradeId + " " + 
                                     "order by m1.msg_uid desc ";

                    connector.DoSelect(querySelect, ref table, ref numberOfRows);
                    connector.CloseConnection();
                    logger.WriteInfo("ReadDataNotBookedVskCustomerTrades: Read Trade XML for trade with trade Id" + tradeId + ".");
                }

                tradeXML = table.Rows[0]["XML_DATA"].ToString();
            }
            catch (Exception exception)
            {
                logger.WriteError("Exception in : " + (MethodBase.GetCurrentMethod().Name) + "(): " + exception.Message.ToString());
                retVal = "Error " + exception.Message.ToString();
            }

            return retVal;
        }
        #endregion
    }
}
