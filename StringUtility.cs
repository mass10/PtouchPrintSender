using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PtouchPrintSender
{
	internal static class StringUtility
	{
		public static bool IsEmptyOrNull(string s)
		{
			return s == null || s == "";
		}
	}
}
