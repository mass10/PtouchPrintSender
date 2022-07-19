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
			// コンフィギュレーション
			var conf = ConfigurationManager.configure();

			try
			{
				new Application().Run(conf);
			}
			catch (Exception e)
			{
				Console.WriteLine("[ERROR] 予期しない実行時エラーです。" + e);
			}
		}
	}
}
