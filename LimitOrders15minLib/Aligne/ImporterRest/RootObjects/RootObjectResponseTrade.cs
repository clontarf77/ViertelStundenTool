using System.Collections.Generic;

namespace ViertelStdTool.AligneImporter.Rest
{
    public class RootObjectResponseTrade
    {
        public List<ImporterResponse> ImporterResponse { get; set; }
    }

    public class ImporterResponse
    {
        public string StatusCode { get; set; }
        public string Status { get; set; }
        public List<ErrorMsgItem> ErrorMsgs { get; set; }
        public List<ReturnItem> ReturnItems { get; set; }
    }

    public class ErrorMsgItem
    {
        public string ErrorMsg { get; set; }       
    }

    public class ReturnItem
    {
        public string ItemName { get; set; }
        public string ItemValue { get; set; }
        public string Remark { get; set; }
    }
}


// EXAMPLE (http://json2csharp.com/)
//    {
//    "importerResponse": [
//        {
//            "statusCode": "0",
//            "status": "success",
//            "returnItems": [
//                {
//                    "itemName": "tpowTpl",
//                    "itemValue": "POWER SHAPES",
//                    "remark": "The trade template"
//                },
//                {
//                    "itemName": "auditRefresh",
//                    "itemValue": "6661469",
//                    "remark": "Refresh value"
//                },
//                {
//                    "itemName": "tradeTnum",
//                    "itemValue": "EDG846",
//                    "remark": "Ticket number value"
//                },
//                {
//                    "itemName": "auditZkey",
//                    "itemValue": "6661469",
//                    "remark": "ZKey value"
//                }
//            ]
//        }
//    ]
//}