using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PtouchPrintSender
{
	/// <summary>
	/// 各種文字列操作
	/// </summary>
	internal static class StringUtility
	{
		/// <summary>
		/// 文字列検査。
		/// </summary>
		/// <param name="s">文字列</param>
		/// <returns>空文字列、または null の場合に true を返します。</returns>
		public static bool IsEmptyOrNull(string s)
		{
			return s == null || s == "";
		}

		/// <summary>
		/// 文字 c が数字かどうかを調べます。
		/// </summary>
		/// <param name="c">文字</param>
		/// <returns>数字の場合は true</returns>
		public static bool IsNumber(char c)
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
		public static bool IsNumber(string text)
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
		public static string FormatPhoneNumber(string phone)
		{
			if (StringUtility.IsEmptyOrNull(phone))
				return "";
			if (!StringUtility.IsNumber(phone))
				return phone;

			if (phone.StartsWith("050"))
			{
				if (phone.Length == 11)
				{
					return $"電話: {phone.Substring(0, 3)}-{phone.Substring(3, 4)}-{phone.Substring(7, 4)}";
				}
			}
			else if (phone.StartsWith("070"))
			{
				if (phone.Length == 11)
				{
					return $"電話: {phone.Substring(0, 3)}-{phone.Substring(3, 4)}-{phone.Substring(7, 4)}";
				}
			}
			else if (phone.StartsWith("080"))
			{
				if (phone.Length == 11)
				{
					return $"電話: {phone.Substring(0, 3)}-{phone.Substring(3, 4)}-{phone.Substring(7, 4)}";
				}
			}
			else if (phone.StartsWith("090"))
			{
				if (phone.Length == 11)
				{
					return $"電話: {phone.Substring(0, 3)}-{phone.Substring(3, 4)}-{phone.Substring(7, 4)}";
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
		public static string FixTargetName(string name, string phone, string productName, string size)
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
		public static string RetrieveField(string[] fields, int index)
		{
			if (fields == null)
				return "";
			if (fields.Length <= index)
				return "";
			return "" + fields[index];
		}
	}
}
