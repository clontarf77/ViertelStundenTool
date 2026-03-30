using System.Data;

namespace ViertelStdTool.Likron
{
    #region structures
    public struct OrderDataGenerationData
    {
        public string pathLoadCSV;
        public string pathOrderLikron;
        public string pathOrderLikronSmaller30Min;
        public string pathLimitExcel;
        public string tableName;
        public bool sumQuantityIsZero;
        public string loadLimit;
        public string sellLimit;
        public string buyLimit;
        public LimitType limitType;
        public string nameOfGeneratedFile;
        public string nameOfGeneratedFileSmaller30Min;
        public DataTable loadDataTable;
        public DataTable limitDataTable;
        public DataTable likronDataTableAll;
        public DataTable likronDataTable;
        public DataTable likronDataTableSmaller30Min;
        public ValidityEnd validityEnd;
    }

    public struct ResultsQuarterHourToLikron
    {
        public string nameOfLoadedFile;
        public string nameOfGeneratedFile;
        public string nameOfGeneratedFileSmaller30Min;
        public string lastExecutionTimeWithNewData;       
        public string lastExecutionTime;
        public DataTable loadDataTable;
        public DataTable likronDataTable;
        public DataTable limitDataTable;
    }

    #endregion

    #region enums
    public enum LimitType
    { 
        NoLimit,
        MWh,
        Percent
    };
        
    public enum ControlArea
    {
        RWE_genettet,
        Regelzonenscharf
    };

    public enum Range
    {
        normal,
        TuD,
        delivery
    };

    public enum ValidityEnd
    {
        source,        
        empty
    };
    #endregion
}


