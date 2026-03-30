using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using ViertelStdTool.Log;
using ViertelStdToolLib.Aligne.Importer.DealStructure;

namespace ViertelStdToolLib.Aligne.Importer.Generate.Xml
{
    public class GenerateImporterXml : IGenerateImporterXml
    {
        // Logger
        private readonly INLogger logger = new NLogger();
         
        #region Generate template XML for VSK for several trades.    
        /// <summary>
        /// Generate template for VSK for several trades.
        /// </summary>
        /// <param name="parameterList"></param>                
        public void GenerateXmlForVsk(List<ParameterXmlGeneration> parameterList)
        {
            foreach(ParameterXmlGeneration item in parameterList)
            {
                GenerateXmlForVskOneTrade(item);
            }
        }
        #endregion

        #region Generate template for VSK for one trade.
        /// <summary>
        /// Generate template for VSK for one trade.
        /// </summary>
        /// <param name="parameter"></param>            
        private void GenerateXmlForVskOneTrade(ParameterXmlGeneration parameter)
        {
            // Path to template
            string pathTemplate = string.Empty;

            // XML Document
            XmlDocument xmlDoc = new XmlDocument();

            // XML Node list
            XmlNodeList tradeImportNodeList;

            // XML Nodes
            XmlNode importFile;
            XmlNode tradeImport;

            // Read path of template.
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(GenerateImporterXml)).CodeBase);
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
                        
            pathTemplate = pathExecutable + @"\Aligne\Importer\Uploader\TradeTemplate_VSK.xml";
            
            // Read XML to file
            xmlDoc.Load(pathTemplate);

            // Get node importFile.
            importFile = xmlDoc.SelectSingleNode("//importFile");

            // Set date in node importfile attribute sysdate.
            importFile.Attributes["sysdate"].Value = parameter.tradeDate.Replace('.', '/');

            // Get node tradeImport. 
            tradeImport = xmlDoc.DocumentElement.FirstChild;
            tradeImportNodeList = xmlDoc.SelectNodes("//tradeImport");

            // Set Buy/Sell
            UpdateNodeBuySell(ref tradeImport, parameter.buySell);

            // Set Control area
            UpdateNodeControlArea(ref tradeImport, parameter.controlArea);

            // Set Trader
            UpdateNodeTrader(ref tradeImport, parameter.trader);

            // Set memo.
            UpdateNodeMemo(ref tradeImport, parameter.memo1, parameter.memo2);

            // Set date
            UpdateNodeDate(ref tradeImport, parameter.deliveryDate);

            // Set quantity and price according contract     
            UpdateNodeQuantityAndPrice(ref tradeImport, parameter.contract, parameter.quantity, parameter.price, parameter.buySell);

            // Write XML to given path with given file name.
            xmlDoc.Save(parameter.path + @"\" + parameter.fileNameGeneratedXml + ".xml");
        }
        #endregion

        #region Update node BuySell importer XML.
        private void UpdateNodeBuySell(ref XmlNode tradeImport, BuySell buysell)
        {
            XmlNode nodeBuySell = tradeImport.SelectSingleNode("//buysell");

            switch (buysell)
            {
                case BuySell.SELL:
                    nodeBuySell.InnerXml = "B"; // Gegendeal (Customer Trade)
                    break;
                case BuySell.BUY:
                    nodeBuySell.InnerXml = "S"; // Gegendeal (Customer Trade)                    
                    break;
            }
        }
        #endregion

        #region Update node ControlArea in importer XML.
        private void UpdateNodeControlArea(ref XmlNode tradeImport, ControlArea controlArea)
        {
            XmlNode nodeLebalGrp;
            XmlNode nodeCpbalGrp;

            XmlNode mkt1 = tradeImport.SelectSingleNode("//mkt1");
            XmlNode comp1 = tradeImport.SelectSingleNode("//comp1");

            XmlNodeList moduleList = tradeImport.SelectNodes("//module");
            foreach (XmlNode node in moduleList)
            {
                if (node.Attributes["name"].Value.Equals("POWERNOMS"))
                {
                    nodeLebalGrp = tradeImport.SelectSingleNode("//lebalgrp");
                    nodeCpbalGrp = tradeImport.SelectSingleNode("//cpbalgrp");

                    if (controlArea != ControlArea._50HZ)
                    {
                        nodeLebalGrp.InnerXml = controlArea.ToString() + "-MVVTRD";
                        nodeCpbalGrp.InnerXml = controlArea.ToString() + "-11XMVV---------5";

                        mkt1.InnerXml = controlArea.ToString();
                        comp1.InnerXml = controlArea.ToString();
                    }
                    else
                    {
                        nodeLebalGrp.InnerXml = "50HZ-MVVTRD";
                        nodeCpbalGrp.InnerXml = "50HZ-11XMVV---------5";

                        mkt1.InnerXml = "50HZ";
                        comp1.InnerXml = "50HZ";
                    }

                    break;
                }
            }
        }
        #endregion
        
        #region Update node Trader in importer XML.
        private void UpdateNodeTrader(ref XmlNode tradeImport, string trader)
        {
            XmlNode nodeTrader = tradeImport.SelectSingleNode("//trader");
            nodeTrader.InnerXml = trader;
        }
        #endregion

        #region Update node Memo in importer XML.
        private void UpdateNodeMemo(ref XmlNode tradeImport, string memo1, string memo2)
        {
            XmlNode nodeMemo1 = tradeImport.SelectSingleNode("//memo1");
            nodeMemo1.InnerXml = memo1;
            XmlNode nodeMemo2 = tradeImport.SelectSingleNode("//memo2");
            nodeMemo2.InnerXml = memo2;
        }
        #endregion

        #region Update node Date in importer XML.
        private void UpdateNodeDate(ref XmlNode tradeImport, string date)
        {
            XmlNode nodeDateStart = tradeImport.SelectSingleNode("//datestart");
            XmlNode nodeDateEnd = tradeImport.SelectSingleNode("//dateend");

            XmlNode nodeDateS = tradeImport.SelectSingleNode("//shapeDetail/dates");
            XmlNode nodeDateE = tradeImport.SelectSingleNode("//shapeDetail/datee");

            nodeDateStart.InnerXml = date;
            nodeDateEnd.InnerXml = date;

            nodeDateS.InnerXml = date.Replace('.', '/');
            nodeDateE.InnerXml = date.Replace('.', '/');
        }
        #endregion

        #region Update node quantity and price in importer XML.
        private void UpdateNodeQuantityAndPrice(ref XmlNode tradeImport, string contract, string quantity, string price, BuySell buysell)
        {
            // Just for init, timeNow never needed.
            DateTime contractDeliveryStartTime = DateTime.Now;
            string timeStart = string.Empty;
            string hourString = string.Empty;
            int hour = 0;
            int quarter = 0;

            // Constant needed to calculate price with fee.
            double priceWithFee = 0;
            const double fee = 0.1275;

            XmlNode amountNode;
            XmlNode priceNode;

            XmlNodeList shapeDetailList;

            string[] split = contract.Split('Q', 'q', '_');

            if ((split.Length >= 2) && (split.Length <= 3))
            {
                // Hold in temp. string variable.
                hourString = split[0];

                if (hourString.StartsWith("T"))
                {
                    // If string starts with T (means Contract will start next day) - char 'T' will be removed. 
                    hourString = hourString.Remove(0, 1);
                }

                hour = Convert.ToInt16(hourString);
                quarter = Convert.ToInt16(split[1]);
                if ((quarter >= 1) & (quarter <= 4))
                {
                    if ((hour >= 0) & (hour <= 23))
                    {
                        // Determine the Tud start time.  
                        switch (quarter)
                        {
                            case 1:
                                contractDeliveryStartTime = Convert.ToDateTime(hour + ":00");
                                break;
                            case 2:
                                contractDeliveryStartTime = Convert.ToDateTime(hour + ":15");
                                break;
                            case 3:
                                contractDeliveryStartTime = Convert.ToDateTime(hour + ":30");
                                break;
                            case 4:
                                contractDeliveryStartTime = Convert.ToDateTime(hour + ":45");
                                break;
                        }

                        // This is the start time of the contract as used in xml node.
                        timeStart = contractDeliveryStartTime.ToString("HHmm");

                        // Calculate price using fees.
                        switch (buysell)
                        {
                            //Sell - price + fee (0.1275)
                            case BuySell.SELL:
                                priceWithFee = Convert.ToDouble(price.Replace('.', ',')) - fee;
                                break;
                            //Buy - price - fee (0.1275)
                            case BuySell.BUY:
                                priceWithFee = Convert.ToDouble(price.Replace('.', ',')) + fee;
                                break;
                        }

                        shapeDetailList = tradeImport.SelectNodes("//shapeDetail/priceDetail");
                        foreach (XmlNode node in shapeDetailList)
                        {
                            // Set price for each contract.
                            priceNode = node.SelectSingleNode(".//price");
                            priceNode.InnerXml = priceWithFee.ToString().Replace(',', '.');

                            // Set amount just in correct contract.
                            if (node.SelectSingleNode(".//times").InnerXml.Equals(timeStart))
                            {
                                amountNode = node.SelectSingleNode(".//amount");
                                amountNode.InnerXml = quantity.Replace(',', '.');
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}


