using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using DataObjects.Models;

namespace WebServices
{
    [ServiceContract]
    public interface IContactAppServices
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/AddContact/{ApiKey}",
            ResponseFormat =WebMessageFormat.Json,
            Method ="POST")]
        Result AddCustomer(Contacts model, string ApiKey);

        [OperationContract]
        [WebInvoke(UriTemplate = "/ViewAllContacts/{ApiKey}",
            ResponseFormat = WebMessageFormat.Json,
            Method = "POST")]
        Result ViewAllCustomers(SearchFilter model, string ApiKey);

        [OperationContract]
        [WebInvoke(UriTemplate = "/ViewContact/{ApiKey}",
            ResponseFormat = WebMessageFormat.Json,
            Method = "POST")]
        Contacts ViewCustomer(Contacts model, string ApiKey);

        [OperationContract]
        [WebInvoke(UriTemplate = "/ViewSentMessages/{ApiKey}",
            ResponseFormat = WebMessageFormat.Json,
            Method = "POST")]
        Result ViewSentMessages(SearchFilter model, string ApiKey);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetOTP/{ApiKey}",
            ResponseFormat = WebMessageFormat.Json,
            Method = "POST")]
        ComposeMessage GetOTP(ComposeMessage model, string ApiKey);

        [OperationContract]
        [WebInvoke(UriTemplate = "/SendOTP/{ApiKey}",
            ResponseFormat = WebMessageFormat.Json,
            Method = "POST")]
        Result SendOTP(ComposeMessage model, string ApiKey);

        [OperationContract]
        [WebInvoke(UriTemplate = "/Login",
            ResponseFormat = WebMessageFormat.Json,
            Method = "POST")]
        Result CheckLogin(User model);
    }
}
