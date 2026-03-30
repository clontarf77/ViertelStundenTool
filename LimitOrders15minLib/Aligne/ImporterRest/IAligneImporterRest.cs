using ViertelStdToolLib.Aligne.Importer.DealStructure;
using ViertelStdToolLib.Aligne.Importer.DealStructure.Rest;

namespace ViertelStdTool.AligneImporter.Rest
{
    public interface IAligneImporterRest
    {
        /// <summary>
        /// Constructor Init object to import files to Aligne via Rest.
        /// </summary>
        /// <param name="server"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        //AligneImporterRest(string server, string user, string password)       

        /// <summary>
        /// ImportDeal to Algne via Rest use Json as input format. 
        /// </summary>                
        /// <param name="tradeClass"></param>       
        /// <param name="encryptionMode"></param> 
        /// <param name="zKey"></param>                    
        /// <returns>String 'success' if OK</returns>   
        string ImportDeal(RootObjectRequestTrade tradeClass, PwEnCrypton encryptionMode, ref string zKey);      
    }
}

