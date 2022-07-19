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
					var postCode = StringUtility.RetrieveField(fields, 0);
					if (postCode == "")
						continue;
					// 住所
					var address = StringUtility.RetrieveField(fields, 1);
					if (address == "")
						continue;
					// お名前
					var name = StringUtility.RetrieveField(fields, 2);
					// 電話番号(もしあれば)
					var phoneNumber = StringUtility.RetrieveField(fields, 3);
					// 製品名(もしあれば)
					var productName = StringUtility.RetrieveField(fields, 4);
					// サイズ(もしあれば)
					var size = StringUtility.RetrieveField(fields, 5);

					var items = new Dictionary<string, string>();
					// 宛名
					items["AddressText"] = String.Format("{0} {1}", postCode, address);
					// お名前
					items["NameText"] = StringUtility.FixTargetName(fields[2], "", productName, size);
					// 配送会社向け連絡先
					items["PhoneText"] = StringUtility.FormatPhoneNumber(phoneNumber);

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
	}
}
