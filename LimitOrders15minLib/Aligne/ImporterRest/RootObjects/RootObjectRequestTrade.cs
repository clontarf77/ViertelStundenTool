using ViertelStdToolLib.Aligne.Importer.DealStructure.Rest;

namespace ViertelStdTool.AligneImporter.Rest
{
    public class RootObjectRequestTrade
    {
        public ImporterRequest[] importerRequest  = new ImporterRequest[1];
                
        public RootObjectRequestTrade(ShapeType shapetype)
        {
            importerRequest[0].templateId = "15532";
            importerRequest[0].legacyTemplateId = "-28";
            importerRequest[0].audit = "NEW";
            // Shape Details
            importerRequest[0].fields.shapeDetail = new ShapeData[1];
           
            if (shapetype == ShapeType._15min)
            {
                importerRequest[0].fields.shapeDetail[0].loadData = new string[96]; // 96 quarter hours for one day
                importerRequest[0].fields.shapeDetail[0].priceData = new string[96]; // 96 quarter hours for one day
            }
            else if(shapetype == ShapeType._30min)
            {
                importerRequest[0].fields.shapeDetail[0].loadData = new string[48]; // 48 half hours for one day
                importerRequest[0].fields.shapeDetail[0].priceData = new string[48]; // 48 half hours for one day
            }
            else if(shapetype == ShapeType.hour)
            {
                importerRequest[0].fields.shapeDetail[0].loadData = new string[24]; // 24 hours for one day
                importerRequest[0].fields.shapeDetail[0].priceData = new string[24]; // 24 hours for one day
            }

            // Module POWERNOMS
            importerRequest[0].fields.module = new ModulePowerNoms[1];
            importerRequest[0].fields.module[0].name = "POWERNOMS";

            for (int i = 0; i < importerRequest[0].fields.shapeDetail[0].loadData.Length; i++)
            {
                importerRequest[0].fields.shapeDetail[0].loadData[i] = "0";
            }

            for (int i = 0; i < importerRequest[0].fields.shapeDetail[0].priceData.Length; i++)
            {
                importerRequest[0].fields.shapeDetail[0].priceData[i] = "0";
            }

            // Set fee data
            importerRequest[0].fields.tradeFees = new TradeFeeData[1];
            importerRequest[0].fields.tradeFees[0].tpowfeeStd = "Yes";
            importerRequest[0].fields.tradeFees[0].tpowfeeName = "TRANSACTION";
            importerRequest[0].fields.tradeFees[0].tpowfeeType = "INTERNAL";
            importerRequest[0].fields.tradeFees[0].tpowfeeAmount = "0.1275";
            importerRequest[0].fields.tradeFees[0].tpowfeeCcy = "EUR";
            importerRequest[0].fields.tradeFees[0].tpowfeeDesc = "Transaction fee";            
            importerRequest[0].fields.tradeFees[0].tpowfeeUnit = "X";
            importerRequest[0].fields.tradeFees[0].tpowfeeIdxFlag = "N";
            importerRequest[0].fields.tradeFees[0].tpowfeeIdxMult = "1";
            importerRequest[0].fields.tradeFees[0].tpowfeePeriod = "H";

            // Fields which are different to EXE version, which are set here automatically
            // TODO must be set, but has no effect...als Day and Date does not work
            importerRequest[0].fields.blocks = new BlocksData[1];
            importerRequest[0].fields.blocks[0].tpow2Days = "SMTWtFsH";
            //importerRequest[0].fields.blocks[0].tpow2Dates = "10-01-2022";
            //importerRequest[0].fields.blocks[0].tpow2Datee = "10-01-2022";
            //importerRequest[0].fields.blocks[0].tpow2Times = "900";
            //importerRequest[0].fields.blocks[0].tpow2Timee = "1000";

            // Empty compared to XML/CSV but is it really needed.
            // Fixing2                           // TPOW_FIX2CHE
            // CptyCcy should be EUR, is empty   // TRADE_CCCY
            // BrkCcy should be EUR, is empty    // TRADE_BCCY
            // SalesCcy should be USD, is empty  // TRADE_SCCY
        }

        #region Setter and mapping to JSON fields
        public string BuySell
        {
            set => importerRequest[0].fields.tpowBs = value;
        }

        public string TpowPrice
        {
            set => importerRequest[0].fields.tpowPrice = value;
        }

        public string TpowAmount
        {
            set => importerRequest[0].fields.tpowAmount = value;
        }       

        public string Memo1
        {
            set => importerRequest[0].fields.tradeMemo = value;
        }

        public string Memo2
        {
            set => importerRequest[0].fields.tradeMemo2 = value;
        }

        public string TpowUnit2
        {
            set => importerRequest[0].fields.tpowUnit2 = value;
        }

        public string Currency
        {
            set => importerRequest[0].fields.tpowCcy = value;
        }

        public string Cpty
        {
            set => importerRequest[0].fields.tradeCpty = value;
        }

        public string Unit
        {
            set => importerRequest[0].fields.tpowUnit = value;
        }

        public string Comp2
        {
            set => importerRequest[0].fields.tpowIdxloc = value;
        }

        public string Mkt2
        {
            set => importerRequest[0].fields.tpowIdxmkt = value;
        }

        public string Book
        {
            set => importerRequest[0].fields.tradeBook = value;
        }

        public string Comp1
        {
            set => importerRequest[0].fields.tpowLoc = value;
        }

        public string Mkt1
        {
            set => importerRequest[0].fields.tpowMkt = value;
        }

        public string Contract
        {
            set => importerRequest[0].fields.tradeManum = value;
        }

        public string Hflag
        {
            set => importerRequest[0].fields.hflag = value;
        }

        public string Tgroup1
        {
            set => importerRequest[0].fields.tradeGroup = value;
        }

        public string Tgroup2
        {
            set => importerRequest[0].fields.tradeSgroup = value;
        }

        public string Tgroup3
        {
            set => importerRequest[0].fields.tradeGroup3 = value;
        }

        public string Tgroup4
        {
            set => importerRequest[0].fields.tradeGroup4 = value;
        }

        public string Util1
        {
            set => importerRequest[0].fields.tpowUtil1 = value;
        }

        public string Util2
        {
            set => importerRequest[0].fields.tpowUtil2 = value;
        }

        public string Util3
        {
            set => importerRequest[0].fields.tpowUtil3 = value;
        }

        public string Util4
        {
            set => importerRequest[0].fields.tpowUtil4 = value;
        }

        public string Util5
        {
            set => importerRequest[0].fields.tpowUtil5 = value;
        }

        public string Status
        {
            set => importerRequest[0].fields.tradeStatus = value;
        }

        public string LEBALGROUP
        {
            set => importerRequest[0].fields.module[0].LEBALGRP = value;
        }

        public string CPBALGROUP
        {
            set => importerRequest[0].fields.module[0].CPBALGRP = value;
        }

        public void SetShapeData(string price, string amount, uint pos)
        {
            ////////////////////////////////////////////////////////////////
            // Example for 15 min shapes                                  //
            // 8:45 --> 8 * 4 + 3 = 35 --> start at 0 --> 34              //
            ////////////////////////////////////////////////////////////////
            // Example for 30 min shapes                                  //
            // 8:30 --> 8 * 2 + 1 = 17 --> start at 0 --> 16              //
            ////////////////////////////////////////////////////////////////
            // Example for hour shapes                                  //
            // 8:00 --> 8 * 1 = 8 --> start at 0 --> 7                    //
            ////////////////////////////////////////////////////////////////
            importerRequest[0].fields.shapeDetail[0].loadData[pos] = amount;
            importerRequest[0].fields.shapeDetail[0].priceData[pos] = price;
        }
        #endregion
    }

    public struct ImporterRequest
    {
        public string templateId;
        public string legacyTemplateId;
        public string audit;
        public Fields fields;        
    }

    public struct Fields
    {
        public string tpowBs;
        public string tpowPrice;
        public string tpowAmount;       
        public string tradeMemo;
        public string tradeMemo2;
        public string tpowUnit2;
        public string tpowCcy;
        public string tradeCpty;
        public string tpowUnit;
        public string tpowIdxloc;
        public string tpowIdxmkt;
        public string tradeBook;
        public string tpowLoc;
        public string tpowMkt;
        public string tradeManum;
        public string hflag;
        public string tradeGroup;
        public string tradeSgroup;
        public string tradeGroup3;
        public string tradeGroup4;
        public string tpowUtil1;
        public string tpowUtil2;
        public string tpowUtil3;
        public string tpowUtil4;
        public string tpowUtil5;
        public string tradeStatus;
        public ShapeData[] shapeDetail;
        public ModulePowerNoms[] module;
        public TradeFeeData[] tradeFees;
        public BlocksData[] blocks;
    }

    public struct ShapeData
    {
        public string[] loadData;
        public string[] priceData;        
    }

    public struct ModulePowerNoms
    {
        public string name;
        public string LEBALGRP;
        public string CPBALGRP;       
    }

    public struct TradeFeeData
    {
        public string tpowfeeStd;
        public string tpowfeeName;
        public string tpowfeeType;
        public string tpowfeeAmount;
        public string tpowfeeCcy;
        public string tpowfeeIdxMult;
        public string tpowfeeDesc;
        public string tpowfeeUnit;
        public string tpowfeeIdxFlag;
        public string tpowfeePeriod; 
    }

    public struct BlocksData
    {
        public string tpow2Days;
        //public string tpow2Dates;
        //public string tpow2Datee;
        //public string tpow2Times;
        //public string tpow2Timee;       
    }
}

