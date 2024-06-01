using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PtouchPrintSender
{
	/// <summary>
	/// アプリケーションの本体
	/// </summary>
	internal sealed class Program
	{
		/// <summary>
		/// エントリーポイント
		/// </summary>
		/// <param name="args">コマンドライン引数</param>
		public static void Main(string[] args)
		{
			try
			{
				MyLogger.Info("### START ###");

				// コンフィギュレーション
				var conf = ConfigurationManager.Configure();

				// アプリケーションを実行
				new Application().Run(conf);

				MyLogger.Info("--- END ---");
			}
			catch (Exception e)
			{
				MyLogger.Error("予期しない実行時エラーです。" + e);
			}
		}
	}
}
