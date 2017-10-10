using AdvertNotificator.DataAccess;
using AdvertNotificator.Helpers;
using AdvertNotificator.Logging;
using AdvertNotificator.NotificationSender;
using AdvertNotificator.Parsers;
using Autofac;

namespace AdvertNotificator
{
	public static class ContainerConfig
	{
		public static IContainer Configure()
		{
			var builder = new ContainerBuilder();

			builder.RegisterType<Application>();

			builder.RegisterType<AdvertsContext>();

			builder.RegisterType<AvitoParser>();

			builder.RegisterType<RegexIntExtractor>();

			builder.RegisterType<AdvertsErrorChecker>();

			builder.RegisterType<AdvertSender>();

			builder.RegisterType<PushAllNotificationSender>().As<IPushNotificationSender>();

			builder.RegisterType<Logger>().As<ILogger>();
			
			return builder.Build();
		}
	}
}