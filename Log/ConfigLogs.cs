using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Config;
using System.IO;
using System.Configuration;
using log4net;

namespace Log
{
    public class ConfigLogs
    {
        private ILog Log;

        public ConfigLogs()
        {
            //try
            //{
            //    string LogDirectory = ConfigurationManager.AppSettings["LogDirectory"];
            //    string FileName = ConfigurationManager.AppSettings["LogFileName"];
            //    Directory.CreateDirectory(LogDirectory);
            //    var file = File.OpenWrite(Path.Combine(LogDirectory + "\\" + FileName));
            //    file.Close();
            //}
            //catch (Exception e)
            //{
            //    Log.Error(e.Message);
            //    throw e;
            //}
            XmlConfigurator.Configure();
            Log = LogManager.GetLogger(typeof(ConfigLogs));
        }
    }
}
