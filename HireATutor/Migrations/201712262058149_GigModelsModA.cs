namespace HireATutor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GigModelsModA : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Gigs", "gigName", c => c.String(nullable: false));
            AddColumn("dbo.Gigs", "gigDesc", c => c.String(nullable: false));
            AlterColumn("dbo.Packages", "price", c => c.Int(nullable: false));
            AlterColumn("dbo.Gigs", "insId", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Timeframes",
                c => new
                    {
                        timeframeId = c.Int(nullable: false, identity: true),
                        timeFrom = c.DateTime(nullable: false),
                        timeTo = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.timeframeId);
            
            CreateTable(
                "dbo.Schedules",
                c => new
                    {
                        scheduleId = c.Int(nullable: false, identity: true),
                        scheduleDate = c.DateTime(nullable: false),
                        timeframeId = c.Int(nullable: false),
                        studentId = c.String(maxLength: 128),
                        gigId = c.Int(nullable: false),
                        packageType = c.Int(nullable: false),
                        dateCreated = c.DateTime(nullable: false),
                        status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.scheduleId);
            
            CreateTable(
                "dbo.DaysOff",
                c => new
                    {
                        dayOffId = c.Int(nullable: false, identity: true),
                        insId = c.String(),
                        dateOff = c.DateTime(nullable: false),
                        reason = c.String(nullable: false),
                        instructor_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.dayOffId);
            
            AlterColumn("dbo.Gigs", "insId", c => c.String());
            AlterColumn("dbo.Packages", "price", c => c.String(nullable: false));
            DropColumn("dbo.Gigs", "gigDesc");
            DropColumn("dbo.Gigs", "gigName");
            CreateIndex("dbo.Schedules", new[] { "gigId", "packageType" });
            CreateIndex("dbo.Schedules", "studentId");
            CreateIndex("dbo.Schedules", "timeframeId");
            CreateIndex("dbo.DaysOff", "instructor_Id");
            AddForeignKey("dbo.Schedules", "timeframeId", "dbo.Timeframes", "timeframeId", cascadeDelete: true);
            AddForeignKey("dbo.Schedules", "studentId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Schedules", new[] { "gigId", "packageType" }, "dbo.Packages", new[] { "gigId", "packageType" }, cascadeDelete: true);
            AddForeignKey("dbo.DaysOff", "instructor_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
