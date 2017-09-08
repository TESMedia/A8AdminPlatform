namespace CaptivePortal.API.Models.A8AdminModel.Migrations.A8AdminDbContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RtlsConfigurations", "FattiEngineSiteName", c => c.String(unicode: false));
            AddColumn("dbo.RtlsConfigurations", "FattiEngineBuildingName", c => c.String(unicode: false));
            AddColumn("dbo.RtlsConfigurations", "SiteId", c => c.Int());
            CreateIndex("dbo.RtlsConfigurations", "SiteId");
            AddForeignKey("dbo.RtlsConfigurations", "SiteId", "dbo.Sites", "SiteId");
            DropColumn("dbo.RtlsConfigurations", "Key");
            DropColumn("dbo.RtlsConfigurations", "Value");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RtlsConfigurations", "Value", c => c.String(unicode: false));
            AddColumn("dbo.RtlsConfigurations", "Key", c => c.String(unicode: false));
            DropForeignKey("dbo.RtlsConfigurations", "SiteId", "dbo.Sites");
            DropIndex("dbo.RtlsConfigurations", new[] { "SiteId" });
            DropColumn("dbo.RtlsConfigurations", "SiteId");
            DropColumn("dbo.RtlsConfigurations", "FattiEngineBuildingName");
            DropColumn("dbo.RtlsConfigurations", "FattiEngineSiteName");
        }
    }
}
