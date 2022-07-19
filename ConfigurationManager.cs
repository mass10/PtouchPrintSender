using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PtouchPrintSender
{
	/// <summary>
	/// コンフィギュレーション
	/// </summary>
	internal sealed class ConfigurationManager
	{
		/// <summary>
		/// テスト実行
		/// </summary>
		public bool dryrun = false;

		/// <summary>
		/// 文字列を bool に変換します。
		/// </summary>
		/// <param name="value">文字列</param>
		/// <returns>結果</returns>
		private static bool ParseBoolean(string value)
		{
			if (value == null)
				return false;
			return value.ToUpper() == "TRUE";
		}

		/// <summary>
		/// コンフィギュレーションの実行
		/// </summary>
		/// <returns>オブジェクトのインスタンス</returns>
		public static ConfigurationManager Configure()
		{
			var conf = new ConfigurationManager();

			// PT_DRYRUN
			var dryrun = ParseBoolean(System.Environment.GetEnvironmentVariable("PT_DRYRUN", System.EnvironmentVariableTarget.Process));
			conf.dryrun = dryrun;

			if (System.Environment.GetCommandLineArgs().Contains("--dryrun"))
			{
				conf.dryrun = true;
			}
			//Console.WriteLine("PT_DRYRUN: " + conf.dryrun);
			return conf;
		}
	}
}
