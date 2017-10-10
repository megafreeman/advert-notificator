using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AdvertNotificator.DataAccess;
using AdvertNotificator.Logging;
using AdvertNotificator.Models;
using AdvertNotificator.NotificationSender;
using AdvertNotificator.Shared;

namespace AdvertNotificator
{
	public class AdvertSender
	{
		private readonly IPushNotificationSender _pushNotificationSender;
		private readonly ILogger _logger;
		private readonly AdvertsContext _context;

		public AdvertSender(IPushNotificationSender pushNotificationSender, ILogger logger, AdvertsContext context)
		{
			_pushNotificationSender = pushNotificationSender;
			_logger = logger;
			_context = context;
		}

		public async Task<PushAllSendResult> DoWork()
		{
			var result = new PushAllSendResult();

			try
			{
				result.BeforeTotal = _context.Adverts.Count(a => !a.IsNotificated);

				List<Advert> adverts = _context.Adverts.Where(a => !a.IsNotificated).OrderBy(a => a.CreatedDate).Take(5).ToList();

				foreach (var advert in adverts)
				{
					bool isSended = _pushNotificationSender.Send(advert);

					if (isSended)
					{
						advert.IsNotificated = true;
						_context.Entry(advert).State = EntityState.Modified;
						_context.SaveChanges();

						result.SendOk = result.SendOk + 1 ?? 1;
					}
					else
					{
						result.SendFail = result.SendFail + 1 ?? 1;
					}

					await Task.Delay(31000);
				}

				result.AfterTotal = _context.Adverts.Count(a => !a.IsNotificated);
				result.GrandTotal = _context.Adverts.Count();
			}
			catch (Exception ex)
			{
				_logger.Error("AdvertSender", ex);
			}

			return result;
		}
	}
}