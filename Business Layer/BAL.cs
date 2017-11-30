using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects.Models;
using DataLayer;
using Log;
using log4net;
using System.Configuration;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Business_Layer
{
    public class BAL : ConfigLogs
    {
        private ILog Log;
        DAL Dal;

        public BAL() : base()
        {
            Log = LogManager.GetLogger(typeof(BAL));
            Dal = new DAL();
        }

        public Result AddCustomer(Contacts model)
        {
            try
            {
                Log.Info("Add new customer");
                Result Result = new Result();
                if (Dal.RetrieveCustomer(model) != null)
                {
                    Result.Data = 0;
                    Result.Status = 0;
                    Result.Err_Message = "Contact Already Exist";
                    Log.Info("Adding new customer failed");
                }
                else
                {
                    Result.Data = Dal.CreateContact(model);
                    Result.Status = 1;
                    Result.Message = "Success";
                    Log.Info("new customer added");
                }
                return Result;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public Result RetrieveAllCustomer(SearchFilter mode)
        {
            try
            {
                Log.Info("Retrieving all customers");
                Result Result = new Result();
                List<Contacts> List = Dal.RetrieveAllCustomer();
                if (List != null)
                {
                    Result.Status = 1;
                    Result.Message = "Success." + List.Count + " record(s) found";
                    //if (model.EndIndex > List.Count)
                    //    model.EndIndex = List.Count;
                    //Result.ListOfContacts = List.GetRange(model.StartIndex, model.EndIndex);
                    Result.ListOfContacts = List;
                    Result.Data = List.Count();
                    Log.Info("list of customer retrieved");
                }
                else
                {
                    Result.Status = 0;
                    Result.Message = "Success. No record found";
                    Result.ListOfContacts = List;
                    Result.Data = 0;
                    Log.Info("No records found");
                }
                return Result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Contacts RetrieveCustomer(Contacts model)
        {
            try
            {
                Log.Info("Retrieving customer" + model.Contact);
                return Dal.RetrieveCustomer(model);
            }
            catch(Exception e)
            {
                Log.Info("Retrieving customer failed");
                Log.Error(e.Message);
                throw e;
            }
        }
        
        public Result GetSentMessages(SearchFilter model)
        {
            try
            {
                Log.Info("Retrieve sent message");
                Result Result = new Result();
                List<ComposeMessage> List = Dal.RetrieveSentMessages();
                if (List != null)
                {
                    Result.Status = 1;
                    Result.Message = "Success." + List.Count + " record(s) found";
                    //if (model.EndIndex > List.Count)
                    //    model.EndIndex = List.Count;
                    //Result.ListOfMessages = List.OrderByDescending(e => e.Time).ToList().GetRange(model.StartIndex, model.EndIndex);
                    Result.ListOfMessages = List.OrderByDescending(e => e.Time).ToList();
                    Result.Data = List.Count();
                    Log.Info("Messages retrieved");
                }
                else
                {
                    Result.Status = 0;
                    Result.Message = "Success. No record found";
                    Result.ListOfMessages = List;
                    Result.Data = 0;
                    Log.Info("Messages retrieval unsuccessful");
                }
                return Result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ComposeMessage GetOTPMessage(ComposeMessage model)
        {
            Log.Info("GetOTP message called");
            model.Message = "Hi. Your OTP is:";
            Random rand = new Random();
            for (int i = 0; i < 6; i++)
                model.Message += rand.Next(0, 9);
            Contacts model1 = new Contacts();
            model1.Contact = model.Contact;
            model1 = Dal.RetrieveCustomer(model1);
            model.FirstName = model1.FirstName;
            model.LastName = model1.LastName;
            Log.Info("OTP sent to presentation");
            return model;
        }

        public Result SendOTP(ComposeMessage model)
        {
            try
            {
                Log.Info("Sending OTP to " + model.Contact);
                var AccountSid = ConfigurationManager.AppSettings["TwilioAccountSid"];
                var AuthToken = ConfigurationManager.AppSettings["TwilioAuthToken"];
                TwilioClient.Init(AccountSid, AuthToken);
                var message = MessageResource.Create(
                    to: new PhoneNumber("+918288871108"),
                    from: new PhoneNumber("+17814104343"),
                    body: model.Message);
                Dal.SaveOTP(model);
                Log.Info("OTP sent to " + model.Contact);
                return new Result() { Status = 1, Message = "Success" };
            }
            catch (Twilio.Exceptions.TwilioException e)
            {
                Log.Error(e.Message);
                throw e;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public Result Login(User model)
        {
            Log.Info("Signing in " + model.Username);
            Result result = new Result();
            if(Dal.RetrieveLoginInfo(model) == 1)
            {
                result.Status = 1;
                result.Message = "Success";
                Log.Info("Signed in as " + model.Username);
            }
            else
            {
                result.Status = 0;
                result.Message = "Wrong Credentials";
                Log.Info("Wrong credentials given as " + model.Username);
            }
            return result;
        }

        public bool CheckApiKeyValidity(string ApiKey)
        {
            Log.Info("Checking ApiKey validity " + ApiKey);
            if (Dal.CheckApiKey(ApiKey) == 1)
            {
                Log.Info("ApiKey valid");
                return true;
            }
            else
            {
                Log.Info("ApiKey invalid");
                return false;
            }
        }

    }
}
