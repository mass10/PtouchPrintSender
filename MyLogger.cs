using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PtouchPrintSender
{
    class MyLogger
    {
        private MyLogger()
        {

        }

        //public static void Info(string format, params object[] args)
        //{
        //    var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        //    var line = string.Format(format, args);
        //    Console.WriteLine($"{timestamp} [INFO] {line}");
        //}

        public static void Info(params object[] args)
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var line = string.Join("", args);
            Console.WriteLine($"{timestamp} [INFO] {line}");
        }

        //public static void Error(string format, params object[] args)
        //{
        //    var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        //    var line = string.Format(format, args);
        //    Console.WriteLine($"{timestamp} [ERROR] {line}");
        //}

        public static void Error(params object[] args)
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var line = string.Join("", args);
            Console.WriteLine($"{timestamp} [ERROR] {line}");
        }
    }
}
