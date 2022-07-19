using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PtouchPrintSender
{
	/// <summary>
	/// brother の b-PAC(Brother P-touch Applicable Component、COM コンポーネント)を用いて、プリンター出力を行うクラスです。
	/// </summary>
	sealed class BrotherPrinterDocument : IDisposable
	{
		/// <summary>
		/// ドキュメントオブジェクト
		/// </summary>
		private readonly bpac.Document _document;

		/// <summary>
		/// コンストラクター
		/// </summary>
		public BrotherPrinterDocument()
		{
			this._document = new bpac.Document();
			// まとめて印刷した方がパフォーマンスが向上します。
			// this._document.StartPrint("DocumentName", bpac.PrintOptionConstants.bpoAutoCut);
		}

		/// <summary>
		/// ドキュメントオブジェクトを返します。
		/// </summary>
		/// <returns></returns>
		private bpac.Document GetDocumentObject()
		{
			return this._document;
		}

		/// <summary>
		/// テンプレートファイルにアタッチします。
		/// </summary>
		/// <param name="templateFileName">テンプレートファイル名</param>
		public void AttachDocumentTemplate(string path)
		{
			var document = this.GetDocumentObject();
			if (!document.Open(path))
			{
				throw new Exception("ドキュメントテンプレートを開けません。");
			}
		}

		public void Print(Dictionary<string, string> fields)
		{
			if (fields == null)
			{
				throw new Exception("Application Error! reason: fields is null.");
			}
			var document = this.GetDocumentObject();

			foreach (var e in fields)
			{
				var field = document.GetObject(e.Key);
				if (field == null)
				{
					Console.WriteLine("[ERROR] field is undefined. key: [{0}]", e.Key);
					continue;
				}
				document.GetObject(e.Key).Text = e.Value;
			}

			// 印刷
			document.StartPrint("DocumentName", bpac.PrintOptionConstants.bpoAutoCut);
			document.PrintOut(1, bpac.PrintOptionConstants.bpoAutoCut);
			document.EndPrint();
		}

		/// <summary>
		/// ドキュメントを閉じてリソースを解放します。
		/// </summary>
		public void Close()
		{
			// まとめて印刷した方がパフォーマンスが向上します。
			// this._document.EndPrint();
			this._document.Close();
			Console.WriteLine("[DEBUG] プリンターオブジェクトが解放されました。");
		}

		/// <summary>
		/// リソースを解放します。
		/// </summary>
		public void Dispose()
		{
			this.Close();
		}
	}
}
