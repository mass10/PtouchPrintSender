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
				document.AttachDocumentTemplate("..\\..\\宛名.lbx");

				var reader = new System.IO.StreamReader("..\\..\\ADDRESS.tsv", System.Text.Encoding.UTF8);
				while (true)
				{
					var line = reader.ReadLine();
					if (line == null)
						break;

					var fields = line.Split('\t');

					var items = new Dictionary<string, string>();
					items["AddressText"] = String.Format("{0} {1}", fields[0], fields[1]);
					items["NameText"] = fields[2];

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
	}
}
