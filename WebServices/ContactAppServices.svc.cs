using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DataObjects.Models;
using Business_Layer;

namespace WebServices
{
    public class ContactAppServices : IContactAppServices
    {
        public Result AddCustomer(Contacts model, string ApiKey)
        {
            if (new BAL().CheckApiKeyValidity(ApiKey))
                return new BAL().AddCustomer(model);
            else
                return new Result() { Status = 0, Err_Message = "Invalid User Id" };
        }

        public ComposeMessage GetOTP(ComposeMessage model, string ApiKey)
        {
            if (new BAL().CheckApiKeyValidity(ApiKey))
                return new BAL().GetOTPMessage(model);
            else
                return null;
        }

        public Result SendOTP(ComposeMessage model, string ApiKey)
        {
            if (new BAL().CheckApiKeyValidity(ApiKey))
                return new BAL().SendOTP(model);
            else
                return new Result() { Status = 0, Err_Message = "Invalid User Id" };
        }

        public Result ViewAllCustomers(SearchFilter model, string ApiKey)
        {
            if (new BAL().CheckApiKeyValidity(ApiKey))
                return new BAL().RetrieveAllCustomer(model);
            else
                return new Result() { Status = 0, Err_Message = "Invalid User Id" };
        }

        public Contacts ViewCustomer(Contacts model, string ApiKey)
        {
            if (new BAL().CheckApiKeyValidity(ApiKey))
                return new BAL().RetrieveCustomer(model);
            else
                return null;
        }

        public Result ViewSentMessages(SearchFilter model, string ApiKey)
        {
            if (new BAL().CheckApiKeyValidity(ApiKey))
                return new BAL().GetSentMessages(model);
            else
                return new Result() { Status = 0, Err_Message = "Invalid User Id" };
        }

        public Result CheckLogin(User model)
        {
            return new BAL().Login(model);
        }
    }
}
