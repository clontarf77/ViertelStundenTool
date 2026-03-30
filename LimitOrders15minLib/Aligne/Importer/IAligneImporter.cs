using ViertelStdToolLib.Aligne.Importer.DealStructure;

namespace ViertelStdTool.AligneImporter
{
    public interface IAligneImporter
    {
        /// <summary>
        /// Constructor Init object to import files to Aligne.
        /// </summary>
        /// <param name="server"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        //AligneImporter(string server, string user, string password)       

        /// <summary>
        /// ImportDeal to Algne use CSV as input format. 
        /// </summary>                
        /// <param name="fileName"></param> 
        /// <param name="path"></param>                
        /// <param name="importFileType"></param>
        /// <param name="encryptionMode"></param> 
        /// <param name="zKey"></param> 
        /// <param name="importerSecurityTurnedOn">We need different arguments if importer security is turned on.</param> 
        /// <returns>String 'success' if OK</returns>   
        string ImportDeal(string fileName, string path, ImportFileType importFileType, PwEnCrypton encryptionModeref, ref string zKey, bool importerSecurityTurnedOn = false);      
    }
}

