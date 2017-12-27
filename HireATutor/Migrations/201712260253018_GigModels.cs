namespace HireATutor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GigModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        catId = c.Int(nullable: false, identity: true),
                        catName = c.String(nullable: false),
                        catDesc = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.catId);
            
            CreateTable(
                "dbo.Gigs",
                c => new
                    {
                        gigId = c.Int(nullable: false, identity: true),
                        catId = c.Int(nullable: false),
                        insId = c.String(maxLength: 128),
                        active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.gigId)
                .ForeignKey("dbo.Categories", t => t.catId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.insId)
                .Index(t => t.catId)
                .Index(t => t.insId);
            
            CreateTable(
                "dbo.Packages",
                c => new
                    {
                        gigId = c.Int(nullable: false),
                        packageType = c.Int(nullable: false),
                        packageDesc = c.String(nullable: false),
                        price = c.String(nullable: false),
                    })
                .PrimaryKey(t => new { t.gigId, t.packageType })
                .ForeignKey("dbo.Gigs", t => t.gigId, cascadeDelete: true)
                .Index(t => t.gigId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Packages", "gigId", "dbo.Gigs");
            DropForeignKey("dbo.Gigs", "instructor_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Gigs", "catId", "dbo.Categories");
            DropIndex("dbo.Packages", new[] { "gigId" });
            DropIndex("dbo.Gigs", new[] { "instructor_Id" });
            DropIndex("dbo.Gigs", new[] { "catId" });
            DropTable("dbo.Packages");
            DropTable("dbo.Gigs");
            DropTable("dbo.Categories");
        }
    }
}
