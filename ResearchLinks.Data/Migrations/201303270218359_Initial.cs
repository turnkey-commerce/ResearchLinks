namespace ResearchLinks.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserName = c.String(nullable: false, maxLength: 100),
                        PasswordHash = c.Binary(nullable: false, maxLength: 64),
                        PasswordSalt = c.Binary(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 200),
                        Comment = c.String(maxLength: 200),
                        IsApproved = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastLogin = c.DateTime(),
                        DateLastActivity = c.DateTime(),
                        DateLastPasswordChange = c.DateTime(nullable: false),
                        FirstName = c.String(maxLength: 50),
                        LastName = c.String(maxLength: 50),
                        RegistrationCode = c.String(maxLength: 50),
                        RegistrationCodeExpiration = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserName);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleName = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.RoleName);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        ProjectId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 255),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.ProjectId)
                .ForeignKey("dbo.Users", t => t.UserName, cascadeDelete: true)
                .Index(t => t.UserName);
            
            CreateTable(
                "dbo.ResearchItems",
                c => new
                    {
                        ResearchItemId = c.Int(nullable: false, identity: true),
                        Subject = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 2000),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                        ProjectId = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.ResearchItemId)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserName)
                .Index(t => t.ProjectId)
                .Index(t => t.UserName);
            
            CreateTable(
                "dbo.Links",
                c => new
                    {
                        LinkId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 255),
                        Url = c.String(nullable: false, maxLength: 255),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                        ResearchItemId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LinkId)
                .ForeignKey("dbo.ResearchItems", t => t.ResearchItemId, cascadeDelete: true)
                .Index(t => t.ResearchItemId);
            
            CreateTable(
                "dbo.Notes",
                c => new
                    {
                        NoteId = c.Int(nullable: false, identity: true),
                        Text = c.String(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                        ResearchItemId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.NoteId)
                .ForeignKey("dbo.ResearchItems", t => t.ResearchItemId, cascadeDelete: true)
                .Index(t => t.ResearchItemId);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        ImageId = c.Int(nullable: false, identity: true),
                        Url = c.String(nullable: false, maxLength: 255),
                        Name = c.String(nullable: false, maxLength: 50),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                        ResearchItemId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ImageId)
                .ForeignKey("dbo.ResearchItems", t => t.ResearchItemId, cascadeDelete: true)
                .Index(t => t.ResearchItemId);
            
            CreateTable(
                "dbo.RoleMemberships",
                c => new
                    {
                        UserName = c.String(nullable: false, maxLength: 100),
                        RoleName = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => new { t.UserName, t.RoleName })
                .ForeignKey("dbo.Users", t => t.UserName, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.RoleName, cascadeDelete: true)
                .Index(t => t.UserName)
                .Index(t => t.RoleName);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.RoleMemberships", new[] { "RoleName" });
            DropIndex("dbo.RoleMemberships", new[] { "UserName" });
            DropIndex("dbo.Images", new[] { "ResearchItemId" });
            DropIndex("dbo.Notes", new[] { "ResearchItemId" });
            DropIndex("dbo.Links", new[] { "ResearchItemId" });
            DropIndex("dbo.ResearchItems", new[] { "UserName" });
            DropIndex("dbo.ResearchItems", new[] { "ProjectId" });
            DropIndex("dbo.Projects", new[] { "UserName" });
            DropForeignKey("dbo.RoleMemberships", "RoleName", "dbo.Roles");
            DropForeignKey("dbo.RoleMemberships", "UserName", "dbo.Users");
            DropForeignKey("dbo.Images", "ResearchItemId", "dbo.ResearchItems");
            DropForeignKey("dbo.Notes", "ResearchItemId", "dbo.ResearchItems");
            DropForeignKey("dbo.Links", "ResearchItemId", "dbo.ResearchItems");
            DropForeignKey("dbo.ResearchItems", "UserName", "dbo.Users");
            DropForeignKey("dbo.ResearchItems", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Projects", "UserName", "dbo.Users");
            DropTable("dbo.RoleMemberships");
            DropTable("dbo.Images");
            DropTable("dbo.Notes");
            DropTable("dbo.Links");
            DropTable("dbo.ResearchItems");
            DropTable("dbo.Projects");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
        }
    }
}
