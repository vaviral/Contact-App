using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects.Models;
using System.IO;
using Log;
using log4net;
using Newtonsoft.Json;
using System.Configuration;

namespace DataLayer
{
    public class DAL : ConfigLogs
    {
        string FilePath;
        string ContactFileName = "Contacts.txt";
        string MessagesFileName = "Sent Messages.txt";
        string UsersFileName = "Users.txt";
        private ILog Log;

        public DAL() : base()
        {
            Log = LogManager.GetLogger(typeof(DAL));
            FilePath = ConfigurationManager.AppSettings["FilePath"];
            Check_File();
        }

        #region Create

        public int CreateContact(Contacts model)
        {
            return WriteJsonToFile(model, ContactFileName);
        }

        public int SaveOTP(ComposeMessage model)
        {
            model.Time = DateTime.Now.ToString();
            return WriteJsonToFile(model);
        }

        #endregion

        #region Retrieve

        public List<Contacts> RetrieveAllCustomer()
        {
            return ReadAllJsonFromFile(ContactFileName);
        }

        public Contacts RetrieveCustomer(Contacts model)
        {
            try
            {
                return ReadJsonFromFile(model, ContactFileName);
            }
            catch(Exception e)
            {
                Log.Error(e.Message);
                throw e;
            }
        }

        public List<ComposeMessage> RetrieveSentMessages()
        {
            return ReadAllJsonFromFile();
        }

        public int RetrieveLoginInfo(User model)
        {
            return CheckLoginInfo(model);
        }

        #endregion

        public int CheckApiKey(string ApiKey)
        {
            return CheckApiKeyInfo(ApiKey);
        }

        /// <summary>
        /// Checks if file exist and create the file does not exist.
        /// </summary>
        /// <returns>
        /// int 1 if Success
        /// int 0 if failure
        /// </returns>
        private int Check_File()
        {
            try
            {
                Log.Info("Creating directory");
                Directory.CreateDirectory(FilePath);
                Log.Info("Directory created");
                Log.Info("Creating file CONTACTS.TXT");
                var file1 = File.OpenWrite(Path.Combine(FilePath + "\\", ContactFileName));
                file1.Close();
                Log.Info("File created");
                Log.Info("Creating file MESSAGES.TXT");
                var file2 = File.OpenWrite(Path.Combine(FilePath + "\\", MessagesFileName));
                file2.Close();
                Log.Info("File created");
                Log.Info("Creating file USERS.TXT");
                var file3 = File.OpenWrite(Path.Combine(FilePath + "\\", UsersFileName));
                file3.Close();
                Log.Info("File created");
                return 1;
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return 0;
            }
        }

        /// <summary>
        /// Writes data to a file as Json.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// int 1 if Success
        /// int 0 if failure
        /// </returns>
        private int WriteJsonToFile(ComposeMessage model)
        {
            try
            {
                List<ComposeMessage> List = ReadAllJsonFromFile();
                if (List == null)
                    List = new List<ComposeMessage>();
                List.Add(model);
                var serializer = new JsonSerializer();
                using (StreamWriter file = File.CreateText(FilePath + @"\" + MessagesFileName))
                using (JsonTextWriter writer = new JsonTextWriter(file))
                    serializer.Serialize(writer, List);
                return 1;
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return 0;
            }
        }

        /// <summary>
        /// Writes data to a file as Json.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// int 1 if Success
        /// int 0 if failure
        /// </returns>
        private int WriteJsonToFile(Contacts model, string FileName)
        {
            try
            {
                List<Contacts> List = ReadAllJsonFromFile(FileName);
                if (List == null)
                    List = new List<Contacts>();
                List.Add(model);
                var serializer = new JsonSerializer();
                using (StreamWriter file = File.CreateText(FilePath + @"\" + FileName))
                using (JsonTextWriter writer = new JsonTextWriter(file))
                    serializer.Serialize(writer, List);
                return 1;
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return 0;
            }
        }

        /// <summary>
        /// Write data to file as Json.
        /// </summary>
        /// <param name="List"></param>
        /// <returns>
        /// int 1 if Success
        /// int 0 if failure
        /// </returns>
        private int WriteJsonToFile(List<Contacts> List, string FileName)
        {
            try
            {
                var serializer = new JsonSerializer();
                using (StreamWriter file = File.CreateText(FilePath + @"\" + FileName))
                using (JsonTextWriter writer = new JsonTextWriter(file))
                    serializer.Serialize(writer, List);
                return 1;
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return 0;
            }
        }

        /// <summary>
        /// Reads Json from file.
        /// </summary>
        /// <returns>
        /// List of Contacts.
        /// </returns>
        private List<ComposeMessage> ReadAllJsonFromFile()
        {
            try
            {
                var serializer = new JsonSerializer();
                List<ComposeMessage> List = new List<ComposeMessage>();
                using (var file = File.OpenText(FilePath + @"\" + MessagesFileName))
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    List = serializer.Deserialize<List<ComposeMessage>>(reader);
                }
                return List;
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return null;
            }
        }

        /// <summary>
        /// Reads Json from file.
        /// </summary>
        /// <returns>
        /// List of Contacts.
        /// </returns>
        private List<Contacts> ReadAllJsonFromFile(string FileName)
        {
            try
            {
                var serializer = new JsonSerializer();
                List<Contacts> List = new List<Contacts>();
                using (var file = File.OpenText(FilePath + @"\" + FileName))
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    List = serializer.Deserialize<List<Contacts>>(reader);
                }
                return List;
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return null;
            }
        }

        /// <summary>
        /// Reads Json from file.
        /// </summary>
        /// <param name="Contact"></param>
        /// <returns>
        /// List of Contacts.
        /// </returns>
        private Contacts ReadJsonFromFile(Contacts model, string FileName)
        {
            try
            {
                var serializer = new JsonSerializer();
                List<Contacts> List = new List<Contacts>();
                using (var file = File.OpenText(FilePath + @"\" + FileName))
                using (JsonTextReader reader = new JsonTextReader(file))
                    List = serializer.Deserialize<List<Contacts>>(reader);
                return List.Where(e=>e.Contact.Equals(model.Contact)).ToList().FirstOrDefault();
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return null;
            }
        }

        /// <summary>
        /// Check Login.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// int 1 if Success
        /// int 0 if failure
        /// </returns>
        private int CheckLoginInfo(User model)
        {
            try
            {
                var serializer = new JsonSerializer();
                List<User> List = new List<User>();
                using (var file = File.OpenText(FilePath + @"\" + UsersFileName))
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    List = serializer.Deserialize<List<User>>(reader);
                }
                if (List.Where(e => e.Username.Equals(model.Username)).ToList().Where(e => e.Password.Equals(model.Password)).ToList().FirstOrDefault() != null)
                    return 1;
                else return 0;

            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return 0;
            }
        }

        /// <summary>
        /// Check ApiKey.
        /// </summary>
        /// <param name="ApiKey"></param>
        /// <returns>
        /// int 1 if Success
        /// int 0 if failure
        /// </returns>
        private int CheckApiKeyInfo(string ApiKey)
        {
            try
            {
                var serializer = new JsonSerializer();
                List<User> List = new List<User>();
                using (var file = File.OpenText(FilePath + @"\" + UsersFileName))
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    List = serializer.Deserialize<List<User>>(reader);
                }
                if (List.Where(e => e.Username.Equals(ApiKey)).ToList().FirstOrDefault() != null)
                    return 1;
                else return 0;

            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return 0;
            }
        }
    }
}
