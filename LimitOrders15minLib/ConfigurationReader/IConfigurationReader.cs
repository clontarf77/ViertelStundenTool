namespace ViertelStdTool.ConfigurationHandler
{
    public interface IConfigurationReader
    {
        /// <summary>
        /// Method to read the configuration.
        /// </summary>      
        ConfigurationStructure Config { get; }

        /// <summary>
        /// Constructor to read app.config settings to configuration structure. 
        /// </summary>
        //ConfigurationReader()
    }
}