namespace ViertelStdToolLib.Aligne.Importer.DealStructure
{  
    #region structures
    public struct ParameterXmlGeneration
    {
        public string path;
        public string fileNameGeneratedXml;
        public BuySell buySell;
        public ControlArea controlArea;
        public string trader;
        public string memo1;
        public string memo2;
        public string tradeDate;
        public string deliveryDate;
        public string contract;
        public string quantity;
        public string price;
        public string tradeNo;
        public string product;
    }   
    #endregion

    #region enums
    public enum ImportFileType
    {
        CSV,
        XML
    };

    public enum PwEnCrypton
    {
        SIMPLE_PASS,
        PW_MANAGER,
        PLAIN
    };

    public enum BuySell
    {
        BUY,
        SELL
    };

    public enum ControlArea
    {
        AMP,
        TENNET,
        ENBW,
        _50HZ
    };
    #endregion


}
