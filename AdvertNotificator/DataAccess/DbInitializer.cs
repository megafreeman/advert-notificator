using System.Data.Entity;
using AdvertNotificator.Migrations;

namespace AdvertNotificator.DataAccess
{
	internal class DbInitializer : MigrateDatabaseToLatestVersion<AdvertsContext, Configuration>
	{
		 
	}
}