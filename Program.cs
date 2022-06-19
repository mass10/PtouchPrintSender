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
			
			// ドキュメントオブジェクトを初期化
			var document = new BrotherPrinterDocument();

			try
			{
				// テンプレートファイルにアタッチ
				// ・P-touch Editor 5.4 で作成したテンプレートファイル
				document.AttachDocumentTemplate("宛名.lbx");

				// アドレス帳を開きます。
				var reader = new System.IO.StreamReader("ADDRESS.tsv", System.Text.Encoding.UTF8);
				while (true)
				{
					var line = reader.ReadLine();
					if (line == null)
						// EOF
						break;

					if (line == "")
						// 無視
						continue;
					if (line[0] == '#')
						// 無視
						continue;

					var fields = line.Split('\t');
					// 郵便番号
					var postCode = RetrieveField(fields, 0);
					if (postCode == "")
						continue;
					// 住所
					var address = RetrieveField(fields, 1);
					if (address == "")
						continue;
					// お名前
					var name = RetrieveField(fields, 2);
					// 電話番号(もしあれば)
					var phoneNumber = RetrieveField(fields, 3);
					// 製品名(もしあれば)
					var productName = RetrieveField(fields, 4);
					// サイズ(もしあれば)
					var size = RetrieveField(fields, 5);

					var items = new Dictionary<string, string>();
					// 宛名
					items["AddressText"] = String.Format("{0} {1}", postCode, address);
					// お名前
					items["NameText"] = FixTargetName(fields[2], "", productName, size);
					// 配送会社向け連絡先
					items["PhoneText"] = FormatPhoneNumber(phoneNumber);

					Console.WriteLine("[DEBUG] 住所: [{0}], 氏名: [{1}], 電話: [{2}]",
							items["AddressText"], items["NameText"], items["PhoneText"]);

					if (conf.dryrun)
						// 印刷しない
						continue;

					// 印刷
					document.Print(items);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("[ERROR] 予期しない実行時エラーです。" + e);
			}
			finally
			{
				document.Close();
			}
		}

		/// <summary>
		/// 文字 c が数字かどうかを調べます。
		/// </summary>
		/// <param name="c">文字</param>
		/// <returns>数字の場合は true</returns>
		private static bool IsNumber(char c)
		{
			switch (c)
			{
				case '0': return true;
				case '1': return true;
				case '2': return true;
				case '3': return true;
				case '4': return true;
				case '5': return true;
				case '6': return true;
				case '7': return true;
				case '8': return true;
				case '9': return true;
			}
			return false;
		}

		/// <summary>
		/// 文字列 text が数字かどうかを調べます。
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		private static bool IsNumber(string text)
		{
			if (text == null)
				return false;
			if (text == "")
				return false;

			foreach (var c in text)
			{
				if (!IsNumber(c))
					// 何か検出したらアウト
					return false;
			}

			return true;
		}

		/// <summary>
		/// 電話番号のフォーマット
		/// </summary>
		/// <param name="phone">電話番号</param>
		/// <returns></returns>
		private static string FormatPhoneNumber(string phone)
		{
			if (StringUtility.IsEmptyOrNull(phone))
			{
				return "";
			}
			if (phone.StartsWith("050"))
			{
				if (IsNumber(phone))
				{
					if (phone.Length == 11)
					{
						return $"電話: {phone.Substring(0, 3)}-{phone.Substring(3, 4)}-{phone.Substring(7, 4)}";
					}
				}
			}
			else if (phone.StartsWith("070"))
			{
				if (IsNumber(phone))
				{
					if (phone.Length == 11)
					{
						return $"電話: {phone.Substring(0, 3)}-{phone.Substring(3, 4)}-{phone.Substring(7, 4)}";
					}
				}
			}
			else if (phone.StartsWith("080"))
			{
				if (IsNumber(phone))
				{
					if (phone.Length == 11)
					{
						return $"電話: {phone.Substring(0, 3)}-{phone.Substring(3, 4)}-{phone.Substring(7, 4)}";
					}
				}
			}
			else if (phone.StartsWith("090"))
			{
				if (IsNumber(phone))
				{
					if (phone.Length == 11)
					{
						return $"電話: {phone.Substring(0, 3)}-{phone.Substring(3, 4)}-{phone.Substring(7, 4)}";
					}
				}
			}
			return phone;
		}

		/// <summary>
		/// お名前、電話番号をフォーマットして返します。
		///
		/// Jimi Hendrix 様 (090-0000-0000)
		/// </summary>
		/// <param name="name">氏名、もしくは法人名</param>
		/// <param name="phone">電話番号</param>
		/// <returns></returns>
		private static string FixTargetName(string name, string phone, string productName, string size)
		{
			var line = new StringBuilder();
			if (StringUtility.IsEmptyOrNull(phone))
			{
				line.Append($"{name} 様");
			}
			else
			{
				line.Append($"{name} 様 ({FormatPhoneNumber(phone)})");
			}

			if (!StringUtility.IsEmptyOrNull(productName))
			{
				line.Append(" (");
				line.Append(productName);
				if (!StringUtility.IsEmptyOrNull(size))
				{
					line.Append($" {size}");
				}
				line.Append(")");
			}

			return line.ToString();

		}

		/// <summary>
		/// フィールドの値を取り出します。
		/// </summary>
		/// <param name="fields">配列</param>
		/// <param name="index">位置</param>
		/// <returns></returns>
		private static string RetrieveField(string[] fields, int index)
		{
			if (fields == null)
				return "";
			if (fields.Length <= index)
				return "";
			return "" + fields[index];
		}
	}
}
