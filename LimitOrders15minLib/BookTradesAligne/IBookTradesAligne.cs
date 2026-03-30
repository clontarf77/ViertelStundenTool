using System.Collections.Generic;
using System.Data;

namespace ViertelStdToolLib.BookTradesAligne
{
    public interface IBookTradesAligne
    {
        /// <summary>
        /// Detect trades via AIF STT XML and import trades to Aligne REST.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="outputDataList"></param>
        /// <param name="importDone"></param>                
        /// <param name="ignoreAlredayBookedTrades"></param>
        /// <returns></returns>
        string DetectTradesAifSttXmltAndBookToAligneREST(InputDataXmlAifStt input, ref List<OutputData> outputDataList, ref bool importDone, bool ignoreAlredayBookedTrades = true);
               
        /// <summary>
        /// Detect trades via Comtrader Export and import trades to Aligne.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="outputDataList"></param>
        /// <param name="importDone"></param>
        /// <param name="vskTradesResult"></param>        
        /// <param name="ignoreAlredayBookedTrades"></param>
        /// <returns></returns>
        string DetectTradesComTraderExportAndBookToAligne(InputDataComtraderExport input, ref List<OutputData> outputDataList, ref bool importDone, DataTable vskTradesResult, bool ignoreAlredayBookedTrades = true);
    
        /// <summary>
        /// Detect trades via AIF STT XML and import trades to Aligne.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="outputDataList"></param>
        /// <param name="importDone"></param>                
        /// <param name="ignoreAlredayBookedTrades"></param>
        /// <returns></returns>
        string DetectTradesAifSttXmltAndBookToAligne(InputDataXmlAifStt input, ref List<OutputData> outputDataList, ref bool importDone, bool ignoreAlredayBookedTrades = true);
    }
}