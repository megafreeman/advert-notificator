using System.Data.Entity.Migrations;
using System.Linq;
using AdvertNotificator.DataAccess;
using AdvertNotificator.Models;
using AdvertNotificator.Shared;

namespace AdvertNotificator.Migrations
{
	internal sealed class Configuration : DbMigrationsConfiguration<AdvertsContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = true;
		}

		protected override void Seed(AdvertsContext context)
		{
			context.Channels.AddOrUpdate(c => c.PushallId,
				new Channel
				{
					PushallId = "2170",
					PushallKey = "c63a8a8c72fd9a8f6857085a230449fa",
					Url = "https://pushall.ru/?fs=2170&key=024b41c98d56a1b1ee4118d10ef38481",
					IsAdmin = true
				},
				new Channel
				{
					PushallId = "2301",
					PushallKey = "9a4cef54a41c86a557669bad4d16de94",
					Url = "https://pushall.ru/?fs=2301&key=0d61acf96a2320d4a32a5e16eeaa9edb"
				});

			context.SaveChanges();

			context.Links.AddOrUpdate(l => l.Name,
				new Link
				{
					Name = "Avito_Kld_Flat_Rent",
					Site = Sites.Avito,
					Url =
						"https://www.avito.ru/kaliningrad/kvartiry/sdam/na_dlitelnyy_srok/1-komnatnye?pmax=15000&user=1&q=%D1%81%D0%BE%D0%B1%D1%81%D1%82%D0%B2%D0%B5%D0%BD%D0%BD%D0%B8%D0%BA",
					IsNew = true,
					IsActive = true,
					Channel = context.Channels.First(c => c.PushallId == "2170"),
					Interval = 45
				},
				new Link
				{
					Name = "Avito_Krasnogorsk_Flat_Rent",
					Site = Sites.Avito,
					Url = "https://www.avito.ru/moskovskaya_oblast_krasnogorsk/kvartiry/sdam/2-komnatnye",
					IsNew = true,
					IsActive = true,
					Channel = context.Channels.First(c => c.PushallId == "2301"),
					Interval = 40
				}
				);
		}
	}
}
