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
				var doc = new BrotherPrinterDocument();

				// テンプレートファイルにアタッチ
				doc.AttachDocumentTemplate("C:\\path\\to\\your\\DATA\\template.lbx");

				// アドレスを印刷
				doc.Print("999-9999 東京都千代田区大手町 1-1", "OTE MACHICO");
			}
			catch (Exception e)
			{
				Console.WriteLine("[ERROR] 予期しない実行時エラーです。" + e);
			}
		}
	}
}
