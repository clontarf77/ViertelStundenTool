using System.Data;
using System.Xml;
using ViertelStdToolLib.Aligne.Importer.DealStructure;
using ViertelStdToolLib.Comtrader;

namespace ViertelStdToolLib.BookTradesAligne
{
    #region structures
    public struct InputDataComtraderExport
    {
        public string date;
        public string fileNameGeneratedXml;
        public string trader;
        public string password;
        public string server;
        public PwEnCrypton pwEnCrypton;
        public ResultTable resultComtraderExport;        
    }

    public struct InputDataXmlAifStt
    {
        public string date;
        public string fileNameGeneratedXml;
        public string trader;
        public string password;
        public string server;
        public string connectionStringStt;
        public PwEnCrypton pwEnCrypton;
        public DataTable vskMissingCustomerTrades;
    }

    public struct OutputData
    {
        public string zKey;
        public string result;
        public string tradeNo;
    }
    #endregion    
}

