using System;
using AdvertNotificator.Models;

namespace AdvertNotificator.NotificationSender
{
	public interface IPushNotificationSender
	{
		bool Send(Advert advert);

		bool Send(Channel channel, string title, string text, string url);

		bool SendEx(Exception exception);
	}
}