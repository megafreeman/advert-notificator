using System.ComponentModel.DataAnnotations;

namespace AdvertNotificator.Models
{
	public class Advert : BaseEntity
	{
		[Required]
		public long PublicId { get; set; }

		[Required, MaxLength(250)]
		public string Url { get; set; }

		[Required]
		public int Price { get; set; }

		[Required, MaxLength(250)]
		public string Title { get; set; }

		[MaxLength(12)]
		public string Prefix { get; set; }

		[Required, MaxLength(250)]
		public string Address { get; set; }

		public bool IsNotificated { get; set; }

		[Required]
		public virtual Channel Channel { get; set; }
	}
}