using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PtouchPrintSender
{
    /// <summary>
    /// ロガー
    /// </summary>
    class MyLogger
    {
        /// <summary>
        /// コンストラクターは非公開です。
        /// </summary>
        private MyLogger()
        {

        }

        /// <summary>
        /// ロギング(INFO)
        /// </summary>
        /// <param name="args"></param>
        public static void Info(params object[] args)
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var line = string.Join("", args);
            Console.WriteLine($"{timestamp} [INFO] {line}");
        }

        /// <summary>
        /// ロギング(ERROR)
        /// </summary>
        /// <param name="args"></param>
        public static void Error(params object[] args)
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var line = string.Join("", args);
            Console.WriteLine($"{timestamp} [ERROR] {line}");
        }
    }
}
