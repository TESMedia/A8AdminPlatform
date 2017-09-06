namespace CaptivePortal.API.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class New_Changes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "CreationDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Users", "UpdateDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "BirthDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Users", "UpdateDate", c => c.DateTime(nullable: false));
        }
    }
}
