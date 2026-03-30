using System;
using System.Data;

namespace ViertelStdToolLib.Comtrader
{
    #region structures
    public struct ResultTable
    {
        public DataTable table;
        public int numberOfRows;
    }

    public struct InputFilter
    {
        public string date;
        public TextMemo2 text;
        public TextMemo2 textAlternative;
        public TraderId traderId;
    }
    #endregion

    #region enums
    public enum TextMemo2
    {
        ALL,
        VSK,
        BEEGY,
        ITA_AUTOTRADING,
        L_PArb
    };

    public enum TraderId
    {
        ALL,        
        TRD010, // Likron-Prod
        TRD001 // Likron-Test
    };
    #endregion  
}


