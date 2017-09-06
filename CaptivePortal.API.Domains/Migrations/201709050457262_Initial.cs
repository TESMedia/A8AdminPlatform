namespace CaptivePortal.API.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdminSiteAccesses",
                c => new
                    {
                        AdminSiteAccessId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AdminSiteAccessId)
                .ForeignKey("dbo.Sites", t => t.SiteId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.SiteId);
            
            CreateTable(
                "dbo.Sites",
                c => new
                    {
                        SiteId = c.Int(nullable: false, identity: true),
                        SiteName = c.String(),
                        CompanyId = c.Int(),
                        AutoLogin = c.Boolean(),
                        ControllerIpAddress = c.String(),
                        MySqlIpAddress = c.String(),
                        Term_conditions = c.String(),
                        TermsAndCondDoc = c.String(),
                        RtlsUrl = c.String(),
                        DashboardUrl = c.String(),
                        IsRtls = c.Boolean(),
                        IsLocationDashboard = c.Boolean(),
                        IsCaptivePortal = c.Boolean(),
                    })
                .PrimaryKey(t => t.SiteId)
                .ForeignKey("dbo.Companies", t => t.CompanyId)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(),
                        OrganisationId = c.Int(),
                        CompanyIcon = c.String(),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Organisations", t => t.OrganisationId)
                .Index(t => t.OrganisationId);
            
            CreateTable(
                "dbo.Organisations",
                c => new
                    {
                        OrganisationId = c.Int(nullable: false, identity: true),
                        OrganisationName = c.String(),
                    })
                .PrimaryKey(t => t.OrganisationId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Email = c.String(maxLength: 256),
                        FirstName = c.String(),
                        LastName = c.String(maxLength: 50),
                        CreationDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        BirthDate = c.DateTime(nullable: false),
                        MobileNumer = c.Int(),
                        GenderId = c.Int(),
                        AgeId = c.Int(),
                        SiteId = c.Int(),
                        GroupId = c.Int(),
                        promotional_email = c.Boolean(),
                        ThirdPartyOptIn = c.Boolean(),
                        UserOfDataOptIn = c.Boolean(),
                        AutoLogin = c.Boolean(),
                        Status = c.String(maxLength: 50),
                        Custom1 = c.String(maxLength: 50),
                        Custom2 = c.String(maxLength: 50),
                        Custom3 = c.String(maxLength: 50),
                        Custom4 = c.String(maxLength: 50),
                        Custom5 = c.String(maxLength: 50),
                        Custom6 = c.String(maxLength: 50),
                        UniqueUserId = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ages", t => t.AgeId)
                .ForeignKey("dbo.Genders", t => t.GenderId)
                .ForeignKey("dbo.Groups", t => t.GroupId)
                .ForeignKey("dbo.Sites", t => t.SiteId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.GenderId)
                .Index(t => t.AgeId)
                .Index(t => t.SiteId)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.Ages",
                c => new
                    {
                        AgeId = c.Int(nullable: false, identity: true),
                        Value = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.AgeId);
            
            CreateTable(
                "dbo.UserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Genders",
                c => new
                    {
                        GenderId = c.Int(nullable: false, identity: true),
                        Value = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.GenderId);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        GroupId = c.Int(nullable: false, identity: true),
                        GroupName = c.String(),
                        Rule = c.String(),
                    })
                .PrimaryKey(t => t.GroupId);
            
            CreateTable(
                "dbo.UserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Forms",
                c => new
                    {
                        FormId = c.Int(nullable: false, identity: true),
                        FormName = c.String(),
                        SiteId = c.Int(nullable: false),
                        BannerIcon = c.String(),
                        BackGroundColor = c.String(),
                        LoginWindowColor = c.String(),
                        IsPasswordRequire = c.Boolean(nullable: false),
                        LoginPageTitle = c.String(),
                        RegistrationPageTitle = c.String(),
                    })
                .PrimaryKey(t => t.FormId)
                .ForeignKey("dbo.Sites", t => t.SiteId, cascadeDelete: true)
                .Index(t => t.SiteId);
            
            CreateTable(
                "dbo.FormControls",
                c => new
                    {
                        FormControlId = c.Int(nullable: false, identity: true),
                        FormId = c.Int(nullable: false),
                        LabelName = c.String(),
                        LabelNameToDisplay = c.String(),
                        IsMandetory = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.FormControlId)
                .ForeignKey("dbo.Forms", t => t.FormId, cascadeDelete: true)
                .Index(t => t.FormId);
            
            CreateTable(
                "dbo.MacAddresses",
                c => new
                    {
                        MacId = c.Int(nullable: false, identity: true),
                        MacAddressValue = c.String(maxLength: 20),
                        UserId = c.Int(nullable: false),
                        BrowserName = c.String(),
                        OperatingSystem = c.String(),
                        IsMobile = c.Boolean(nullable: false),
                        UserAgentName = c.String(),
                        IsRegisterInRtls = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.MacId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ManagePromotions",
                c => new
                    {
                        ManagePromotionId = c.Int(nullable: false, identity: true),
                        SiteId = c.Int(nullable: false),
                        SuccessPageOption = c.String(),
                        WebPageURL = c.String(),
                        OptionalPictureForSuccessPage = c.String(),
                    })
                .PrimaryKey(t => t.ManagePromotionId)
                .ForeignKey("dbo.Sites", t => t.SiteId, cascadeDelete: true)
                .Index(t => t.SiteId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.UsersAddresses",
                c => new
                    {
                        AddressId = c.Int(nullable: false, identity: true),
                        Address1 = c.String(),
                        Address2 = c.String(),
                        PostTown = c.String(),
                        County = c.String(),
                        State = c.String(),
                        Country = c.String(),
                        PostCode = c.String(),
                        Notes = c.String(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AddressId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ApiAccessUserSessions",
                c => new
                    {
                        UserSessionId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        SessionId = c.String(),
                    })
                .PrimaryKey(t => t.UserSessionId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApiAccessUserSessions", "UserId", "dbo.Users");
            DropForeignKey("dbo.UsersAddresses", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.ManagePromotions", "SiteId", "dbo.Sites");
            DropForeignKey("dbo.MacAddresses", "UserId", "dbo.Users");
            DropForeignKey("dbo.FormControls", "FormId", "dbo.Forms");
            DropForeignKey("dbo.Forms", "SiteId", "dbo.Sites");
            DropForeignKey("dbo.AdminSiteAccesses", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "SiteId", "dbo.Sites");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserLogins", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.Users", "GenderId", "dbo.Genders");
            DropForeignKey("dbo.UserClaims", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "AgeId", "dbo.Ages");
            DropForeignKey("dbo.AdminSiteAccesses", "SiteId", "dbo.Sites");
            DropForeignKey("dbo.Sites", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.Companies", "OrganisationId", "dbo.Organisations");
            DropIndex("dbo.ApiAccessUserSessions", new[] { "UserId" });
            DropIndex("dbo.UsersAddresses", new[] { "UserId" });
            DropIndex("dbo.Roles", "RoleNameIndex");
            DropIndex("dbo.ManagePromotions", new[] { "SiteId" });
            DropIndex("dbo.MacAddresses", new[] { "UserId" });
            DropIndex("dbo.FormControls", new[] { "FormId" });
            DropIndex("dbo.Forms", new[] { "SiteId" });
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.UserLogins", new[] { "UserId" });
            DropIndex("dbo.UserClaims", new[] { "UserId" });
            DropIndex("dbo.Users", new[] { "GroupId" });
            DropIndex("dbo.Users", new[] { "SiteId" });
            DropIndex("dbo.Users", new[] { "AgeId" });
            DropIndex("dbo.Users", new[] { "GenderId" });
            DropIndex("dbo.Users", "UserNameIndex");
            DropIndex("dbo.Companies", new[] { "OrganisationId" });
            DropIndex("dbo.Sites", new[] { "CompanyId" });
            DropIndex("dbo.AdminSiteAccesses", new[] { "SiteId" });
            DropIndex("dbo.AdminSiteAccesses", new[] { "UserId" });
            DropTable("dbo.ApiAccessUserSessions");
            DropTable("dbo.UsersAddresses");
            DropTable("dbo.Roles");
            DropTable("dbo.ManagePromotions");
            DropTable("dbo.MacAddresses");
            DropTable("dbo.FormControls");
            DropTable("dbo.Forms");
            DropTable("dbo.UserRoles");
            DropTable("dbo.UserLogins");
            DropTable("dbo.Groups");
            DropTable("dbo.Genders");
            DropTable("dbo.UserClaims");
            DropTable("dbo.Ages");
            DropTable("dbo.Users");
            DropTable("dbo.Organisations");
            DropTable("dbo.Companies");
            DropTable("dbo.Sites");
            DropTable("dbo.AdminSiteAccesses");
        }
    }
}
