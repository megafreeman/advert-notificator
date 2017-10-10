using System.Data.Entity;
using AdvertNotificator.Models;

namespace AdvertNotificator.DataAccess
{
	public class AdvertsContext : DbContext
	{
		public AdvertsContext()
			: base("Name=AdvertsDatabaseContext")
		{
		}

		public DbSet<Advert> Adverts { get; set; }
		public DbSet<Channel> Channels { get; set; }
		public DbSet<Link> Links { get; set; }
	}
}