namespace ViertelStdTool.CredentialsHandler
{
    public interface ICredentialsReader
    {
        /// <summary>
        /// Method to read the user credentials.
        /// </summary>    
        CredentialsStructureAligne Credentials { get; }

        /// <summary>
        /// Constructor to read settings.xml to credentials structure. 
        /// </summary>
        //CredentialsReader()
    }
}