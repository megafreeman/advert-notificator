using System;
using System.Data.Entity;
using System.Linq;
using AdvertNotificator.DataAccess;
using AdvertNotificator.Logging;
using Autofac;
using Topshelf;
using Topshelf.Autofac;

namespace AdvertNotificator
{
	internal class Program
	{
		private static IContainer _container;

		private static void Main()
		{
			log4net.Config.XmlConfigurator.Configure();
			//Database.SetInitializer(new DropCreateDatabaseIfModelChanges<AdvertsContext>());
			Database.SetInitializer(new DbInitializer());

			_container = ContainerConfig.Configure();

			using (var scope = _container.BeginLifetimeScope())
			{
				var logger = scope.Resolve<ILogger>();
				logger.Info("Приложение запущено");

				var ctx = scope.Resolve<AdvertsContext>();

				try
				{
					var d = ctx.Channels.ToList();
					var l = ctx.Links.ToList();
				}
				catch (Exception ex)
				{

					throw;
				}
				
			}

			

			HostFactory.Run(config =>
			{
				config.UseAutofacContainer(_container);

				config.Service<Application>(s =>
				{
					s.ConstructUsingAutofacContainer();

					s.WhenStarted((service, control) => service.Start());
					s.WhenStopped((service, control) => service.Stop());
					//s.WhenPaused((service, control) => service.Pause());
				});

				config.SetServiceName("AdvertNotificator");
				config.SetDisplayName("Advert Notificator");
				config.SetDescription("Сервис по отслеживанию новых объявлений");

				config.StartAutomatically();
			});
		}
	}
}