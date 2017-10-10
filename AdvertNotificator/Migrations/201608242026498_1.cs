namespace AdvertNotificator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Adverts",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        PublicId = c.Long(nullable: false),
                        Url = c.String(nullable: false, maxLength: 250),
                        Price = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 250),
                        Prefix = c.String(maxLength: 12),
                        Address = c.String(nullable: false, maxLength: 250),
                        IsNotificated = c.Boolean(nullable: false),
						CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2", defaultValueSql: "GETUTCDATE()"),
                        Channel_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Channels", t => t.Channel_Id, cascadeDelete: true)
                .Index(t => t.Channel_Id);
            
            CreateTable(
                "dbo.Channels",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        PushallId = c.String(nullable: false, maxLength: 12),
                        PushallKey = c.String(nullable: false, maxLength: 50),
                        Url = c.String(nullable: false, maxLength: 250),
                        IsAdmin = c.Boolean(nullable: false),
						CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2", defaultValueSql: "GETUTCDATE()"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Links",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Prefix = c.String(nullable: false, maxLength: 12),
                        Url = c.String(nullable: false, maxLength: 50),
                        Interval = c.Int(nullable: false),
                        Site = c.Int(nullable: false),
                        IsNew = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
						CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2", defaultValueSql: "GETUTCDATE()"),
                        Channel_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Channels", t => t.Channel_Id, cascadeDelete: true)
                .Index(t => t.Channel_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Links", "Channel_Id", "dbo.Channels");
            DropForeignKey("dbo.Adverts", "Channel_Id", "dbo.Channels");
            DropIndex("dbo.Links", new[] { "Channel_Id" });
            DropIndex("dbo.Adverts", new[] { "Channel_Id" });
            DropTable("dbo.Links");
            DropTable("dbo.Channels");
            DropTable("dbo.Adverts");
        }
    }
}
