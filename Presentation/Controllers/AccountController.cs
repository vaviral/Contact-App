using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using Newtonsoft.Json;
using Presentation.Models;
using log4net;

namespace Presentation.Controllers
{
    public class AccountController : Controller
    {
        string ServiceAddress;
        HttpClient Client;
        private ILog Log;

        public AccountController()
        {
            Log = LogManager.GetLogger(typeof(AccountController));
            ServiceAddress = ConfigurationManager.AppSettings["ServiceAddress"];
            Client = new HttpClient();
            Client.BaseAddress = new Uri(ServiceAddress);
        }

        [Authorize]
        public ActionResult AddContact()
        {
            Log.Info("Showing add contact view");
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddContact(Contacts model)
        {
            try
            {
                Log.Info("Submiting new contact information");
                var response = JsonConvert.DeserializeObject<Result>(Client.PostAsJsonAsync("AddContact/" + Session["Auth"].ToString(), model).Result.Content.ReadAsStringAsync().Result);
                if (response.Status == 1)
                {
                    Log.Info("New contact information submitted");
                    return Redirect("~/Account/Home");
                }
                else
                {
                    Log.Info("New contact info submission failed");
                    throw new Exception(response.Err_Message);
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        [Authorize]
        public ActionResult Home()
        {
            try
            {
                Log.Info("Retrieveing and showing contacts");
                SearchFilter model = new SearchFilter();
                model.StartIndex = 0;
                model.EndIndex = 13;
                var response = JsonConvert.DeserializeObject<Result>(Client.PostAsJsonAsync("ViewAllContacts/" + Session["Auth"].ToString(), model).Result.Content.ReadAsStringAsync().Result);
                if (response.Status == 1)
                    ViewBag.Contacts = response.ListOfContacts;
                return View();
            }
            catch (Exception e)
            {
                Log.Info(e.Message);
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        //[Authorize]
        //[HttpPost]
        //public ActionResult Home(SearchFilter model)
        //{
        //    try
        //    {
        //        Log.Info("")
        //        var response = JsonConvert.DeserializeObject<Result>(Client.PostAsJsonAsync("ViewAllContacts/" + Session["Auth"].ToString(), model).Result.Content.ReadAsStringAsync().Result);
        //        if (response.Status == 1)
        //            ViewBag.Contacts = response.ListOfContacts;
        //        return View();
        //    }
        //    catch (Exception e)
        //    {
        //        ViewBag.ErrorMessage = e.Message;
        //        return View("Error");
        //    }
        //}

        [Authorize]
        public ActionResult ContactInfo(string Contact)
        {
            try
            {
                Log.Info("Getting contact info for " + Contact);
                Contacts model = new Contacts();
                model.Contact = Contact;
                var response = JsonConvert.DeserializeObject<Contacts>(Client.PostAsJsonAsync("ViewContact/" + Session["Auth"].ToString(), model).Result.Content.ReadAsStringAsync().Result);
                if (response != null)
                    ViewBag.Contacts = response;
                return View();
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        [Authorize]
        public ActionResult SentMessages()
        {
            try
            {
                Log.Info("Getting sent messages");
                SearchFilter model = new SearchFilter();
                model.StartIndex = 0;
                model.EndIndex = 13;
                var response = JsonConvert.DeserializeObject<Result>(Client.PostAsJsonAsync("ViewSentMessages/" + Session["Auth"].ToString(), model).Result.Content.ReadAsStringAsync().Result);
                ViewBag.OTP = response.ListOfMessages;
                if (response.Status == 1)
                {
                    ViewBag.Contacts = response.ListOfMessages;
                    //ViewBag.TotalIndexes = response.Data;
                }
                return View(model);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        [Authorize]
        public ActionResult Compose(string Contact)
        {
            try
            {
                Log.Info("Composing new message for " + Contact);
                ComposeMessage model = new ComposeMessage();
                model.Contact = Contact;
                var response = JsonConvert.DeserializeObject<ComposeMessage>(Client.PostAsJsonAsync("GetOTP/" + Session["Auth"].ToString(), model).Result.Content.ReadAsStringAsync().Result);
                return View(response);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult Send(ComposeMessage model)
        {
            try
            {
                Log.Info("Sending OTP to " + model.Contact);
                var response = JsonConvert.DeserializeObject<Result>(Client.PostAsJsonAsync("SendOTP/" + Session["Auth"].ToString(), model).Result.Content.ReadAsStringAsync().Result);
                if (response.Status == 1)
                    return Redirect("/Account/SentMessages");
                return View();
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        public ActionResult Login()
        {
            Log.Info("Viewing Login page");
            return View();
        }

        [HttpPost]
        public ActionResult Login(User model)
        {
            try
            {
                Log.Info("Signing in " + model.Username);
                var response = JsonConvert.DeserializeObject<Result>(Client.PostAsJsonAsync("Login", model).Result.Content.ReadAsStringAsync().Result);
                if (response.Status == 1)
                {
                    Session["Auth"] = model.Username;
                    FormsAuthentication.SetAuthCookie(model.Username, true);
                    return Redirect("/Account/Home");
                }
                else throw new Exception("Wrong Credentials");
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        [Authorize]
        public ActionResult Logout()
        {
            Log.Info("Signing out " + Session["Auth"].ToString());
            Session.Remove("Auth");
            FormsAuthentication.SignOut();
            return View("Login");
        }
    }
}