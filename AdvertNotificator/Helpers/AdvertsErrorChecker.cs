using System;
using System.Collections.Generic;
using System.Linq;
using AdvertNotificator.Models;

namespace AdvertNotificator.Helpers
{
	public class AdvertsErrorChecker
	{
		public List<Advert> CheckList(List<Advert> adverts)
		{
			List<Advert> checkedAdverts = adverts.Where(_checkPredicate).Select(a => a).ToList();

			return checkedAdverts;
		}

		private readonly Func<Advert, bool> _checkPredicate = advert =>
		{
			if (advert.PublicId <= 0)
				return false;

			if (string.IsNullOrWhiteSpace(advert.Url))
				return false;

			if (advert.Price <= 0)
				return false;

			if (string.IsNullOrWhiteSpace(advert.Title))
				return false;

			if (string.IsNullOrWhiteSpace(advert.Address))
				return false;

			return true;
		};
	}
}