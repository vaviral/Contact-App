using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //string FilePath = @"C:\Users\HP\Documents\Visual Studio 2015\Projects\SampleContactApp\DataLayer\Files";
            //Directory.CreateDirectory(FilePath);
            //var file = File.OpenWrite(Path.Combine(FilePath + "\\", "Contacts.txt"));
            //file.Close();
            string Time = DateTime.Now.ToString();
            Console.WriteLine(Time);
            Console.Read();
        }
    }
}
