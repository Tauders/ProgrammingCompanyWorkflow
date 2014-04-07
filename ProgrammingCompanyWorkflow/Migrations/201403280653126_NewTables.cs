namespace ProgrammingCompanyWorkflow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accesses",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ApplicationRole_Id = c.String(maxLength: 128),
                        FromRole_Id = c.String(maxLength: 128),
                        ToRole_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.ApplicationRole_Id)
                .ForeignKey("dbo.Roles", t => t.FromRole_Id)
                .ForeignKey("dbo.Roles", t => t.ToRole_Id)
                .Index(t => t.ApplicationRole_Id)
                .Index(t => t.FromRole_Id)
                .Index(t => t.ToRole_Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Components",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProjectComponents",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Count = c.Int(nullable: false),
                        Component_Id = c.Guid(),
                        Project_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Components", t => t.Component_Id)
                .ForeignKey("dbo.Projects", t => t.Project_Id)
                .Index(t => t.Component_Id)
                .Index(t => t.Project_Id);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Body = c.String(nullable: false),
                        Comment = c.String(),
                        CreatingTime = c.DateTime(nullable: false),
                        LastModifyTime = c.DateTime(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        CreatingUser_Id = c.String(maxLength: 128),
                        CurrentResponseUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.Users", t => t.CreatingUser_Id)
                .ForeignKey("dbo.Users", t => t.CurrentResponseUser_Id)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.CreatingUser_Id)
                .Index(t => t.CurrentResponseUser_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.UserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.UserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.LoginProvider, t.ProviderKey })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ProjectMovements",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ModifyTime = c.DateTime(nullable: false),
                        FromUser_Id = c.String(maxLength: 128),
                        Project_Id = c.Guid(),
                        ToUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.FromUser_Id)
                .ForeignKey("dbo.Projects", t => t.Project_Id)
                .ForeignKey("dbo.Users", t => t.ToUser_Id)
                .Index(t => t.FromUser_Id)
                .Index(t => t.Project_Id)
                .Index(t => t.ToUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectMovements", "ToUser_Id", "dbo.Users");
            DropForeignKey("dbo.ProjectMovements", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.ProjectMovements", "FromUser_Id", "dbo.Users");
            DropForeignKey("dbo.ProjectComponents", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.Projects", "CurrentResponseUser_Id", "dbo.Users");
            DropForeignKey("dbo.Projects", "CreatingUser_Id", "dbo.Users");
            DropForeignKey("dbo.Projects", "ApplicationUser_Id", "dbo.Users");
            DropForeignKey("dbo.UserClaims", "User_Id", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.UserLogins", "UserId", "dbo.Users");
            DropForeignKey("dbo.ProjectComponents", "Component_Id", "dbo.Components");
            DropForeignKey("dbo.Accesses", "ToRole_Id", "dbo.Roles");
            DropForeignKey("dbo.Accesses", "FromRole_Id", "dbo.Roles");
            DropForeignKey("dbo.Accesses", "ApplicationRole_Id", "dbo.Roles");
            DropIndex("dbo.ProjectMovements", new[] { "ToUser_Id" });
            DropIndex("dbo.ProjectMovements", new[] { "Project_Id" });
            DropIndex("dbo.ProjectMovements", new[] { "FromUser_Id" });
            DropIndex("dbo.ProjectComponents", new[] { "Project_Id" });
            DropIndex("dbo.Projects", new[] { "CurrentResponseUser_Id" });
            DropIndex("dbo.Projects", new[] { "CreatingUser_Id" });
            DropIndex("dbo.Projects", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.UserClaims", new[] { "User_Id" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserLogins", new[] { "UserId" });
            DropIndex("dbo.ProjectComponents", new[] { "Component_Id" });
            DropIndex("dbo.Accesses", new[] { "ToRole_Id" });
            DropIndex("dbo.Accesses", new[] { "FromRole_Id" });
            DropIndex("dbo.Accesses", new[] { "ApplicationRole_Id" });
            DropTable("dbo.ProjectMovements");
            DropTable("dbo.UserRoles");
            DropTable("dbo.UserLogins");
            DropTable("dbo.UserClaims");
            DropTable("dbo.Users");
            DropTable("dbo.Projects");
            DropTable("dbo.ProjectComponents");
            DropTable("dbo.Components");
            DropTable("dbo.Roles");
            DropTable("dbo.Accesses");
        }
    }
}
