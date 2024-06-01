using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PtouchPrintSender
{
	/// <summary>
	/// アプリケーション本体の定義
	/// </summary>
	internal sealed class Application
	{
		/// <summary>
		/// コンストラクター
		/// </summary>
		public Application()
		{

		}

		/// <summary>
		/// アプリケーションを実行します。
		/// </summary>
		/// <param name="conf"></param>
		public void Run(ConfigurationManager conf)
		{
			if (conf.addressFiles.Count == 0)
			{
				MyLogger.Error("アドレス帳ファイルが指定されていません。");
				return;
			}

			foreach (var filepath in conf.addressFiles)
			{
				// 印刷を実行します。
				Print(filepath, conf.dryrun);
			}
		}

		/// <summary>
		/// 印刷を実行します。
		/// </summary>
		/// <param name="filepath">アドレス帳ファイル</param>
		/// <param name="dryrun"></param>
		private void Print(string filepath, bool dryrun)
		{
			MyLogger.Info("テンプレート [", filepath, "] を印字中...");

			// ドキュメントオブジェクトを初期化
			var document = new BrotherPrinterDocument();

			int recordCount = 0;

			try
			{
				// テンプレートファイルにアタッチ
				// ・P-touch Editor 5.4 で作成したテンプレートファイル
				document.AttachDocumentTemplate("宛名.lbx");

				// アドレス帳を開きます。
				var reader = new System.IO.StreamReader(filepath, System.Text.Encoding.UTF8); 
				while (true)
				{
					var line = reader.ReadLine();
					if (line == null)
					{
						// EOF
						break;
					}
					if (line == "")
					{
						// 無視
						continue;
					}
					if (line[0] == '#')
					{
						// 無視
						continue;
					}

					// 行を分解
					var fields = ParseAddressLine(line);
					if (fields == null)
					{
						// 無視
						MyLogger.Error("次の行は無視されました。[", line, "]");
						continue;
					}

					if (dryrun)
					{
						// 印刷しない
						continue;
					}

					// 印刷
					MyLogger.Info("(印字中)");
					document.Print(fields);

					recordCount++;
				}
			}
			finally
			{
				// 結果件数を表示
				MyLogger.Info("印刷が完了しました。処理件数: [", recordCount, "件]");
				
				MyLogger.Info("ドキュメントをクローズしています...");
				document.Close();
			}
		}

		/// <summary>
		/// アドレス帳の行をパースします。
		/// </summary>
		/// <param name="line">行</param>
		/// <returns>ディクショナリー</returns>
		private static Dictionary<string, string> ParseAddressLine(string line)
		{
			var columns = line.Split('\t');

			// 郵便番号
			var postCode = StringUtility.At(columns, 0);
			if (postCode == "")
			{
				// フォーマットが無効
				return null;
			}

			// 住所
			var address = StringUtility.At(columns, 1);
			if (address == "")
			{
				// フォーマットが無効
				return null;
			}

			// お名前
			var name = StringUtility.At(columns, 2);
			if (name == "")
			{
				// フォーマットが無効
				return null;
			}

			// 電話番号(もしあれば)
			var phoneNumber = StringUtility.At(columns, 3);

			// 製品名(もしあれば)
			var productName = StringUtility.At(columns, 4);

			// サイズ(もしあれば)
			var size = StringUtility.At(columns, 5);

			var fields = new Dictionary<string, string>();
			// 宛名
			fields["AddressText"] = String.Format("{0} {1}", postCode, address);
			// お名前
			fields["NameText"] = StringUtility.FixTargetName(columns[2], "", productName, size);
			// 配送会社向け連絡先
			fields["PhoneText"] = StringUtility.FormatPhoneNumber(phoneNumber);

			MyLogger.Info("住所: [", fields["AddressText"], "], 氏名: [", fields["NameText"], "], 電話: [", fields["PhoneText"], "]");

			return fields;
		}
	}
}

