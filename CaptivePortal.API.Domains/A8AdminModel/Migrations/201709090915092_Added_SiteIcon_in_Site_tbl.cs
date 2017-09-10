namespace CaptivePortal.API.Models.A8AdminModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_SiteIcon_in_Site_tbl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Site", "SiteIconPath", c => c.String(maxLength: 50, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Site", "SiteIconPath");
        }
    }
}
