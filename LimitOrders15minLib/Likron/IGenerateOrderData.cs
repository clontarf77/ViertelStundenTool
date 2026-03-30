namespace ViertelStdTool.Likron
{
    public interface IGenerateOrderData
    {
        /// <summary>
        /// Generate  Order Data for Likron - Normal for Control Area RWE.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>Ok in case of success</returns>
        string GenerateCsvForLikron(ref OrderDataGenerationData parameter);

        /// <summary>
        ///  Generate  Order Data for Likron - TuD - Per Control Area.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>Ok in case of success</returns>
        string GenerateCsvForLikronTuD(ref OrderDataGenerationData parameter);
    }
}

