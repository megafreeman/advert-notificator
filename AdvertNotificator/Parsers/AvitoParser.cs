using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using AdvertNotificator.Helpers;
using AdvertNotificator.Logging;
using AdvertNotificator.Models;
using AngleSharp.Parser.Html;

namespace AdvertNotificator.Parsers
{
	public class AvitoParser
	{
		private readonly RegexIntExtractor _regexIntExtractor;
		private readonly ILogger _logger;

		public AvitoParser(RegexIntExtractor regexIntExtractor, ILogger logger)
		{
			_regexIntExtractor = regexIntExtractor;
			_logger = logger;
		}

		public List<Advert> StartWork(Link link)
		{
			var advertsList = new List<Advert>();

			try
			{
				var parser = new HtmlParser();
				string url = link.Url;
				var webClient = new WebClient();

				using (Stream stream = webClient.OpenRead(url))
				{
					var document = parser.Parse(stream);

					var blocks = document.All
						.Where(b => b.LocalName == "div" && b.ClassList.Any(o => o == "item") && b.ClassList.Any(o => o == "item_table")
									&& b.ClassList.Any(o => o == "clearfix") && b.ClassList.Any(o => o == "js-catalog-item-enum") &&
									b.ClassList.Any(o => o == "c-b-0"))
						.ToList();

					if (blocks.Any())
					{
						foreach (var block in blocks)
						{
							string idRaw = block.Id;
							long publicId = _regexIntExtractor.GetLong(idRaw);

							var ad = new Advert
							{
								PublicId = publicId,
								Prefix = link.Prefix,
								Channel = link.Channel,
								IsNotificated = link.IsNew
							};

							var ahref = block.GetElementsByClassName("item-description-title-link").FirstOrDefault();

							if (ahref != null)
							{
								var attr = ahref.Attributes.FirstOrDefault(a => a.LocalName == "href");

								if (attr != null)
									ad.Url = string.Format("https://www.avito.ru{0}", attr.Value);

								ad.Title = ahref.InnerHtml.Trim();
							}

							var address = block.GetElementsByClassName("address").FirstOrDefault();

							if (address != null)
							{
								ad.Address = address.InnerHtml.Trim();
							}

							var price = block.GetElementsByClassName("about").FirstOrDefault();

							if (price != null)
							{
								string priceRaw = price.InnerHtml;

								foreach (var child in price.Children)
								{
									priceRaw = priceRaw.Replace(child.OuterHtml, string.Empty);
								}

								priceRaw = priceRaw.Trim();
								ad.Price = Convert.ToInt32(_regexIntExtractor.GetLong(priceRaw));
							}

							advertsList.Add(ad);
						}
					}
				}
			}
			catch (Exception ex)
			{
				_logger.Error("AvitoParser", ex);
			}

			return advertsList;
		}
	}
}