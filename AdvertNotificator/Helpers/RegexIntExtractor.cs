using System.Text.RegularExpressions;

namespace AdvertNotificator.Helpers
{
	public class RegexIntExtractor
	{
		public long GetLong(string raw)
		{
			long retVal = 0;
			string str = string.Empty;

			Match match = Regex.Match(raw, @"\d");

			while (true)
			{
				if (match.Success)
				{
					str += match.Value;
					match = match.NextMatch();
				}
				else break;
			}

			if (!string.IsNullOrWhiteSpace(str))
			{
				long.TryParse(str, out retVal);
				//retVal = Convert.ToInt32(str);
			}

			return retVal;
		}
	}
}