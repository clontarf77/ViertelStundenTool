namespace ViertelStdTool.SFTPManager
{
    public interface ISFTP
    {
        /// <summary>
        /// Download file from sFTP server.
        /// </summary>
        /// <param name="remotePath"></param>
        /// <param name="localPath"></param>
        /// <returns></returns>
        string DownloadFile(string remotePath, string localPath);

        /// <summary>
        /// Upload file to sFTP server.
        /// </summary>
        /// <param name="remotePath"></param>
        /// <param name="localPath"></param>
        /// <returns></returns>
        string UploadFile(string localPath, string remotePath);        
               
        /// <summary>
        /// Download latest file from sFTP server which starts with given filter name.
        /// </summary>       
        /// <param name="remoteDirectoryPath"></param>
        /// <param name="localDestination"></param>      
        /// <param name="fileStartsWith"></param>
        /// <param name="ref nameOfDownloadedFile"></param>
        /// <returns></returns>
        string DownloadLatesFileUseFilter(string remoteDirectoryPath, string localDestination, string fileStartsWith, ref string nameOfDownloadedFile);        
    }
}