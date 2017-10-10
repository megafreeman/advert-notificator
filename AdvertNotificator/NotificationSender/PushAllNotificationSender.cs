using System;
using System.IO;
using System.Net;
using System.Text;
using AdvertNotificator.Models;
using AdvertNotificator.Shared;
using Newtonsoft.Json;

namespace AdvertNotificator.NotificationSender
{
	public class PushAllNotificationSender : IPushNotificationSender
	{
		public bool Send(Advert advert)
		{
			//return true;

			Channel channel = advert.Channel;

			var request = (HttpWebRequest)WebRequest.Create("https://pushall.ru/api.php");

			var postData = "type=broadcast";//var postData = "type=self";
			postData += string.Format("&id={0}", channel.PushallId);//postData += "&id=35378";
			postData += string.Format("&key={0}", channel.PushallKey);//postData += "&key=1f36f5612b426cfbc6f65e0170e20b1b";
			string prefix = string.IsNullOrWhiteSpace(advert.Prefix) ? string.Empty : string.Format("[{0}] ", advert.Prefix);
			postData += string.Format("&title={0}{1}", prefix, advert.Title);
			postData += string.Format("&text={0} - {1}", advert.Price, advert.Address);
			postData += string.Format("&url={0}", advert.Url);
			var data = Encoding.UTF8.GetBytes(postData);

			request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded";
			request.ContentLength = data.Length;

			using (var stream = request.GetRequestStream())
			{
				stream.Write(data, 0, data.Length);
			}

			var response = (HttpWebResponse)request.GetResponse();

			var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

			PushAllResponse responseObject = JsonConvert.DeserializeObject<PushAllResponse>(responseString);

			bool retStatus = false;

			if (responseObject != null)
				retStatus = responseObject.Success > 0;

			return retStatus;
		}

		public bool Send(Channel channel, string title, string text, string url)
		{
			//return true;

			var request = (HttpWebRequest)WebRequest.Create("https://pushall.ru/api.php");

			var postData = "type=broadcast";//var postData = "type=self";
			postData += string.Format("&id={0}", channel.PushallId);//postData += "&id=35378";
			postData += string.Format("&key={0}", channel.PushallKey);//postData += "&key=1f36f5612b426cfbc6f65e0170e20b1b";
			postData += string.Format("&title={0}", title);
			postData += string.Format("&text={0}", text);
			postData += string.Format("&url={0}", url);
			var data = Encoding.UTF8.GetBytes(postData);

			request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded";
			request.ContentLength = data.Length;

			using (var stream = request.GetRequestStream())
			{
				stream.Write(data, 0, data.Length);
			}

			var response = (HttpWebResponse)request.GetResponse();

			var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

			PushAllResponse responseObject = JsonConvert.DeserializeObject<PushAllResponse>(responseString);

			bool retStatus = false;

			if (responseObject != null)
				retStatus = responseObject.Success > 0;

			return retStatus;
		}

		public bool SendEx(Exception exception)
		{
			throw new NotImplementedException();
		}
	}
}