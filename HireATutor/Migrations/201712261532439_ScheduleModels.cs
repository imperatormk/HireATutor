namespace HireATutor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ScheduleModels : DbMigration
    {
        public override void Up()
        {
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
                .PrimaryKey(t => t.dayOffId)
                .ForeignKey("dbo.AspNetUsers", t => t.instructor_Id)
                .Index(t => t.instructor_Id);
            
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
                .PrimaryKey(t => t.scheduleId)
                .ForeignKey("dbo.Packages", t => new { t.gigId, t.packageType }, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.studentId)
                .ForeignKey("dbo.Timeframes", t => t.timeframeId, cascadeDelete: true)
                .Index(t => t.timeframeId)
                .Index(t => t.studentId)
                .Index(t => new { t.gigId, t.packageType });
            
            CreateTable(
                "dbo.Timeframes",
                c => new
                    {
                        timeframeId = c.Int(nullable: false, identity: true),
                        timeFrom = c.DateTime(nullable: false),
                        timeTo = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.timeframeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Schedules", "timeframeId", "dbo.Timeframes");
            DropForeignKey("dbo.Schedules", "studentId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Schedules", new[] { "gigId", "packageType" }, "dbo.Packages");
            DropForeignKey("dbo.DaysOff", "instructor_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Schedules", new[] { "gigId", "packageType" });
            DropIndex("dbo.Schedules", new[] { "studentId" });
            DropIndex("dbo.Schedules", new[] { "timeframeId" });
            DropIndex("dbo.DaysOff", new[] { "instructor_Id" });
            DropTable("dbo.Timeframes");
            DropTable("dbo.Schedules");
            DropTable("dbo.DaysOff");
        }
    }
}
