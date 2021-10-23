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
			try
			{
				// ドキュメントオブジェクトを初期化
				var document = new BrotherPrinterDocument();

				// テンプレートファイルにアタッチ
				document.AttachDocumentTemplate("template.lbx");

				// AddressText
				var fields = new Dictionary<string, string>();
				fields["AddressText"] = "999-9999 東京都千代田区大手町 1-1";
				fields["NameText"] = "OTE MACHICO";

				// アドレスを印刷
				document.Print(fields);
			}
			catch (Exception e)
			{
				Console.WriteLine("[ERROR] 予期しない実行時エラーです。" + e);
			}
		}
	}
}
