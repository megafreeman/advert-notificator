using System.ComponentModel.DataAnnotations;
using AdvertNotificator.Shared;

namespace AdvertNotificator.Models
{
	public class Link : BaseEntity
	{
		[Required, MaxLength(50)]
		public string Name { get; set; }

		[MaxLength(12)]
		public string Prefix { get; set; }

		[Required, MaxLength(250)]
		public string Url { get; set; }

		[Required]
		public int Interval { get; set; }

		[Required]
		public Sites Site { get; set; }

		public bool IsNew { get; set; }

		public bool IsActive { get; set; }

		[Required]
		public virtual Channel Channel { get; set; }
	}
}