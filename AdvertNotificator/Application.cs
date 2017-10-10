using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Timers;
using AdvertNotificator.DataAccess;
using AdvertNotificator.Helpers;
using AdvertNotificator.Logging;
using AdvertNotificator.Models;
using AdvertNotificator.NotificationSender;
using AdvertNotificator.Parsers;
using AdvertNotificator.Shared;
using Autofac;

namespace AdvertNotificator
{
	public class Application
	{
		private readonly Timer _timer;
		private readonly Timer _timerPushSender;

		private readonly ILogger _logger;
		private readonly ILifetimeScope _scope;
		private readonly AdvertsContext _advertsContext;
		private readonly AdvertsErrorChecker _advertsErrorChecker;
		private readonly AdvertSender _advertSender;
		private readonly IPushNotificationSender _pushNotificationSender;

		public Application(ILifetimeScope scope,
			ILogger logger,
			AdvertsContext advertsContext,
			AdvertsErrorChecker advertsErrorChecker,
			AdvertSender advertSender,
			IPushNotificationSender pushNotificationSender)
		{
			_scope = scope;
			_logger = logger;
			_advertsContext = advertsContext;
			_advertsErrorChecker = advertsErrorChecker;
			_advertSender = advertSender;
			_pushNotificationSender = pushNotificationSender;

			_timer = new Timer { AutoReset = true };
			_timer.Elapsed += OnTimedEvent;

			_timerPushSender = new Timer { AutoReset = true };
			_timerPushSender.Elapsed += OnPushSenderTimedEvent;
		}

		private async void OnPushSenderTimedEvent(object source, ElapsedEventArgs e)
		{
			_timerPushSender.Stop();

			_logger.Info("Сработал таймер отправки");

			int? sendedCount = null;

			try
			{
				PushAllSendResult result = await _advertSender.DoWork();

				var stringBuilder = new StringBuilder();

				stringBuilder.AppendLine("Отправка окончена. Результат:");
				stringBuilder.AppendLine(string.Format("Число сообщений до отправки: {0}", result.BeforeTotal));
				stringBuilder.AppendLine(string.Format("Число успешно отправленных сообщений: {0}", result.SendOk));
				stringBuilder.AppendLine(string.Format("Число не отправленных сообщений: {0}", result.SendFail));
				stringBuilder.AppendLine(string.Format("Число сообщений после отправки: {0}", result.AfterTotal));
				stringBuilder.AppendLine(string.Format("Общее число сообщений: {0}", result.GrandTotal));

				_logger.Info(stringBuilder.ToString());

				sendedCount = result.SendOk;
			}
			catch (Exception ex)
			{
				_logger.Error("Application.OnPushSenderTimedEvent", ex);
			}

			_timerPushSender.Interval = sendedCount.HasValue && sendedCount.Value > 0 ? 31000 : 10000;
			_timerPushSender.Start();
		}

		private void OnTimedEvent(object source, ElapsedEventArgs e)
		{
			_timer.Stop();

			_logger.Info("Сработал таймер сбора");

			try
			{
				List<Link> links = _advertsContext.Links.Where(l => l.IsActive).ToList();
				_logger.Info(string.Format("Количество профилей - {0}", links.Count));

				AvitoParser avitoParser = _scope.Resolve<AvitoParser>();

				foreach (Link link in links)
				{
					List<Advert> advertsRaw = avitoParser.StartWork(link);
					List<Advert> adverts = _advertsErrorChecker.CheckList(advertsRaw);

					int count = 0;

					foreach (Advert advert in adverts)
					{
						if (_advertsContext.Adverts.Any(a => a.PublicId == advert.PublicId))
							continue;

						_advertsContext.Adverts.Add(advert);
						_advertsContext.SaveChanges();

						count++;
					}

					if (count > 0)
						_logger.Info(string.Format("{0} -> {1}", link.Name, count));
				}

				if (links.Any(p => p.IsNew))
				{
					foreach (Link link in links.Where(l => l.IsNew))
					{
						//_pushNotificationSender.Send(link.Channel, "Служебное сообщение",
						//	"Сервис настроен. Теперь Вам будут приходить сообщения о новых объявлениях", "pushall.ru");

						link.IsNew = false;
						_advertsContext.Entry(link).State = EntityState.Modified;
						_advertsContext.SaveChanges();
					}
				}
			}
			catch (Exception ex)
			{
				_logger.Error("Application.OnTimedEvent", ex);
			}

			_logger.Info("Обработка завершена");

			_timer.Interval = 45000;
			_timer.Start();
		}

		public bool Start()
		{
			_timer.Interval = 1000;
			_timer.Start();

			_logger.Info("Таймер сбора запущен");


			_timerPushSender.Interval = 3000;
			_timerPushSender.Start();

			_logger.Info("Таймер отправки запущен");

#if (DEBUG)
			{
				List<Channel> channels = _advertsContext.Channels.Where(c => c.IsAdmin).ToList();

				foreach (Channel channel in channels)
				{
					_pushNotificationSender.Send(channel, "Служебное сообщение", "Сервис запущен", "pushall.ru");
				}
			}
#endif

			return true;
		}

		public bool Stop()
		{
			_timer.Stop();
			_logger.Info("Таймер сбора остановлен");

			_timerPushSender.Stop();
			_logger.Info("Таймер отправки остановлен");

			return true;
		}
	}
}