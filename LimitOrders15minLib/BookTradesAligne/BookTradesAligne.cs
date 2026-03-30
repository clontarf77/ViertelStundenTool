using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml;
using ViertelStdTool.AligneImporter;
using ViertelStdTool.Log;
using ViertelStdToolLib.Aligne.DataBaseRequests;
using ViertelStdToolLib.Aligne.Importer.DealStructure;
using ViertelStdToolLib.Aligne.Importer.Generate.Xml;
using ViertelStdToolLib.Comtrader;

namespace ViertelStdToolLib.BookTradesAligne
{
    public class BookTradesAligne : IBookTradesAligne
    {
        private readonly INLogger logger = new NLogger();
        private readonly IDatabaseAccess dbAccess = new DatabaseAccess();


        #region Detect trades via AIF STT XML and import trades to Aligne REST.
        /// <summary>
        /// Detect trades via AIF STT XML and import trades to Aligne REST.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="outputDataList"></param>
        /// <param name="importDone"></param>                
        /// <param name="ignoreAlredayBookedTrades"></param>
        /// <returns></returns>
        public string DetectTradesAifSttXmltAndBookToAligneREST(InputDataXmlAifStt input, ref List<OutputData> outputDataList, ref bool importDone, bool ignoreAlredayBookedTrades = true)
        {
            string retVal = "NOK - Process not finished.";
            bool errorInImport = false;
            string errorInformation = string.Empty;
            List<ParameterXmlGeneration> parameterList = new List<ParameterXmlGeneration>();

            // Set paths of excecutable and name of generated XML.
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(BookTradesAligne)).CodeBase);
            #region Workaround if started with long path. If not substituted path will exceed 260 chars and exception will be thrown.
            if (pathExecutable.StartsWith(@"file:\konzern\dfs\firmen"))
            {
                pathExecutable = pathExecutable.Replace(@"file:\konzern\dfs\firmen", "T:");
                logger.WriteInfo("Replaced path 'file:\\konzern\\dfs\\firmen' with 'T:'");
            }
            else if (pathExecutable.StartsWith("file:\\"))
            {
                pathExecutable = pathExecutable.Remove(0, 6);
            }
            else if (pathExecutable.StartsWith(@"\\konzern\dfs\Firmen"))
            {
                pathExecutable = pathExecutable.Replace(@"\\konzern\dfs\Firmen", "T:");
                logger.WriteInfo("Replaced path '\\\\konzern\\dfs\\Firmen' with 'T:'");
            }
            #endregion
            pathExecutable += @"\Aligne\Importer\Uploader";

            // Parameter for XML generation.           
            ParameterXmlGeneration parameter = new ParameterXmlGeneration()
            {
                path = pathExecutable,
                trader = input.trader,
                tradeDate = input.date
            };

            // Constructor for importer.
            IAligneImporter aligne = new AligneImporter(input.server, input.trader, input.password);

            // Output data structure for importer.
            OutputData output = new OutputData();

            try
            {
                IGenerateImporterXml xmlGenerator = new GenerateImporterXml();
                // Provide parameter for input XML.
                ProvideParameterForInputXML(ref parameterList, input, parameter);

                // Generate XML for Import 
                xmlGenerator.GenerateXmlForVsk(parameterList);

                if (parameterList.Count == 0)
                {
                    importDone = false;
                    logger.WriteInfo("ReadAndBookTrades Thread: No new trades from Likron detected, thus no Customer Trades were imported to Aligne.");
                }

                // AppDomainSetup before loop.
                errorInImport = false;

                // Loop via trades/xmls which have to be imported.
                foreach (ParameterXmlGeneration param in parameterList)
                {
                    // Reset values.
                    output.zKey = string.Empty;
                    output.result = string.Empty;
                    output.tradeNo = param.tradeNo;
                    // Import trade to Aligne. 
                    output.result = aligne.ImportDeal(param.fileNameGeneratedXml, pathExecutable, ImportFileType.XML, input.pwEnCrypton, ref output.zKey, false);
                    // Add output data to list.
                    outputDataList.Add(output);

                    // Write result for each trade to log file.
                    if (output.result.Equals("success"))
                    {
                        importDone = true;
                        logger.WriteInfo("ReadAndBookTrades Thread: Made Customer Trade for '" + output.tradeNo + "' and imported to Aligne.");
                    }
                    else
                    {
                        errorInImport = true;
                        logger.WriteError("ReadAndBookTrades Thread: MVV Trade with TradeID made in Likron '" + output.tradeNo + "' NOT imported to Aligne. '" + output.result + "'.");
                        retVal = "NOK - At least one trade not imported to Aligne. For more info see log";
                    }
                }

                // Just set return value to OK if all transfers worked fine.
                if (!errorInImport)
                {
                    retVal = "OK";
                }
            }
            catch (Exception exception)
            {
                output.result = exception.Message;
                logger.WriteError("Exception in : " + (MethodBase.GetCurrentMethod().Name) + "(): " + exception.Message.ToString());
                retVal = "NOK - Exception in : " + (MethodBase.GetCurrentMethod().Name) + "(): " + exception.Message.ToString();
            }

            return retVal;
        }
        #endregion

        #region Detect trades via AIF STT XML and import trades to Aligne.
        /// <summary>
        /// Detect trades via AIF STT XML and import trades to Aligne.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="outputDataList"></param>
        /// <param name="importDone"></param>               
        /// <param name="ignoreAlredayBookedTrades"></param>
        /// <returns></returns>
        public string DetectTradesAifSttXmltAndBookToAligne(InputDataXmlAifStt input, ref List<OutputData> outputDataList, ref bool importDone, bool ignoreAlredayBookedTrades = true)
        {
            string retVal = "NOK - Process not finished.";
            bool errorInImport = false;
            string errorInformation = string.Empty;
            List<ParameterXmlGeneration> parameterList = new List<ParameterXmlGeneration>();

            // Set paths of excecutable and name of generated XML.
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(BookTradesAligne)).CodeBase);
            #region Workaround if started with long path. If not substituted path will exceed 260 chars and exception will be thrown.
            if (pathExecutable.StartsWith(@"file:\konzern\dfs\firmen"))
            {
                pathExecutable = pathExecutable.Replace(@"file:\konzern\dfs\firmen", "T:");
                logger.WriteInfo("Replaced path 'file:\\konzern\\dfs\\firmen' with 'T:'");
            }
            else if (pathExecutable.StartsWith("file:\\"))
            {
                pathExecutable = pathExecutable.Remove(0, 6);
            }
            else if (pathExecutable.StartsWith(@"\\konzern\dfs\Firmen"))
            {
                pathExecutable = pathExecutable.Replace(@"\\konzern\dfs\Firmen", "T:");
                logger.WriteInfo("Replaced path '\\\\konzern\\dfs\\Firmen' with 'T:'");
            }
            #endregion
            pathExecutable += @"\Aligne\Importer\Uploader";                      

            // Parameter for XML generation.           
            ParameterXmlGeneration parameter = new ParameterXmlGeneration()
            {
                path = pathExecutable,
                trader = input.trader,
                tradeDate = input.date 
            };            

            // Constructor for importer.
            IAligneImporter aligne = new AligneImporter(input.server, input.trader, input.password);

            // Output data structure for importer.
            OutputData output = new OutputData();

            try
            {
                IGenerateImporterXml xmlGenerator = new GenerateImporterXml();
                // Provide parameter for input XML.
                ProvideParameterForInputXML(ref parameterList, input, parameter);                
                
                // Generate XML for Import
                xmlGenerator.GenerateXmlForVsk(parameterList);

                if (parameterList.Count == 0)
                {
                    importDone = false;
                    logger.WriteInfo("ReadAndBookTrades Thread: No new trades from Likron detected, thus no Customer Trades were imported to Aligne.");
                }

                // AppDomainSetup before loop.
                errorInImport = false;

                // Loop via trades/xmls which have to be imported.
                foreach (ParameterXmlGeneration param in parameterList)
                {
                    // Reset values.
                    output.zKey = string.Empty;
                    output.result = string.Empty;
                    output.tradeNo = param.tradeNo;
                    // Import trade to Aligne. 
                    output.result = aligne.ImportDeal(param.fileNameGeneratedXml, pathExecutable, ImportFileType.XML, input.pwEnCrypton, ref output.zKey, false);
                    // Add output data to list.
                    outputDataList.Add(output);

                    // Write result for each trade to log file.
                    if (output.result.Equals("success"))
                    {
                        importDone = true;
                        logger.WriteInfo("ReadAndBookTrades Thread: Made Customer Trade for '" + output.tradeNo + "' and imported to Aligne.");
                    }
                    else
                    {
                        errorInImport = true;
                        logger.WriteError("ReadAndBookTrades Thread: MVV Trade with TradeID made in Likron '" + output.tradeNo + "' NOT imported to Aligne. '" + output.result + "'.");
                        retVal = "NOK - At least one trade not imported to Aligne. For more info see log";
                    }
                }

                // Just set return value to OK if all transfers worked fine.
                if (!errorInImport)
                {
                    retVal = "OK";
                }
            }
            catch (Exception exception)
            {
                output.result = exception.Message;
                logger.WriteError("Exception in : " + (MethodBase.GetCurrentMethod().Name) + "(): " + exception.Message.ToString());
                retVal = "NOK - Exception in : " + (MethodBase.GetCurrentMethod().Name) + "(): " + exception.Message.ToString();
            }

            return retVal;
        }
        #endregion

        #region Detect trades via Comtrader Export and import trades to Aligne.
        /// <summary>
        /// Detect trades via Comtrader Export and import trades to Aligne.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="outputDataList"></param>
        /// <param name="importDone"></param>
        /// <param name="vskTradesResult"></param>        
        /// <param name="ignoreAlredayBookedTrades"></param>
        /// <returns></returns>
        public string DetectTradesComTraderExportAndBookToAligne(InputDataComtraderExport input, ref List<OutputData> outputDataList, ref bool importDone, DataTable vskTradesResult, bool ignoreAlredayBookedTrades = true)
        {
            string retVal = "NOK - Process not finished.";
            bool errorInImport = false;           

            // Set paths of excecutable and name of generated XML.
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(BookTradesAligne)).CodeBase);
            #region Workaround if started with long path. If not substituted path will exceed 260 chars and exception will be thrown.
            if (pathExecutable.StartsWith(@"file:\konzern\dfs\firmen"))
            {
                pathExecutable = pathExecutable.Replace(@"file:\konzern\dfs\firmen", "T:");
                logger.WriteInfo("Replaced path 'file:\\konzern\\dfs\\firmen' with 'T:'");
            }
            else if (pathExecutable.StartsWith("file:\\"))
            {
                pathExecutable = pathExecutable.Remove(0, 6);
            }
            else if (pathExecutable.StartsWith(@"\\konzern\dfs\Firmen"))
            {
                pathExecutable = pathExecutable.Replace(@"\\konzern\dfs\Firmen", "T:");
                logger.WriteInfo("Replaced path '\\\\konzern\\dfs\\Firmen' with 'T:'");
            }
            #endregion
            pathExecutable += @"\Aligne\Importer\Uploader";

            List<ParameterXmlGeneration> parameterList = new List<ParameterXmlGeneration>(); 
            
            // Parameter for XML generation.           
            ParameterXmlGeneration parameter = new ParameterXmlGeneration()
            {
                path = pathExecutable,                
                trader = input.trader,
                tradeDate = input.date // This is the date. Is identical to date in comtrader export.
            };

            string errorInformation = string.Empty;                       

            // Constructor for importer.
            IAligneImporter aligne = new AligneImporter(input.server, input.trader, input.password);

            // Output data structure for importer.
            OutputData output = new OutputData();            

            try
            {
                IGenerateImporterXml xmlGenerator = new GenerateImporterXml();
               
                if (ignoreAlredayBookedTrades && vskTradesResult.Rows.Count > 0)
                {
                    //Remove trades from table already booked to Aligne.
                    RemoveTradesAlreadyBookedToAligne(ref input.resultComtraderExport, vskTradesResult);
                }
                
                // Provide parameter for input XML.
                ProvideParameterForInputXmlReferenceComtrader(ref parameterList, input.resultComtraderExport, parameter);               
                // Generate XML for Import
                xmlGenerator.GenerateXmlForVsk(parameterList); 

                if (parameterList.Count == 0)
                {
                    importDone = false;
                    logger.WriteInfo("ReadAndBookTrades Thread: No new trades from Likron detected, thus no Customer Trades were imported to Aligne.");
                }

                // AppDomainSetup before loop.
                errorInImport = false; 

                // Loop via trades/xmls which have to be imported.
                foreach (ParameterXmlGeneration param in parameterList)
                {
                    // Reset values.
                    output.zKey = string.Empty;
                    output.result = string.Empty;
                    output.tradeNo = param.tradeNo;
                    // Import trade to Aligne. 
                    output.result = aligne.ImportDeal(param.fileNameGeneratedXml, pathExecutable, ImportFileType.XML, input.pwEnCrypton, ref output.zKey, false);
                    // Add output data to list.
                    outputDataList.Add(output);

                    // Write result for each trade to log file.
                    if(output.result.Equals("success"))
                    {
                        importDone = true;
                        logger.WriteInfo("ReadAndBookTrades Thread: Made Customer Trade for '" + output.tradeNo + "' and imported to Aligne.");                        
                    }
                    else
                    {
                        errorInImport = true;
                        logger.WriteError("ReadAndBookTrades Thread: MVV Trade with TradeID made in Likron '" + output.tradeNo + "' NOT imported to Aligne. '" + output.result + "'.");                        
                        retVal = "NOK - At least one trade not imported to Aligne. For more info see log"; 
                    }                    
                }

                // Just set return value to OK if all transfers worked fine.
                if (!errorInImport)
                {
                    retVal = "OK";
                }
            }
            catch (Exception exception)
            {
                output.result = exception.Message;
                logger.WriteError("Exception in : " + (MethodBase.GetCurrentMethod().Name) + "(): " + exception.Message.ToString());
                retVal = "NOK - Exception in : " + (MethodBase.GetCurrentMethod().Name) + "(): " + exception.Message.ToString(); 
            }

            return retVal;
        }
        #endregion
              
        
        
        #region Provide parameter for input XML use AIF STT XML as reference.
        /// <summary>
        /// Provide parameter for input XML use Comtrader-Export as reference.
        /// </summary>
        /// <param name="parameterList"></param>
        /// <param name="vskMissingCustomerTrades"></param>
        /// <param name="parameter"></param>
        private void ProvideParameterForInputXML(ref List<ParameterXmlGeneration> parameterList, InputDataXmlAifStt input, ParameterXmlGeneration parameter)
        {
            string tradeId = string.Empty;
            string tradeXml = string.Empty;

            foreach (DataRow resultRow in input.vskMissingCustomerTrades.Rows)
            {
                tradeId = resultRow["TRADEID"].ToString();
                // Read correspondig AIF XML from STT
                dbAccess.ReadDataNotBookedVskCustomerTrades(ref tradeXml, input.connectionStringStt, tradeId);
                               
                // Convert string to XMLDocument
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(tradeXml);
                // Get node tradeImport. 
                XmlNode tradeImport = xmlDoc.DocumentElement.FirstChild;
                
                //////////////////////////////////////////////////////////////////////////////////////////////////
                // Define if this a buy or a sell.
                XmlNode nodeBuySell = tradeImport.SelectSingleNode("//buysell");
                switch (nodeBuySell.InnerText)
                {
                    case "S":
                        parameter.buySell = BuySell.SELL; // Gegendeal (Customer Trade)
                        break;
                    case "B":
                        parameter.buySell = BuySell.BUY; // Gegendeal (Customer Trade)                    
                        break;
                    default:
                        logger.WriteError("Error in : " + (MethodBase.GetCurrentMethod().Name) + "(): Could not get B_S from AIF XML STT.");
                        break;
                }
                //////////////////////////////////////////////////////////////////////////////////////////////////
                // Get Control area.
                XmlNode mkt1 = tradeImport.SelectSingleNode("//mkt1");
                switch (mkt1.InnerText)
                {
                    case "50HzT":
                        parameter.controlArea = ControlArea._50HZ;
                        break;
                    case "AMP":
                        parameter.controlArea = ControlArea.AMP;
                        break;
                    case "EMBW":
                        parameter.controlArea = ControlArea.ENBW;
                        break;
                    case "TENNET":
                        parameter.controlArea = ControlArea.TENNET;
                        break;
                    default:
                        logger.WriteError("Error in : " + (MethodBase.GetCurrentMethod().Name) + "(): Could not get area from AIF XML STT.");
                        break;
                }
                //////////////////////////////////////////////////////////////////////////////////////////////////
                // Set Contract.
                XmlNode times = tradeImport.SelectSingleNode("//times");
                XmlNode timee = tradeImport.SelectSingleNode("//timee");

                int quarter = 1; 
                int hourStart = Convert.ToInt16(times.InnerText.Substring(0, 2));
                int hourEnd = Convert.ToInt16(timee.InnerText.Substring(0, 2));
                int quarterStart = Convert.ToInt16(times.InnerText.Substring(2, 2));
                int quarterEnd = Convert.ToInt16(timee.InnerText.Substring(2, 2));

                if ((quarterStart >= 00) & (quarterStart <= 45) & (quarterEnd >= 00) & (quarterEnd <= 45))
                {
                    if ((hourStart >= 00) & (hourEnd <= 23))
                    {
                        // Determine the quarter needed for Aligne.
                        switch (quarterStart)
                        {
                            case 00:
                                quarter = 1;
                                break;
                            case 15:
                                quarter = 2;
                                break;
                            case 30:
                                quarter = 3;
                                break;
                            case 45:
                                quarter = 4;
                                break;
                        }

                        // Determine contract needed for Aligne.
                        parameter.contract = hourStart.ToString("D2") + "Q" + quarter.ToString();
                    }
                    else
                    {                        
                        // Do the logging.
                        logger.WriteError("NOK - Wrong product.Wrong hour.");
                    }
                }
                else
                {                    
                    // Do the logging.
                    logger.WriteError("NOK - Wrong product. Wrong quarter.(Hour not checked.)");
                }
                //////////////////////////////////////////////////////////////////////////////////////////////////
                // Set quantity.                                  
                XmlNode amount = tradeImport.SelectSingleNode("//amount");
                parameter.quantity = amount.InnerText;
                //////////////////////////////////////////////////////////////////////////////////////////////////
                // Set price.                                  
                XmlNode price = tradeImport.SelectSingleNode("//price");
                parameter.price = price.InnerText;
                //////////////////////////////////////////////////////////////////////////////////////////////////
                // Set name of generated files (Xml + Log)
                parameter.fileNameGeneratedXml = "TradeNo_" + tradeId;
                //////////////////////////////////////////////////////////////////////////////////////////////////
                // Set TradeNo/TradeID.
                parameter.tradeNo = tradeId;
                //////////////////////////////////////////////////////////////////////////////////////////////////
                // Set delivery date.
                XmlNode deliveryDate = tradeImport.SelectSingleNode("//datestart");
                parameter.deliveryDate = deliveryDate.InnerText.Replace('-','.');
                //////////////////////////////////////////////////////////////////////////////////////////////////
                // Set memo1 as text from memo2.
                XmlNode memo2 = tradeImport.SelectSingleNode("//memo2");
                parameter.memo1 = memo2.InnerText;
                //////////////////////////////////////////////////////////////////////////////////////////////////
                // Set memo2.
                parameter.memo2 = tradeId + "-" + parameter.contract + " " + parameter.price + " Euro " + parameter.quantity + " MW";
                parameter.memo2 = parameter.memo2.Replace('.', ',');
                //////////////////////////////////////////////////////////////////////////////////////////////////
                // Set product. TODO
                //parameter.product = "";
                //////////////////////////////////////////////////////////////////////////////////////////////////

                // Add current parameter set to list.
                parameterList.Add(parameter);
            }
        }
        #endregion

        #region Provide parameter for input XML use Comtrader-Export as reference.
        /// <summary>
        /// Provide parameter for input XML use Comtrader-Export as reference.
        /// </summary>
        /// <param name="parameterList"></param>
        /// <param name="resultTable"></param>
        /// <param name="parameter"></param>
        private void ProvideParameterForInputXmlReferenceComtrader(ref List<ParameterXmlGeneration> parameterList, ResultTable resultTable, ParameterXmlGeneration parameter)
        {
            foreach (DataRow resultRow in resultTable.table.Rows)
            {
                // Define if this a buy or a sell.
                GetBuyOrSell(ref parameter.buySell, resultRow);
                // Get Control area.
                GetControlArea(ref parameter.controlArea, resultRow);               
                // Contract from Comtrader Export '20200622 10:15-20200622 10:30' --> 10Q2
                GetContractRange(ref parameter.contract, resultRow);
                // Qty from Comtrader Export
                parameter.quantity = resultRow["Qty"].ToString();
                // Prc from comtrader Export
                parameter.price = resultRow["Prc"].ToString();
                // Set name of generated files (Xml + Log)
                parameter.fileNameGeneratedXml = "TradeNo_" + resultRow["TradeNo"].ToString();
                // TradeNo/TradeID from Comtrader Export File
                parameter.tradeNo = resultRow["TradeNo"].ToString();
                // Get delivery date.
                GetDeliveryData(ref parameter.deliveryDate, resultRow);
                // TradeDate is set from input parameter data which is identical to date in comtrader export.
                // Text from Comtrader Export File
                parameter.memo1 = resultRow["Text"].ToString();
                // TradeNo/TradeID from Comtrader Export File. 
                parameter.memo2 = resultRow["TradeNo"].ToString() + "-" + parameter.contract + " " + parameter.price + " Euro " + parameter.quantity + " MW";
                // Text from Comtrader Export File
                parameter.product = resultRow["Product"].ToString();

                // Add current parameter set to list.
                parameterList.Add(parameter);
            }
        }
        #endregion

      
        
        #region Get control area.
        /// <summary>
        /// Get control area.
        /// </summary>
        /// <param name="contract"></param>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        private string GetControlArea(ref ControlArea controlArea, DataRow dataRow)
        {
            string retValue = string.Empty;            

            try
            {
                // Convert control area from Comtrader-Export to the one needed for Aligne
                switch (dataRow["TSO"].ToString())
                {
                    case "50HzT":
                        controlArea = ControlArea._50HZ;
                        break;
                    case "AMP":
                        controlArea = ControlArea.AMP;
                        break;
                    case "TNG":
                        controlArea = ControlArea.ENBW;
                        break;
                    case "TTG":
                        controlArea = ControlArea.TENNET;
                        break;
                    default:
                        logger.WriteError("Error in : " + (MethodBase.GetCurrentMethod().Name) + "(): Read wrong control area from Comtrader-Export.");
                        break;
                }                       
                retValue = "OK";
            }
            catch (Exception exception)
            {
                // Exception set info.
                retValue = "NOK - " + exception.Message.ToString();
                // Do the logging.
                logger.WriteError("Exception in : " + (MethodBase.GetCurrentMethod().Name) + "(): " + exception.Message.ToString() + ".");
            }

            return retValue;
        }
        #endregion

        #region Get delivery date.
        /// <summary>
        /// Get delivery date.
        /// </summary>
        /// <param name="deliveryDate"></param>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        private string GetDeliveryData(ref string deliveryDate, DataRow dataRow)
        {
            string retValue = string.Empty;
            string[] split;

            try
            {
                deliveryDate = dataRow["Contract"].ToString();
                split = deliveryDate.Split(' ');
                deliveryDate = split[0];

                deliveryDate = DateTime.ParseExact(deliveryDate, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd.MM.yyyy");

                retValue = "OK";
            }
            catch (Exception exception)
            {
                // Exception set info.
                retValue = "NOK - " + exception.Message.ToString();
                // Do the logging.
                logger.WriteError("Exception in : " + (MethodBase.GetCurrentMethod().Name) + "(): " + exception.Message.ToString() + ".");
            }

            return retValue;
        }
        #endregion

        #region Get if trade is a buy or a sell.
        /// <summary>
        /// Get if trade is a buy or a sell.
        /// </summary>
        /// <param name="buySell"></param>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        private string GetBuyOrSell(ref BuySell buySell, DataRow dataRow)
        {
            string retValue = string.Empty;
            string buySellString = dataRow["B_S"].ToString();

            try
            {
                if (buySellString.Equals("B"))
                {
                    buySell = BuySell.BUY;
                }
                else
                {
                    buySell = BuySell.SELL;
                }

                retValue = "OK";
            }
            catch (Exception exception)
            {
                // Exception set info.
                retValue = "NOK - " + exception.Message.ToString();
                // Do the logging.
                logger.WriteError("Exception in : " + (MethodBase.GetCurrentMethod().Name) + "(): " + exception.Message.ToString() + ".");
            }

            return retValue;
        }
        #endregion

        #region Get contract range from comtrader export.
        /// <summary>
        /// Get contract range from comtrader export.
        /// </summary>
        /// <param name="contract"></param>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        private string GetContractRange(ref string contract, DataRow dataRow)
        {
            string retValue = string.Empty;
            contract = dataRow["Contract"].ToString();
            int hourStart, hourEnd, quarterStart, quarterEnd, quarter = 1;
            
            try
            {
                string[] split = contract.Split(' ', '-', ':');
                if (split.Length == 6)
                {
                    hourStart = Convert.ToInt16(split[1]);
                    hourEnd = Convert.ToInt16(split[4]);
                    quarterStart = Convert.ToInt16(split[2]);
                    quarterEnd = Convert.ToInt16(split[5]);                  

                    if ((quarterStart >= 00) & (quarterStart <= 45) & (quarterEnd >= 00) & (quarterEnd <= 45))
                    {
                        if ((hourStart >= 00) & (hourEnd <= 23)) 
                        {
                            // Determine the quarter needed for Aligne.
                            switch (quarterStart)
                            {
                                case 00:
                                    quarter = 1;
                                    break;
                                case 15:
                                    quarter = 2;
                                    break;
                                case 30:
                                    quarter = 3;
                                    break;
                                case 45:
                                    quarter = 4;
                                    break;
                            }

                            // Determine contract needed for Aligne.
                            contract = hourStart.ToString("D2") + "Q" + quarter.ToString(); 
                            retValue = "OK";
                        }
                        else
                        {
                            retValue = "NOK - Wrong product. Wrong hour.";
                            // Do the logging.
                            logger.WriteError(retValue);
                        }
                    }
                    else
                    {
                        retValue = "NOK - Wrong product. Wrong quarter.(Hour not checked.)";
                        // Do the logging.
                        logger.WriteError(retValue);
                    }
                }
                else
                {
                    retValue = "NOK - Wrong product. Could not be separated in hour and quarter.";
                    // Do the logging.
                    logger.WriteError(retValue);
                }
            }
            catch (Exception exception)
            {
                // Exception set info.
                retValue = "NOK - " + exception.Message.ToString();
                // Do the logging.
                logger.WriteError("Exception in : " + (MethodBase.GetCurrentMethod().Name) + "(): " + exception.Message.ToString() + ".");
            }

            return retValue;
        }
        #endregion

        #region Check if trades are already booked in Aligne.
        /// <summary>
        /// Check if trades are already booked in Aligne.
        /// </summary>
        /// <param name="resultTable"></param>
        /// <param name="vskTradesResult"></param>
        private void RemoveTradesAlreadyBookedToAligne(ref ResultTable vskTradesComtraderExport, DataTable vskTradesAligne)
        {
            string tradeIdToBeBooked = string.Empty;
            string tradeIdBooked = string.Empty;
            DataTable copyVskTradesComtraderExport = vskTradesComtraderExport.table.Clone();
            bool alreadyBooked = false;

            // Table vskTradesComtraderExport trades found in Comtrader Export which should be booked to Aligne if not booked before.
            foreach (DataRow tradeToBeBooked in vskTradesComtraderExport.table.Rows)
            {
                // Use column tradeNo to get TradeID.
                tradeIdToBeBooked = tradeToBeBooked["TradeNo"].ToString();

                foreach (DataRow tradesBooked in vskTradesAligne.Rows)
                {
                    // Use column tradeNo to get TradeID - jus use the trade id 
                    tradeIdBooked = tradesBooked["MEMO2"].ToString();
                    string[] split = tradeIdBooked.Split('-');
                    tradeIdBooked = split[0];
                    alreadyBooked = false;

                    // If tradeIds are equal trade already booked.
                    if (tradeIdToBeBooked.Equals(tradeIdBooked))
                    {
                        // Trade already booked.
                        alreadyBooked = true;
                        break;
                    }
                }

                if(!alreadyBooked)
                {
                    // Trade not booked add to table.
                    var tempRow = copyVskTradesComtraderExport.NewRow();
                    tempRow.ItemArray = tradeToBeBooked.ItemArray;
                    copyVskTradesComtraderExport.Rows.Add(tempRow);
                }
            }

            // Fill original table with table of copied one.
            vskTradesComtraderExport.table.Clear();
            vskTradesComtraderExport.table = copyVskTradesComtraderExport.Copy();
            vskTradesComtraderExport.numberOfRows = copyVskTradesComtraderExport.Rows.Count;
        }
        #endregion
    }
}
