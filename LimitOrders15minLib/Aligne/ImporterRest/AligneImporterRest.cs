using Newtonsoft.Json;
using PasswordEnDeCrypter;
using RestSharp;
using System;
using System.Reflection;
using ViertelStdTool.Log;
using ViertelStdToolLib.Aligne.Importer.DealStructure;

namespace ViertelStdTool.AligneImporter.Rest
{
    public class AligneImporterRest : IAligneImporterRest
    {
        // Rest client for that class. Just use one to share cookie container.
        private RestClient restClient;
        private INLogger logger = new NLogger();
        private readonly string setUser = string.Empty;
        private readonly string setPassword = string.Empty;
        private readonly string setServer = string.Empty;

        #region Constructor - Init object to import files to Aligne via Rest.
        /// <summary>
        /// Init object to import files to Aligne via Rest.
        /// </summary>
        /// <param name="server"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        public AligneImporterRest(string server, string user, string password)
        {
            setUser = user;
            setPassword = password;
            setServer = server;            
        }
        #endregion

        #region Import Deal to Aligne use CSV/YML as import format.
        /// <summary>
        /// ImportDeal to Algne via Rest use Json as input format. 
        /// </summary>                
        /// <param name="tradeClass"></param>       
        /// <param name="encryptionMode"></param> 
        /// <param name="zKey"></param>            
        /// <returns>String 'success' if OK</returns>   
        public string ImportDeal(RootObjectRequestTrade trade, PwEnCrypton encryptionMode, ref string zKey)   
         {
            IRestResponse responseImport = new RestResponse();
            
            RootObjectResponseTrade rootObjectResponse;
            string password = string.Empty;
            string retValue = string.Empty;
            string url = string.Empty;
            string sessionId = string.Empty;
            string response = string.Empty;
            bool passwordValid = false;
            bool loggedIn = false;            

            switch (encryptionMode)
            {
                case PwEnCrypton.PW_MANAGER:
                    try
                    {
                        // Decode password.
                        password = Encoder.DecryptString(setPassword, "PESTRASIRUSE");
                        //password = encrypted ? Encoder.DecryptString(setPassword, "PESTRASIRUSE") : setPassword;
                        passwordValid = true;
                    }
                    catch (Exception)
                    {
                        // Catch exception password could not be decoded.
                        retValue = "Password could not be decoded";
                        passwordValid = false;
                    }

                    break;

                case PwEnCrypton.SIMPLE_PASS:
                    password = "*" + setPassword;
                    passwordValid = true;
                    break;

                case PwEnCrypton.PLAIN:
                    password = setPassword;
                    passwordValid = true;
                    break;
            }

            if (passwordValid)
            {
                try
                {
                    url = "http://" + setServer + @":8180/trm/rest/trade";

                    // Login to TRM.
                    loggedIn = Login(ref sessionId, encryptionMode, ref retValue);

                    if (loggedIn)
                    {
                        restClient.BaseUrl = new Uri(url);
                        var request = new RestRequest(Method.PUT);

                        request.AddHeader("content-type", "application/json");
                        request.AddHeader("accept", "application/json");
                        request.AddHeader("ALIGNE_SESSION_ID", sessionId);                           

                        string output = Newtonsoft.Json.JsonConvert.SerializeObject(trade);

                        //Adding JSON body as parameter to the request
                        request.AddParameter("application/json", output, ParameterType.RequestBody);

                        responseImport = restClient.Execute(request);

                        //Deserialize response
                        rootObjectResponse = JsonConvert.DeserializeObject<RootObjectResponseTrade>(responseImport.Content);
                        response = rootObjectResponse.ImporterResponse[0].Status;

                        if(null == rootObjectResponse.ImporterResponse[0].ErrorMsgs)
                        {
                            zKey = rootObjectResponse.ImporterResponse[0].ReturnItems[3].ItemValue;
                        }
                        else
                        {
                            retValue = rootObjectResponse.ImporterResponse[0].ErrorMsgs[0].ErrorMsg;
                        }                        

                        if (response.Equals("success"))
                        {
                            // Logout to TRM. Use retVal because it is the last step.                               
                            if(Logout(sessionId, encryptionMode))
                            {
                                retValue = response;
                            }
                            else
                            {
                                retValue = "Import sucessfully, logout failed for TRM REST.";
                                logger.WriteError("Import sucessfully, logout failed for TRM REST.");
                            }
                        }
                    }
                    else
                    {   // Use value from Login function.
                        //retValue = "Login failed for TRM REST.";
                        logger.WriteError("Login failed for TRM REST. Password not valid.");
                    }                    
                }
                catch (Exception exception)
                {
                    exception.Data.Add("ImportDeal", "Exception in " + (MethodBase.GetCurrentMethod().Name) + "(): ");
                    logger.WriteError("Exception in : " + (MethodBase.GetCurrentMethod().Name) + "(): " + exception.Message.ToString());
                    retValue = "NOK - Exception in : " + (MethodBase.GetCurrentMethod().Name) + "(): " + exception.Message.ToString(); 
                    //throw exception;
                }
            }
            else
            {
                // Use value from Login function.
                retValue = "Login failed for TRM REST. Password not valid.";
                logger.WriteError("Login failed for TRM REST. Password not valid.");
            }

            return retValue;
        }
        #endregion

        #region Login to JMS API via REST POST.
        /// <summary>
        /// Login to JMS API via REST POST.
        /// </summary>      
        /// <param name="ref sessionId"></param>
        /// <param name="encryptionMode"></param>    
        /// <param name="errorMessage"></param>    
        /// <returns>true if logged in </returns>
        private bool Login(ref string sessionId, PwEnCrypton encryptionMode, ref string errorMessage) 
        {
            string retValue = string.Empty; 
            string password = string.Empty;
            string url = string.Empty;
            bool passwordValid = false;
            bool loggedIn = false;
            RootObjectRequestLogin loginRequest = new RootObjectRequestLogin();
            RootObjectResponseLogin loginResponse = new RootObjectResponseLogin();
            IRestResponse response = new RestResponse();
           

            switch (encryptionMode)
            {
                case PwEnCrypton.PW_MANAGER:
                    try
                    {
                        // Decode password.
                        password = Encoder.DecryptString(setPassword, "PESTRASIRUSE");
                        //password = encrypted ? Encoder.DecryptString(setPassword, "PESTRASIRUSE") : setPassword;
                        passwordValid = true;
                    }
                    catch (Exception)
                    {
                        // Catch exception password could not be decoded.
                        retValue = "Password could not be decoded";
                        passwordValid = false;
                    }

                    break;

                case PwEnCrypton.SIMPLE_PASS:
                    password = "*" + setPassword;
                    passwordValid = true;
                    break;

                case PwEnCrypton.PLAIN:
                    password = setPassword;
                    passwordValid = true;
                    break;
            }

            if (passwordValid)
            {
                try
                {
                    url = "http://" + setServer + @":8180/trm/rest/login";
                    var request = new RestRequest(Method.POST);

                    restClient = new RestClient(url);

                    // header
                    request.AddHeader("content-type", "application/json");
                    // JSON body ----- {\"userId\":\"USER\",\"password\":\"PWD\"}
                    loginRequest.userId = setUser;
                    loginRequest.password = password;

                    string loginJson = Newtonsoft.Json.JsonConvert.SerializeObject(loginRequest);
                    request.AddJsonBody(loginJson);
                    response = restClient.Execute(request);
                    
                    //Deserialize response
                    if(response.IsSuccessful)
                    {
                        loginResponse = JsonConvert.DeserializeObject<RootObjectResponseLogin>(response.Content);
                        sessionId = loginResponse.sessionId;
                        loggedIn = loginResponse.loggedIn;
                    }
                    else
                    {
                        sessionId = "";
                        loggedIn = false;
                        if (null != response.ErrorMessage)
                        {
                            errorMessage = response.ErrorMessage;
                        }
                        else
                        {
                            errorMessage = response.StatusDescription;
                        }
                    }
                }
                catch (Exception exception)
                {
                    exception.Data.Add("InsertDataTable", "Exception in " + (MethodBase.GetCurrentMethod().Name) + "(): ");
                    throw exception;
                }
            }
            return loggedIn;
        }
        #endregion

        #region Logout from JMS API via REST POST.
        /// <summary>
        /// Logout from JMS API via REST POST.
        /// <param name="environment"></param>   
        /// <param name="encryptionMode"></param>    
        /// <returns>true if logged in </returns>
        private bool Logout(string sessionId, PwEnCrypton encryptionMode) 
        {
            RootObjectRequestLogout logoutRequest = new RootObjectRequestLogout();
            RootObjectResponseLogout logoutResponse = new RootObjectResponseLogout();
            IRestResponse response = new RestResponse();
            string url = string.Empty;
            bool loggedOut = false;

            try
            {
                url = "http://" + setServer + @":8180/trm/rest/login";
                var request = new RestRequest(Method.POST);

                restClient = new RestClient(url);

                // header
                request.AddHeader("content-type", "application/json");
                // JSON body 
                logoutRequest.action = "logout";
                logoutRequest.sessionId = sessionId;

                string logoutJson = Newtonsoft.Json.JsonConvert.SerializeObject(logoutRequest);
                request.AddJsonBody(logoutJson);
                response = restClient.Execute(request);

                //Deserialize response
                logoutResponse = JsonConvert.DeserializeObject<RootObjectResponseLogout>(response.Content);
                loggedOut = logoutResponse.loggedOut;
            }
            catch (Exception exception)
            {
                exception.Data.Add("InsertDataTable", "Exception in " + (MethodBase.GetCurrentMethod().Name) + "(): ");
                throw exception;
            }

            return loggedOut;
        }
        #endregion               
    }

    // Possible improvements
    // Make method Login and Logout public to be in the position to import several trades within one Login/Logout.
    // Method ImportTrade: Use bool 'doLoginLogOff' to decide if Login/Logout is needed.
    // If not needed use second paramter sessionID recieved from Login mwrhod.
}

