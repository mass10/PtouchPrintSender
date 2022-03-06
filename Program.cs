using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PtouchPrintSender
{
	internal sealed class Program
	{
		/// <summary>
		/// エントリーポイント
		/// </summary>
		/// <param name="args">コマンドライン引数</param>
		public static void Main(string[] args)
		{
			// ドキュメントオブジェクトを初期化
			var document = new BrotherPrinterDocument();

			try
			{
				// テンプレートファイルにアタッチ
				document.AttachDocumentTemplate("宛名.lbx");

				// アドレス帳を開きます。
				var reader = new System.IO.StreamReader("ADDRESS.tsv", System.Text.Encoding.UTF8);
				while (true)
				{
					var line = reader.ReadLine();
					if (line == null)
						break;

					var fields = line.Split('\t');

					var items = new Dictionary<string, string>();
					items["AddressText"] = String.Format("{0} {1}", fields[0], fields[1]);

					// 電話番号(もしあれば)
					var phoneNumber = RetrieveField04(fields);

					items["NameText"] = FixTargetName(fields[2], phoneNumber);

					// 印刷
					document.Print(items);

					Console.WriteLine("[DEBUG] 住所: [{0}], 氏名: [{1}]", items["AddressText"], items["NameText"]);
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

		private static string FormatPhoneNumber(string phone)
		{
			if (phone.StartsWith("050"))
			{
				if (IsNumber(phone))
				{
					if (phone.Length == 11)
					{
						return $"{phone.Substring(0, 3)}-{phone.Substring(3, 4)}-{phone.Substring(7, 4)}";
					}
				}
			}
			else if (phone.StartsWith("070"))
			{
				if (IsNumber(phone))
				{
					if (phone.Length == 11)
					{
						return $"{phone.Substring(0, 3)}-{phone.Substring(3, 4)}-{phone.Substring(7, 4)}";
					}
				}
			}
			else if (phone.StartsWith("080"))
			{
				if (IsNumber(phone))
				{
					if (phone.Length == 11)
					{
						return $"{phone.Substring(0, 3)}-{phone.Substring(3, 4)}-{phone.Substring(7, 4)}";
					}
				}
			}
			else if (phone.StartsWith("090"))
			{
				if (IsNumber(phone))
				{
					if (phone.Length == 11)
					{
						return $"{phone.Substring(0, 3)}-{phone.Substring(3, 4)}-{phone.Substring(7, 4)}";
					}
				}
			}
			return phone;
		}

		/// <summary>
		/// 名前に敬称を付加します。
		/// </summary>
		/// <param name="name">氏名、もしくは法人名</param>
		/// <returns></returns>
		private static string FixTargetName(string name, string phone)
		{
			name = $"{name}様";

			if (phone == null)
				return name;
			if (phone == "")
				return name;

			return $"{name} ({phone})";
		}

		/// <summary>
		/// フィールド[4] を取り出します。これは電話番号を意味します。
		/// </summary>
		/// <param name="fielids"></param>
		/// <returns></returns>
		private static string RetrieveField04(string[] fielids)
		{
			if (fielids == null)
				return "";
			if (fielids.Length < 4)
				return "";
			return "" + fielids[3];
		}
	}
}
