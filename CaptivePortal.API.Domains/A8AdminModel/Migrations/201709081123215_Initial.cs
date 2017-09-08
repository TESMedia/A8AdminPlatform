namespace CaptivePortal.API.Models.A8AdminModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdminSiteAccess",
                c => new
                    {
                        AdminSiteAccessId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AdminSiteAccessId)
                .ForeignKey("dbo.Site", t => t.SiteId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.SiteId);
            
            CreateTable(
                "dbo.Site",
                c => new
                    {
                        SiteId = c.Int(nullable: false, identity: true),
                        SiteName = c.String(unicode: false),
                        CompanyId = c.Int(),
                        AutoLogin = c.Boolean(),
                        ControllerIpAddress = c.String(unicode: false),
                        MySqlIpAddress = c.String(unicode: false),
                        Term_conditions = c.String(unicode: false),
                        TermsAndCondDoc = c.String(unicode: false),
                        RtlsUrl = c.String(unicode: false),
                        DashboardUrl = c.String(unicode: false),
                        IsRtls = c.Boolean(),
                        IsLocationDashboard = c.Boolean(),
                        IsCaptivePortal = c.Boolean(),
                    })
                .PrimaryKey(t => t.SiteId)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.Company",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(unicode: false),
                        OrganisationId = c.Int(),
                        CompanyIcon = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Organisation", t => t.OrganisationId)
                .Index(t => t.OrganisationId);
            
            CreateTable(
                "dbo.Organisation",
                c => new
                    {
                        OrganisationId = c.Int(nullable: false, identity: true),
                        OrganisationName = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.OrganisationId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 256, storeType: "nvarchar"),
                        Email = c.String(maxLength: 256, storeType: "nvarchar"),
                        FirstName = c.String(unicode: false),
                        LastName = c.String(maxLength: 50, storeType: "nvarchar"),
                        CreationDate = c.DateTime(nullable: false, precision: 0),
                        UpdateDate = c.DateTime(nullable: false, precision: 0),
                        BirthDate = c.DateTime(nullable: false, precision: 0),
                        MobileNumer = c.Int(),
                        GenderId = c.Int(),
                        AgeId = c.Int(),
                        SiteId = c.Int(),
                        GroupId = c.Int(),
                        promotional_email = c.Boolean(),
                        ThirdPartyOptIn = c.Boolean(),
                        UserOfDataOptIn = c.Boolean(),
                        AutoLogin = c.Boolean(),
                        Status = c.String(maxLength: 50, storeType: "nvarchar"),
                        Custom1 = c.String(maxLength: 50, storeType: "nvarchar"),
                        Custom2 = c.String(maxLength: 50, storeType: "nvarchar"),
                        Custom3 = c.String(maxLength: 50, storeType: "nvarchar"),
                        Custom4 = c.String(maxLength: 50, storeType: "nvarchar"),
                        Custom5 = c.String(maxLength: 50, storeType: "nvarchar"),
                        Custom6 = c.String(maxLength: 50, storeType: "nvarchar"),
                        UniqueUserId = c.String(unicode: false),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(unicode: false),
                        SecurityStamp = c.String(unicode: false),
                        PhoneNumber = c.String(unicode: false),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(precision: 0),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Age", t => t.AgeId)
                .ForeignKey("dbo.Gender", t => t.GenderId)
                .ForeignKey("dbo.Group", t => t.GroupId)
                .ForeignKey("dbo.Site", t => t.SiteId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.GenderId)
                .Index(t => t.AgeId)
                .Index(t => t.SiteId)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.Age",
                c => new
                    {
                        AgeId = c.Int(nullable: false, identity: true),
                        Value = c.String(maxLength: 50, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.AgeId);
            
            CreateTable(
                "dbo.UserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ClaimType = c.String(unicode: false),
                        ClaimValue = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Gender",
                c => new
                    {
                        GenderId = c.Int(nullable: false, identity: true),
                        Value = c.String(maxLength: 50, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.GenderId);
            
            CreateTable(
                "dbo.Group",
                c => new
                    {
                        GroupId = c.Int(nullable: false, identity: true),
                        GroupName = c.String(unicode: false),
                        Rule = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.GroupId);
            
            CreateTable(
                "dbo.UserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        ProviderKey = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
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
                "dbo.AppLog",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false, precision: 0),
                        Thread = c.String(unicode: false),
                        Level = c.String(unicode: false),
                        Logger = c.String(unicode: false),
                        Message = c.String(unicode: false),
                        Exception = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TrackMember",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MacAddress = c.String(unicode: false),
                        VisitedDateTime = c.DateTime(nullable: false, precision: 0),
                        PostDateTime = c.DateTime(nullable: false, precision: 0),
                        RecieveDateTime = c.DateTime(nullable: false, precision: 0),
                        AreaName = c.String(unicode: false),
                        X = c.Int(nullable: false),
                        Y = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Device",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Mac = c.String(unicode: false),
                        Intstatus = c.Int(nullable: false),
                        IsCreatedByAdmin = c.Boolean(nullable: false),
                        CreatedDateTime = c.DateTime(nullable: false, precision: 0),
                        IsAdminSelected = c.Boolean(nullable: false),
                        IsRegisterInRtls = c.Boolean(nullable: false),
                        IsTracking = c.Boolean(nullable: false),
                        IsDisplay = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Form",
                c => new
                    {
                        FormId = c.Int(nullable: false, identity: true),
                        FormName = c.String(unicode: false),
                        SiteId = c.Int(nullable: false),
                        BannerIcon = c.String(unicode: false),
                        BackGroundColor = c.String(unicode: false),
                        LoginWindowColor = c.String(unicode: false),
                        IsPasswordRequire = c.Boolean(nullable: false),
                        LoginPageTitle = c.String(unicode: false),
                        RegistrationPageTitle = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.FormId)
                .ForeignKey("dbo.Site", t => t.SiteId, cascadeDelete: true)
                .Index(t => t.SiteId);
            
            CreateTable(
                "dbo.FormControl",
                c => new
                    {
                        FormControlId = c.Int(nullable: false, identity: true),
                        FormId = c.Int(nullable: false),
                        LabelName = c.String(unicode: false),
                        LabelNameToDisplay = c.String(unicode: false),
                        IsMandetory = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.FormControlId)
                .ForeignKey("dbo.Form", t => t.FormId, cascadeDelete: true)
                .Index(t => t.FormId);
            
            CreateTable(
                "dbo.LocationData",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        mac = c.String(unicode: false),
                        sequence = c.String(unicode: false),
                        sn = c.String(unicode: false),
                        bn = c.String(unicode: false),
                        fn = c.String(unicode: false),
                        x = c.Int(nullable: false),
                        y = c.Int(nullable: false),
                        z = c.Int(nullable: false),
                        last_seen_ts = c.DateTime(nullable: false, precision: 0),
                        action = c.String(unicode: false),
                        fix_result = c.String(unicode: false),
                        AreaName = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MacAddress",
                c => new
                    {
                        MacId = c.Int(nullable: false, identity: true),
                        MacAddressValue = c.String(maxLength: 20, storeType: "nvarchar"),
                        UserId = c.Int(nullable: false),
                        BrowserName = c.String(unicode: false),
                        OperatingSystem = c.String(unicode: false),
                        IsMobile = c.Boolean(nullable: false),
                        UserAgentName = c.String(unicode: false),
                        IsRegisterInRtls = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.MacId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ManagePromotion",
                c => new
                    {
                        ManagePromotionId = c.Int(nullable: false, identity: true),
                        SiteId = c.Int(nullable: false),
                        SuccessPageOption = c.String(unicode: false),
                        WebPageURL = c.String(unicode: false),
                        OptionalPictureForSuccessPage = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.ManagePromotionId)
                .ForeignKey("dbo.Site", t => t.SiteId, cascadeDelete: true)
                .Index(t => t.SiteId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(unicode: false),
                        Name = c.String(nullable: false, maxLength: 256, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.RtlsConfiguration",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FattiEngineSiteName = c.String(unicode: false),
                        FattiEngineBuildingName = c.String(unicode: false),
                        SiteId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Site", t => t.SiteId)
                .Index(t => t.SiteId);
            
            CreateTable(
                "dbo.UsersAddress",
                c => new
                    {
                        AddressId = c.Int(nullable: false, identity: true),
                        Address1 = c.String(unicode: false),
                        Address2 = c.String(unicode: false),
                        PostTown = c.String(unicode: false),
                        County = c.String(unicode: false),
                        State = c.String(unicode: false),
                        Country = c.String(unicode: false),
                        PostCode = c.String(unicode: false),
                        Notes = c.String(unicode: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AddressId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ApiAccessUserSession",
                c => new
                    {
                        UserSessionId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        SessionId = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.UserSessionId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApiAccessUserSession", "UserId", "dbo.Users");
            DropForeignKey("dbo.UsersAddress", "UserId", "dbo.Users");
            DropForeignKey("dbo.RtlsConfiguration", "SiteId", "dbo.Site");
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.ManagePromotion", "SiteId", "dbo.Site");
            DropForeignKey("dbo.MacAddress", "UserId", "dbo.Users");
            DropForeignKey("dbo.FormControl", "FormId", "dbo.Form");
            DropForeignKey("dbo.Form", "SiteId", "dbo.Site");
            DropForeignKey("dbo.AdminSiteAccess", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "SiteId", "dbo.Site");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserLogins", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "GroupId", "dbo.Group");
            DropForeignKey("dbo.Users", "GenderId", "dbo.Gender");
            DropForeignKey("dbo.UserClaims", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "AgeId", "dbo.Age");
            DropForeignKey("dbo.AdminSiteAccess", "SiteId", "dbo.Site");
            DropForeignKey("dbo.Site", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.Company", "OrganisationId", "dbo.Organisation");
            DropIndex("dbo.ApiAccessUserSession", new[] { "UserId" });
            DropIndex("dbo.UsersAddress", new[] { "UserId" });
            DropIndex("dbo.RtlsConfiguration", new[] { "SiteId" });
            DropIndex("dbo.Roles", "RoleNameIndex");
            DropIndex("dbo.ManagePromotion", new[] { "SiteId" });
            DropIndex("dbo.MacAddress", new[] { "UserId" });
            DropIndex("dbo.FormControl", new[] { "FormId" });
            DropIndex("dbo.Form", new[] { "SiteId" });
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.UserLogins", new[] { "UserId" });
            DropIndex("dbo.UserClaims", new[] { "UserId" });
            DropIndex("dbo.Users", new[] { "GroupId" });
            DropIndex("dbo.Users", new[] { "SiteId" });
            DropIndex("dbo.Users", new[] { "AgeId" });
            DropIndex("dbo.Users", new[] { "GenderId" });
            DropIndex("dbo.Users", "UserNameIndex");
            DropIndex("dbo.Company", new[] { "OrganisationId" });
            DropIndex("dbo.Site", new[] { "CompanyId" });
            DropIndex("dbo.AdminSiteAccess", new[] { "SiteId" });
            DropIndex("dbo.AdminSiteAccess", new[] { "UserId" });
            DropTable("dbo.ApiAccessUserSession");
            DropTable("dbo.UsersAddress");
            DropTable("dbo.RtlsConfiguration");
            DropTable("dbo.Roles");
            DropTable("dbo.ManagePromotion");
            DropTable("dbo.MacAddress");
            DropTable("dbo.LocationData");
            DropTable("dbo.FormControl");
            DropTable("dbo.Form");
            DropTable("dbo.Device");
            DropTable("dbo.TrackMember");
            DropTable("dbo.AppLog");
            DropTable("dbo.UserRoles");
            DropTable("dbo.UserLogins");
            DropTable("dbo.Group");
            DropTable("dbo.Gender");
            DropTable("dbo.UserClaims");
            DropTable("dbo.Age");
            DropTable("dbo.Users");
            DropTable("dbo.Organisation");
            DropTable("dbo.Company");
            DropTable("dbo.Site");
            DropTable("dbo.AdminSiteAccess");
        }
    }
}
