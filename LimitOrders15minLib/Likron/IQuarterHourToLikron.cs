using ViertelStdTool.ConfigurationHandler;

namespace ViertelStdTool.Likron
{
    public interface IQuarterHourToLikron
    {
        /// <summary>
        /// Do the init.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns>Ok in case of success.</returns>       
        string Init(ConfigurationStructure configuration);

        /// <summary>
        /// Write the quarter hour orders to Likron.
        /// </summary>
        /// <param name="limitType"></param>
        /// <param name="loadLimit"></param>
        /// <param name="sellLimit"></param>
        /// <param name="buyLimit"></param>
        /// <param name="fileStartsWith"></param>
        ///  /// <param name="results"></param>
        /// <returns>Ok in case of success.</returns>
        string WriteQuarterHourOrderToLikron(LimitType limitType, string loadLimit, string sellLimit, string buyLimit, string fileStartsWith, ref ResultsQuarterHourToLikron results);

        /// <summary>
        /// Write the quarter hour orders to Likron for TuD.
        /// </summary>
        /// <param name="limitType"></param>
        /// <param name="loadLimit"></param>    
        /// <param name="sellLimit"></param>
        /// <param name="buyLimit"></param>
        /// <param name="fileStartsWith"></param>
        /// <param name="results"></param>
        /// <returns>Ok in case of success.</returns>
        string WriteQuarterHourOrderToLikronTuD(LimitType limitType, string loadLimit, string sellLimit, string buyLimit, string fileStartsWith, ref ResultsQuarterHourToLikron results);
                
        /// <summary>
        /// Get range of given contract.
        /// </summary>
        /// <param name="contract"></param>
        /// <param name="contractRange"></param>
        /// <param name="additionalRangeInfo"></param>
        /// <returns>Ok if successful.</returns>
        string GetContractRange(string contract, ref Range contractRange, ref string additionalRangeInfo);
    }
}