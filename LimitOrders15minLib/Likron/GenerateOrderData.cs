using ViertelStdTool.Converter;
using System;
using System.Data;
using ViertelStdTool.Log;
using ViertelStdToolLib.Converter;

namespace ViertelStdTool.Likron
{
    public class GenerateOrderData : IGenerateOrderData
    {
        private const string area = "Area";
        private const string type = "Type";
        private const string buySell = "B/S";
        private const string accnt = "Accnt";
        private const string product = "Product";
        private const string ctrct = "Ctrct";
        private const string qty = "Qty";
        private const string prc = "Prc";
        private const string bg = "BG";
        private const string txt = "Txt";
        private const string pQty = "PQty";
        private const string valRes = "ValRes";
        private const string valDate = "ValDate";
        private const string exeRes = "ExeRes";

        private readonly INLogger logger = new NLogger();

        private readonly IContractValidityConverter validityConverter = new ContractValidityConverter();

        #region Generate  Order Data for Likron - Normal for Control Area RWE.
        /// <summary>
        /// Generate  Order Data for Likron - Normal for Control Area RWE.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>Ok in case of success</returns>
        public string GenerateCsvForLikron(ref OrderDataGenerationData parameter)
        {
            // Generate data which will be loaded to LikronsFTP.
            char seperator = ';';           
            parameter.limitDataTable = new DataTable();
            parameter.nameOfGeneratedFile = string.Empty;
            ICsvDataTableConverter converterCsv = new CsvDataTableConverter();
            IExcelDataTableConverter converterExcel = new ExcelDataTableConverter();
            parameter.sumQuantityIsZero = false;
            string retVal = "NOK";           
            DataTable distinctContractTable = new DataTable();            

            // Convert input CSV to DataTable.
            parameter.loadDataTable = converterCsv.ConvertCsvToDataTable(parameter.pathLoadCSV, seperator);

            // Convert input limit excel to DataTable.
            parameter.limitDataTable = converterExcel.ConvertExcelToDataTable(parameter.pathLimitExcel, parameter.tableName);

            if (parameter.limitDataTable.Rows.Count == 0)
            {
                // Table with limit data is empty.
                retVal = "NOK - Limit Data from Limit-File could not be read.";
                logger.WriteInfo("Limit Data from Limit-File could not be read.");
            }
            else
            {
                // Remove areas from contract.
                foreach (DataRow orderRow in parameter.loadDataTable.Rows)
                {
                    string[] contract = orderRow[ctrct].ToString().Split('_');
                    orderRow[ctrct] = contract[0];

                    string productString = orderRow[product].ToString();
                    productString = productString.Replace("_Amprion", "");
                    productString = productString.Replace("_50Hz", "");
                    productString = productString.Replace("_Tennet", "");
                    productString = productString.Replace("_TransnetBW", "");
                    orderRow[product] = productString;
                }

                // Limit data could be read. Go on with generation.
                // Create DataTable for Likron with tradetimes >= 30 min. Copy Structure and Data.
                parameter.likronDataTable = parameter.loadDataTable.Copy();
                // Create DataTable for Likron with trades leadtime < 30min. Copy Structure no Data.
                parameter.likronDataTableSmaller30Min = parameter.loadDataTable.Clone();

                // Get number and value of contracts in loadDataTable
                distinctContractTable = parameter.likronDataTable.DefaultView.ToTable(true, ctrct, valDate, product, exeRes, accnt, type);                             

                // Add datarow with default values for likron. Table is cleared before adding thus just one row is in table.
                AddDataRowWithDefaultValuesLikron(ref parameter.likronDataTable, distinctContractTable.Rows.Count);

                // Set values taken over from input data.
                SetValuesFromInputForLikron(ref parameter.likronDataTable, parameter.loadDataTable, distinctContractTable, ref parameter.sumQuantityIsZero, parameter.validityEnd);

                // Set limits for quantity.
                SetLimitsForQuantity(ref parameter.likronDataTable, parameter.limitType, parameter.loadLimit);

                // Calculate Price
                CalculatePriceOrderData(ref parameter.likronDataTable, parameter.limitDataTable, parameter.sellLimit, parameter.buyLimit);

                // Separate DataTable Rows (Orders) for leadtime >= 30 min and leadtime < 30 min which is TUD.
                SeparateStandardAndTUD(ref parameter.likronDataTableAll, ref parameter.likronDataTable, ref parameter.likronDataTableSmaller30Min, DateTime.Now);

                // Set file name for result (order data file for likron)   
                parameter.nameOfGeneratedFile = "Deal_Import_Active_Timestamp_";
                parameter.nameOfGeneratedFile += DateTime.Now.ToString("yyyyMMddHHmm");
                parameter.nameOfGeneratedFileSmaller30Min = parameter.nameOfGeneratedFile + "_TUD.csv";
                parameter.nameOfGeneratedFile += ".csv";
                parameter.pathOrderLikron += parameter.nameOfGeneratedFile;
                parameter.pathOrderLikronSmaller30Min += parameter.pathOrderLikron.Replace(".csv", "") + "_TUD.csv";

                // Convert DataTable back to CSV (this is for leadtimes >= 30 min)
                if (parameter.likronDataTable.Rows.Count > 0)
                {
                    // No need to convert table to file if table is empty.
                    converterCsv.ConvertDataTableToCsv(parameter.likronDataTable, parameter.pathOrderLikron, seperator);
                    logger.WriteInfo("Order Thread: CSV for Likron (leadtimes >= 30 min) generated in path: '" + parameter.pathOrderLikron + "'.");                    
                }
                else
                {
                    parameter.pathOrderLikron = string.Empty;
                    logger.WriteInfo("Order Thread: CSV for Likron not generated cause no trades were found with leadtimes >= 30 min.");
                }
                // Convert DataTable back to to CSV (this is for leadtimes < 30min)
                if (parameter.likronDataTableSmaller30Min.Rows.Count > 0)
                {
                    // No need to convert table to file if table is empty.
                    converterCsv.ConvertDataTableToCsv(parameter.likronDataTableSmaller30Min, parameter.pathOrderLikronSmaller30Min, seperator);
                    logger.WriteInfo("Order Thread: CSV for Likron (leadtimes < 30min) generated in path: '" + parameter.pathOrderLikronSmaller30Min + "'.");                    
                }
                else
                {
                    parameter.pathOrderLikronSmaller30Min = string.Empty;
                    logger.WriteInfo("Order Thread: CSV for Likron not generated cause no trades were found with leadtimes < 30 min.");
                }

                retVal = "OK";
            }
            return retVal;
        }
        #endregion

        #region  Generate  Order Data for Likron - TuD - Per Control Area.
        /// <summary>
        ///  Generate  Order Data for Likron - TuD - Per Control Area.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>Ok in case of success</returns>
        public string GenerateCsvForLikronTuD(ref OrderDataGenerationData parameter)
        {
            // Generate data which will be loaded to LikronsFTP.
            char seperator = ';';
            parameter.limitDataTable = new DataTable();
            parameter.nameOfGeneratedFile = string.Empty;
            ICsvDataTableConverter converterCsv = new CsvDataTableConverter();
            IExcelDataTableConverter converterExcel = new ExcelDataTableConverter();
            parameter.sumQuantityIsZero = false;
            string retVal = "NOK";
            DataTable distinctContractTable = new DataTable();

            // Convert input CSV to DataTable.
            parameter.loadDataTable = converterCsv.ConvertCsvToDataTable(parameter.pathLoadCSV, seperator);

            // Convert input limit excel to DataTable.
            parameter.limitDataTable = converterExcel.ConvertExcelToDataTable(parameter.pathLimitExcel, parameter.tableName);

            if (parameter.limitDataTable.Rows.Count == 0)
            {
                // Table with limit data is empty.
                retVal = "NOK - Limit Data from Limit-File could not be read.";
                logger.WriteInfo("Limit Data from Limit-File could not be read.");
            }
            else
            {
                // Remove areas from contract.
                foreach (DataRow orderRow in parameter.loadDataTable.Rows)
                {
                    string[] contract = orderRow[ctrct].ToString().Split('_');
                    orderRow[ctrct] = contract[0];

                    string productString = orderRow[product].ToString();
                    productString = productString.Replace("_Amprion", "");
                    productString = productString.Replace("_50Hz", "");
                    productString = productString.Replace("_Tennet", "");
                    productString = productString.Replace("_TransnetBW", "");
                    orderRow[product] = productString;
                }

                // Linit data could be read. Go on with generation.
                // Create DataTable for Likron. Copy Structure and Data.
                parameter.likronDataTable = parameter.loadDataTable.Copy();

                // Substitute Likron Strategy. Currently value is set manually and not taken from load-file.
                SubstituteLikronStrategyForTud(ref parameter.likronDataTable);

                // Substitute Names of Control Areas for TuD and BG for MVV.
                SubstituteControlAreasBgTxtForTud(ref parameter.likronDataTable);

                // Get number and value of contracts in loadDataTable
                distinctContractTable = parameter.likronDataTable.DefaultView.ToTable(true, ctrct, valDate, product, exeRes, accnt, type);

                // Summarize trades
                SummarizeTrades(ref parameter.likronDataTable, distinctContractTable);

                // Set Prices for TUD
                CalculatePriceOrderData(ref parameter.likronDataTable, parameter.limitDataTable, parameter.sellLimit, parameter.buyLimit);

                // Set Limits
                SetLimitsForQuantity(ref parameter.likronDataTable, parameter.limitType, parameter.loadLimit);

                // Set file name for result (order data file for likron)   
                parameter.nameOfGeneratedFile = "Deal_Import_Active_Timestamp_";
                parameter.nameOfGeneratedFile += DateTime.Now.ToString("yyyyMMddHHmm");
                parameter.nameOfGeneratedFile += ".csv";
                parameter.pathOrderLikron += parameter.nameOfGeneratedFile;

                // Convert DataTable back to CSV
                converterCsv.ConvertDataTableToCsv(parameter.likronDataTable, parameter.pathOrderLikron, seperator);

                retVal = "OK";
                logger.WriteInfo("CSV for Likron generated in path: " + parameter.pathOrderLikron + ".");
            }
            return retVal;
        }
        #endregion

        #region Add datarow with default values for likron. Table is cleared before adding thus just one row is in table. 
        /// <summary>
        /// Add datarow with default values for likron. Table is cleared before adding thus just one row is in table. 
        /// </summary>
        /// <param name="tableLikron"></param>
        /// <param name="numberOfDifferentContracts"></param>
        private void AddDataRowWithDefaultValuesLikron(ref DataTable tableLikron, int numberOfDifferentContracts)
        {
            tableLikron.Clear();

            for (int i = 0; i < numberOfDifferentContracts; i++)
            {
                // Fill table for Likron - These is the template data
                DataRow resultRow = tableLikron.NewRow();
                resultRow[area] = "AMP"; // Set to RWE
                resultRow[type] = ""; // Use from input 
                resultRow[buySell] = ""; // Calculate
                resultRow[accnt] = ""; // Use from input
                resultRow[product] = ""; // Use from input
                resultRow[ctrct] = ""; // Use from input
                resultRow[qty] = ""; // Calculate
                resultRow[prc] = ""; // Calculate
                resultRow[bg] = "11XMVVTRADING--1"; //  Set for MVV
                resultRow[txt] = "VSK"; // Set to VSK
                resultRow[pQty] = ""; // Empty
                resultRow[valRes] = "GFS"; // Input from emsys use GTD - changed to GFS (Likron)
                resultRow[valDate] = ""; // Use from input
                resultRow[exeRes] = ""; // Use from input

                tableLikron.Rows.Add(resultRow);
            }
        }
        #endregion

        #region Set values taken over from input data.
        /// <summary>
        /// Set values taken over from input data.
        /// </summary>
        /// <param name="tableLikron"></param>
        /// <param name="tableInput"></param>
        /// <param name="distinctContractTable"></param>
        /// <param name="sumQuantityIsZero"></param>
        /// <param name="validityEnd"></param>
        private void SetValuesFromInputForLikron(ref DataTable tableLikron, DataTable tableInput, DataTable distinctContractTable, ref bool sumQuantityIsZero, ValidityEnd validityEnd)
        {
            decimal quantity = 0;
            decimal quantitySum = 0;
            bool sumQuantityIsNotZeroForContract = false;
            // This is set to true if for all contracts in a load file the sum is 0.
            // So just init once before loop.
            sumQuantityIsZero = false;

            for (int i = 0; i < distinctContractTable.Rows.Count; i++)
            {
                // Fill data from input load table.
                tableLikron.Rows[i][ctrct] = distinctContractTable.Rows[i][ctrct];

                switch (validityEnd)
                {
                    case ValidityEnd.empty:
                        // Delete validity end - just set empty string. 
                        tableLikron.Rows[i][valDate] = string.Empty;
                        break;
                    case ValidityEnd.source:
                        // Take validity end from load-file from emsys.
                        tableLikron.Rows[i][valDate] = distinctContractTable.Rows[i][valDate];
                        break;
                }

                tableLikron.Rows[i][product] = distinctContractTable.Rows[i][product];
                //tableLikron.Rows[0][valRes] = tableInput.Rows[0][valRes]; // Not taken from Load-File changed in Code. This is the likron strategy which was changed manually.
                tableLikron.Rows[i][exeRes] = distinctContractTable.Rows[i][exeRes];
                tableLikron.Rows[i][accnt] = distinctContractTable.Rows[i][accnt];
                tableLikron.Rows[i][type] = distinctContractTable.Rows[i][type];
                quantity = 0;
                quantitySum = 0;

                // Sum up quantity and set buy or sell.
                foreach (DataRow row in tableInput.Rows)
                {
                    // Just calculate quantity for current contract.
                    if (row[ctrct].Equals(tableLikron.Rows[i][ctrct]))
                    {
                        // Read quantity from load (row).
                        quantity = Convert.ToDecimal(row[qty]);

                        if (row[buySell].ToString().Equals("B"))
                        {
                            // This is a buy add quantity of current load.
                            quantitySum += quantity;
                        }
                        else
                        {
                            // This is a sell subtract quantity of current load.
                            quantitySum -= quantity;
                        }
                    }
                }

                // Set buy/sell in resulting order for Likron.
                if (quantitySum > 0)
                {
                    // Positive sum this is a buy.
                    tableLikron.Rows[i][buySell] = "B";

                }
                else
                {
                    // Negative sum this is a sell.
                    tableLikron.Rows[i][buySell] = "S";

                    // Sum is always positive. S or B is used to indicate buy or sell.
                    quantitySum = Math.Abs(quantitySum);
                }

                // In this case buy amount is equal to sell amount.
                if (quantitySum == 0)
                {
                    // Set sum in resulting order for Likron.
                    tableLikron.Rows[i][qty] = "0";
                }
                else
                {
                    sumQuantityIsNotZeroForContract = true;
                    // Set sum in resulting order for Likron.
                    tableLikron.Rows[i][qty] = quantitySum.ToString();
                }

                if (!sumQuantityIsNotZeroForContract)
                {
                    // Sum is zero over all contracts no order for likron needed.
                    // Order file for likron is biult with quantity 0. S is set foe order file with empty amount.
                    sumQuantityIsZero = true;
                }
            }
        }
        #endregion

        #region Set limits for quantity.
        /// <summary>
        /// Set limits for quantity.
        /// </summary>
        /// <param name="tableLikron"></param>
        /// <param name="limitType"></param>
        /// <param name="limit"></param>
        public void SetLimitsForQuantity(ref DataTable tableLikron, LimitType limitType, string limit)
        {
            decimal quantity;
            decimal limitDecimal = Convert.ToDecimal(limit);

            foreach (DataRow rowLoad in tableLikron.Rows)
            {
                quantity = Convert.ToDecimal(rowLoad[qty]);

                switch (limitType)
                {
                    case LimitType.NoLimit:
                        // No limits are set do nothing.
                        break;
                    case LimitType.MWh:
                        // Calculate new quantity using limit in MWh.
                        if (limitDecimal < quantity)
                        {
                            quantity = limitDecimal;
                            rowLoad[qty] = quantity.ToString();
                        }
                        break;
                    case LimitType.Percent:
                        if ((limitDecimal > 0) && (limitDecimal <= 100))
                        {
                            // Calculate new quantity use perecentage.
                            quantity = quantity * (limitDecimal / 100);
                            rowLoad[qty] = quantity.ToString();
                        }
                        break;
                    default:
                        // Should not happen
                        break;
                }
            }
        }
        #endregion

        #region Set prices taken over input data.
        /// <summary>
        /// Set prices taken over input data.
        /// Set limit with given quarter for each load.
        /// </summary>
        /// <param name="tableLikron"></param>
        /// <param name="tableLimit"></param>
        private void CalculatePriceOrderData(ref DataTable tableLikron, DataTable tableLimit, string sellLimit, string buyLimit)
        {
            string limit = string.Empty;
            string quarter = string.Empty;
            string buySellTemp = string.Empty;

            // Loop ober all load data.
            // For Standard just one row for TuD more.
            foreach (DataRow rowLoad in tableLikron.Rows)
            {
                //Read quarter and Buy Sell from likron table.
                quarter = rowLoad[ctrct].ToString();
                buySellTemp = rowLoad[buySell].ToString();

                if (quarter.StartsWith("T"))
                {
                    // If string starts with T (means Contract will start next day) - char 'T' will be removed. 
                    quarter = quarter.Remove(0, 1);
                }

                //Read limits for given quarter for Buy and Sell from tableLimit.
                //Write that limit as price (depending on Buy/Sell) to likron table.
                foreach (DataRow rowLimit in tableLimit.Rows)
                {
                    if (quarter.Equals(rowLimit[0].ToString()))
                    {
                        // Current row has same quarter as set in table for likron.
                        if (buySellTemp.Equals("B"))
                        {
                            if ((null == buyLimit) || buyLimit.Equals("0"))
                            {
                                // Read 'von HA an DV (Buy)' if B is set in likron table (positive).
                                rowLoad[prc] = rowLimit[3].ToString();
                            }
                            else
                            {
                                // Set limit from GUI 
                                rowLoad[prc] = buyLimit;
                            }
                        }
                        else
                        {
                            if ((null == sellLimit) || sellLimit.Equals("0"))
                            {
                                // Read 'von HA an DV (Sell)' if S is set in likron table (negative).
                                rowLoad[prc] = rowLimit[2].ToString();
                            }
                            else
                            {
                                // Set limit from GUI 
                                rowLoad[prc] = sellLimit;
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Substitute Likron Strategy for TuD.
        /// <summary>
        /// Substitute Likron Strategy for TuD.
        /// </summary>
        /// <param name="tableLikron"></param>
        private void SubstituteLikronStrategyForTud(ref DataTable tableLikron)
        {
            string controlArea = string.Empty;

            foreach (DataRow row in tableLikron.Rows)
            {
                //Substitute BG
                row[valRes] = "GFS"; // Input from emsys use GTD - changed to GFS (Likron)
            }
        }
        #endregion

        #region Substitute Names of Control Areas for TuD, Text and BG for MVV.
        /// <summary>
        /// Substitute Names of Control Areas for TuD, Text and BG for MVV.
        /// </summary>
        /// <param name="tableLikron"></param>
        private void SubstituteControlAreasBgTxtForTud(ref DataTable tableLikron)
        {
            string controlArea = string.Empty;

            foreach (DataRow row in tableLikron.Rows)
            {
                //Substitute BG
                row[bg] = "11XMVVTRADING--1"; //  Set for MVV      
                //Sunstitute Text                              
                row[txt] = "VSK";

                // Substitute Control Areas
                controlArea = row[area].ToString();

                //Volue Release R55
                switch (controlArea)
                {
                    case "RWE":
                        row[area] = "AMP";
                        break;
                    case "ENBW":
                        row[area] = "TNG";
                        break;
                    case "VE":
                        row[area] = "50HzT";
                        break;
                    case "EON":
                        row[area] = "TTG";
                        break;                  
                    default:
                        // Should never happen.
                        break;
                }
            }
        }
        #endregion

        #region Summarize trades.
        /// <summary>
        /// Summarize trades
        /// </summary>
        /// <param name="tableLikron"></param>
        /// <param name="distinctContractTable"></param>
        private void SummarizeTrades(ref DataTable tableLikron, DataTable distinctContractTable)
        {
            string contract = string.Empty;

            // Make temp. copy and truncate dataTable.
            DataTable tablelikronTemp = tableLikron.Copy();
            tablelikronTemp.Clear();

            for (int i = 0; i < distinctContractTable.Rows.Count; i++)
            {
                // One contract for each pass of the loop.
                contract = distinctContractTable.Rows[i][ctrct].ToString();

                DataRow[] ordersRWE = tableLikron.Select(area + " = 'AMP' AND " + ctrct + " = '" + contract + "'");
                DataRow[] ordersEON = tableLikron.Select(area + " = 'TTG' AND " + ctrct + " = '" + contract + "'");
                DataRow[] ordersENBW = tableLikron.Select(area + " = 'TNG' AND " + ctrct + " = '" + contract + "'");
                DataRow[] ordersVE = tableLikron.Select(area + " = '50HzT' AND " + ctrct + " = '" + contract + "'");

                // Copy first found row to sum. Quantity and price of that row will be adapted later on.
                // Rest of column data stays as it is.
                tablelikronTemp.ImportRow(ordersRWE[0]);
                tablelikronTemp.ImportRow(ordersEON[0]);
                tablelikronTemp.ImportRow(ordersENBW[0]);
                tablelikronTemp.ImportRow(ordersVE[0]);

                // Remove quantity and buy/sell from temp. likron table. // TODO just for rows with used contract.
                foreach (DataRow row in tablelikronTemp.Rows)
                {
                    if (row[ctrct].Equals(contract))
                    {
                        row[qty] = 0;
                        row[buySell] = string.Empty;
                    }
                }

                // Extract sum rows for temp. likron table. Just use first row of each area.            
                DataRow sumOrderRWE = tablelikronTemp.Select(area + " = 'AMP' AND " + ctrct + " = '" + contract + "'")[0];
                DataRow sumOrderEON = tablelikronTemp.Select(area + " = 'TTG' AND " + ctrct + " = '" + contract + "'")[0];
                DataRow sumOrderENBW = tablelikronTemp.Select(area + " = 'TNG' AND " + ctrct + " = '" + contract + "'")[0];
                DataRow sumOrderVE = tablelikronTemp.Select(area + " = '50HzT' AND " + ctrct + " = '" + contract + "'")[0];

                // There is more than one order to RWE.
                CaluclateQuantity(ordersRWE, ref sumOrderRWE);

                // There is more than one order to EON.
                CaluclateQuantity(ordersEON, ref sumOrderEON);

                // There is more than one order to ENBW.
                CaluclateQuantity(ordersENBW, ref sumOrderENBW);

                // There is more than one order to VE.
                CaluclateQuantity(ordersVE, ref sumOrderVE);
            }

            // Copy result data from temp. datatable to real one.
            tableLikron = tablelikronTemp;
        }
        #endregion

        #region Method takes all rows from same Area and caluclates one resulting row for specific area.
        /// <summary>
        /// Method takes all rows from same Area and caluclates one resulting row for specific area.
        /// </summary>
        /// <param name="ordersSameArea"></param>
        /// <param name="sumOrderArea"></param>
        private void CaluclateQuantity(DataRow[] ordersSameArea, ref DataRow sumOrderArea)
        {
            string buySellTemp = string.Empty;
            decimal quantitySumTemp = 0;

            // There is more than one order to RWE.
            if (ordersSameArea.Length > 0)
            {
                // Set default value.
                quantitySumTemp = 0;

                // Loop over all RWE orders.               
                foreach (DataRow orderArea in ordersSameArea)
                {
                    buySellTemp = orderArea[buySell].ToString();

                    // Current row has same quarter as set in table for likron.
                    if (buySellTemp.Equals("B"))
                    {
                        //If B is set add quantity to sum.
                        quantitySumTemp += Convert.ToDecimal(orderArea[qty]);
                    }
                    else
                    {
                        //If B is set subtract quantity from sum.
                        quantitySumTemp -= Convert.ToDecimal(orderArea[qty]);
                    }
                }

                if (quantitySumTemp >= 0)
                {
                    // We have a buy.
                    sumOrderArea[buySell] = "B";
                    sumOrderArea[qty] = quantitySumTemp.ToString();
                }
                else
                {
                    // We have a sell.
                    sumOrderArea[buySell] = "S";
                    sumOrderArea[qty] = (quantitySumTemp * (-1)).ToString();
                }
            }
        }
        #endregion

        #region // Separate DataTable Rows (Orders) for leadtime >= 30 min and leadtime < 30 min which is TUD.
        /// <summary>
        /// Separate DataTable Rows (Orders) for leadtime >= 30 min and leadtime < 30 min which is TUD.
        /// </summary>
        /// <param name="likronDataTable"></param>
        /// <param name="likronDataTableSmaller30Min"></param>
        /// <param name="timeNow"></param>
        private void SeparateStandardAndTUD(ref DataTable likronDataTableAll,  ref DataTable likronDataTable, ref DataTable likronDataTableSmaller30Min, DateTime timeNow)
        {
            DateTime tudStartTimeDateTime;
            likronDataTableAll = likronDataTable.Copy();
            likronDataTable.Clear();

            foreach (DataRow row in likronDataTableAll.Rows)
            {
                string deliveryStartTime = string.Empty;               
                // Get contract of order.
                string contract = row[ctrct].ToString();
                // Caluclate delivery start time.
                validityConverter.ConvertContractToValidityStartTime(contract, ref deliveryStartTime);

                //If contract starts with T it will start on the next day.
                if (contract.StartsWith("T"))
                {
                    // Calculate start of TUD for same day.
                    tudStartTimeDateTime = Convert.ToDateTime(deliveryStartTime).AddDays(1).AddMinutes(-30);
                }
                else
                {
                    // Calculate start of TUD for same day.
                    tudStartTimeDateTime = Convert.ToDateTime(deliveryStartTime).AddMinutes(-30);
                }
               
                // Separate Orders for leadtime >= 30 min and leadtime < 30 min.
                if (DateTime.Compare(tudStartTimeDateTime, timeNow) < 0)
                {
                    // leadtime is < 30 min 
                    // These are the TUD orders, add them datatable to likronDataTableSmaller30Min.                    
                    likronDataTableSmaller30Min.Rows.Add(row.ItemArray);
                }
                else
                {
                    // leadtime is >= 30 min 
                    // These are standard orders, add them datatable to likronDataTable.       
                    likronDataTable.Rows.Add(row.ItemArray);
                }                
            }
        }
        #endregion
    }
}


