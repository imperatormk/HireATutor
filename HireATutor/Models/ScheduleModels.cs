using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Newtonsoft.Json;

namespace HireATutor.Models
{
    public class Timeframe
    {
        [Key]
        [Display(Name = "Timeframe")]
        public int timeframeId { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Time from")]
        public DateTime timeFrom { get; set; }

        public string timeFromOnly
        {
            get
            {
                return timeFrom.ToShortTimeString();
            } 
        }

        public string timeToOnly
        {
            get
            {
                return timeTo.ToShortTimeString();
            }
        }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Time to")]
        public DateTime timeTo { get; set; }
    }

    public class DayOff
    {
        [Key]
        public int dayOffId { get; set; }

        public string instructor_Id { get; set; }

        [ForeignKey("instructor_Id")]
        public Instructor instructor { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        public DateTime dateOff { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Reason")]
        public string reason { get; set; }
    }

    public class Schedule
    {
        [Key]
        public int scheduleId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Schedule date")]
        public DateTime scheduleDate { get; set; }

        public int timeframeId { get; set; }

        [Association("Timeframe", "timeframeId", "timeframeId", IsForeignKey = true)]
        public Timeframe timeframe { get; set; }

        public string studentId { get; set; }

        [Association("Student", "studentId", "Id", IsForeignKey = true)]
        public Student student { get; set; }

        [Required]
        public int gigId { get; set; }

        [Required]
        public int packageType { get; set; }

        [Display(Name = "Package type")]
        public string packageTypeStr
        {
            get
            {
                switch (packageType)
                {
                    case 1:
                        return "Economy";
                    case 2:
                        return "Regular";
                    case 3:
                        return "Exclusive";
                }
                return "";
            } 
        }

        [ForeignKey("gigId")]
        public virtual Gig gig { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date created")]
        public DateTime dateCreated { get; set; }

        public int status { get; set; }
    }

    public partial class ScheduleContext : ApplicationDbContext
    {
        public ScheduleContext() : base()
        {
            Database.SetInitializer<ScheduleContext>(null);
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public virtual DbSet<Timeframe> Timeframes { get; set; }
        public virtual DbSet<DayOff> DaysOff { get; set; }
        public virtual DbSet<Schedule> Schedules { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Timeframe>().ToTable("Timeframes");
            modelBuilder.Entity<DayOff>().ToTable("DaysOff");
            modelBuilder.Entity<Schedule>().ToTable("Schedules");
        }

        public new static ScheduleContext Create()
        {
            return new ScheduleContext();
        }

        public DbQuery<T> Query<T>() where T : class
        {
            return Set<T>().AsNoTracking();
        }
    }

}