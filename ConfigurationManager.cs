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
		/// アドレス帳ファイル
		/// </summary>
		public readonly List<string> addressFiles = new List<string>();

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

			// コマンドライン引数の読み取り
			// * OPTION: --dryrun (bool)
			// * OPTION: --address-file (multiple)

			var currentSection = "";

			foreach (var arg in System.Environment.GetCommandLineArgs().Skip(1))
			{
				if (arg == "--dryrun")
				{
					currentSection = "dryrun";
					conf.dryrun = true;
				}
				else if (arg.StartsWith("--address-file="))
				{
					// オプションと値が同時に指定されている
					conf.addressFiles.Add(arg.Substring("--address-file=".Length));
				}
				else if (arg == "--address-file")
				{
					// オプションの後に値が続く
					currentSection = "address-file";
				}
				else if (arg.StartsWith("--"))
				{
					// その他のオプションは無い
					currentSection = "";
					throw new Exception("Invalid argument: [" + arg + "]");
				}
				else if (currentSection == "dryrun")
				{
					currentSection = "";
					throw new Exception("Invalid argument: [" + arg + "]");
				}
				else if (currentSection == "address-file")
				{
					if (arg != "") conf.addressFiles.Add(arg);
					currentSection = "";
				}
				else
				{
					// 不明な文字列
					throw new Exception("Invalid argument: [" + arg + "]");
				}
			}

			Console.WriteLine("--dryrun: " + conf.dryrun);
			Console.WriteLine("--address-file: " + string.Join(", ", conf.addressFiles));

			return conf;
		}
	}
}
