using System;
using log4net;

namespace AdvertNotificator.Logging
{
	public class Logger : ILogger
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Logger));

		public void Debug(string msg)
		{
			Log.Debug(msg);

			//System.Diagnostics.Debug.WriteLine("{0} {1}", DateTime.Now.TimeOfDay, msg);
		}

		public void Error(string msg, Exception exception)
		{
			Log.Error(msg, exception);

			//System.Diagnostics.Debug.WriteLine("{0} {1}", DateTime.Now.TimeOfDay, msg);
		}

		public void Error(string msg)
		{
			Log.Error(msg);

			//System.Diagnostics.Debug.WriteLine("{0} {1}", DateTime.Now.TimeOfDay, msg);
		}

		public void Info(string msg)
		{
			Log.Info(msg);
		}
	}
}