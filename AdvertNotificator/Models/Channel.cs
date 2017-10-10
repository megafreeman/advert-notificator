using System.ComponentModel.DataAnnotations;

namespace AdvertNotificator.Models
{
	public class Channel : BaseEntity
	{
		[Required, MaxLength(12)]
		public string PushallId { get; set; }

		[Required, MaxLength(50)]
		public string PushallKey { get; set; }

		[Required, MaxLength(250)]
		public string Url { get; set; }

		public bool IsAdmin { get; set; }
	}
}