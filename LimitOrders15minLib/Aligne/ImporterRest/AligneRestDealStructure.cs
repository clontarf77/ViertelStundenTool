namespace ViertelStdToolLib.Aligne.Importer.DealStructure.Rest
{  
    #region structures
    public struct TradeClassToSerialize
    {
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
    public enum ShapeType
    {
        _15min,
        _30min,
        hour        
    }
    #endregion
}



