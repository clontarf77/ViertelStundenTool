namespace ViertelStdToolLib.Comtrader
{
    public interface IReadExport
    {
        /// <summary>
        /// Export deals from Comtrader Export as DataTable.
        /// </summary>
        /// <param name="pathComtraderExport"></param>
        /// <param name="filter"></param>
        /// <param name="resultTable"></param>        
        /// <param name="startAtRow">optional</param>     
        /// <returns></returns>
        string ExportDealsComtrader(string pathComtraderExport, InputFilter filter, ref ResultTable resultTable, uint startAtRow = 0);
    }
}