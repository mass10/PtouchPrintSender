using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PtouchPrintSender
{
	internal sealed class ConfigurationManager
	{
		public bool dryrun = false;

		private static bool ParseBoolean(string value)
		{
			if (value == null)
				return false;
			return value.ToUpper() == "TRUE";
		}

		public static ConfigurationManager configure()
		{
			var conf = new ConfigurationManager();

			// PT_DRYRUN
			var dryrun = ParseBoolean(System.Environment.GetEnvironmentVariable("PT_DRYRUN", System.EnvironmentVariableTarget.Process));
			conf.dryrun = dryrun;

			if (System.Environment.GetCommandLineArgs().Contains("--dryrun"))
			{
				conf.dryrun = true;
			}
			Console.WriteLine("PT_DRYRUN: " + conf.dryrun);
			return conf;
		}
	}
}
