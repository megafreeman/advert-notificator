using System;

namespace AdvertNotificator.Logging
{
	public interface ILogger
	{
		void Debug(string msg);
		void Error(string msg, Exception exception);
		void Error(string msg);
		void Info(string msg);
	}
}