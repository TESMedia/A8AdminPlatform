namespace CaptivePortal.API.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class New_Changes_As_Per : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "BirthDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "BirthDate", c => c.DateTime(nullable: false));
        }
    }
}
