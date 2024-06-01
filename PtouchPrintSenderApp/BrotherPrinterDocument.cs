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

		/// <summary>
		/// ドキュメントを印刷します。
		/// </summary>
		/// <param name="fields">変数のマッピング情報</param>
		public void Print(Dictionary<string, string> fields)
		{
			// ドキュメントオブジェクトを参照
			var document = this.GetDocumentObject();

			foreach (var e in fields)
			{
				// ドキュメント内のプレースホルダーを参照
				var field = document.GetObject(e.Key);
				if (field == null)
				{
					MyLogger.Error("フィールド [{0}] が未定義です。", e.Key);
					continue;
				}

				// 文字列を埋め込み
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
		}

		/// <summary>
		/// リソースを解放します。IDisposable の実装です。
		/// </summary>
		public void Dispose()
		{
			this.Close();
		}
	}
}
