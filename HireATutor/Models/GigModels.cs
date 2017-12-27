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
    public class Category
    {
        [Key]
        public int catId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Category name")]
        public string catName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Category description")]
        public string catDesc { get; set; }

        public ICollection<Gig> Gigs { get; set; }
    }

    public class Gig
    {
        [Key]
        public int gigId { get; set; }

        [Required]
        public int catId { get; set; }

        [Association("Category", "catId", "catId", IsForeignKey =true)]
        public Category category {get;set;}

        [Required]
        [Display(Name = "Gig name")]
        public string gigName { get; set; }

        [Required]
        [Display(Name = "Gig description")]
        public string gigDesc { get; set; }

        [Required]
        public string instructor_Id { get; set; }

        [ForeignKey("instructor_Id")]
        public Instructor instructor { get; set; }

        public bool active { get; set; }

        public virtual ICollection<Package> Packages {
            get; set;
        }

        public virtual Package PackageA{
            get
            {
                try
                {
                    IEnumerator<Package> en = Packages.GetEnumerator();
                    en.MoveNext();
                    Package p = en.Current;
                    en.Reset();
                    return p;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return new Package();
                }
            }
        }

        public virtual Package PackageB
        {
            get
            {
                try
                {
                    IEnumerator<Package> en = Packages.GetEnumerator();
                    en.MoveNext();
                    en.MoveNext();
                    Package p = en.Current;
                    en.Reset();
                    return p;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return new Package();
                }
            }
        }

        public virtual Package PackageC
        {
            get
            {
                try
                {
                    IEnumerator<Package> en = Packages.GetEnumerator();
                    en.MoveNext();
                    en.MoveNext();
                    en.MoveNext();
                    Package p = en.Current;
                    en.Reset();
                    return p;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return new Package();
                }
            }
        }
    }

    public class Package
    {
        [Key]
        [Column(Order = 1)]
        public int gigId { get; set; }

        [ForeignKey("gigId")]
        [JsonIgnore]
        public virtual Gig gig { get; set; }

        [Key]
        [Column(Order = 2)]
        [Display(Name = "Package type")]
        public int packageType { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Package description")]
        public string packageDesc { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Package price")]
        public int price { get; set; }
    }

    public partial class GigContext : ApplicationDbContext
    {
        public GigContext() : base()
        {
            Database.SetInitializer<GigContext>(null);
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Gig> Gigs { get; set; }
        public virtual DbSet<Package> Packages { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().ToTable("Categories");
            modelBuilder.Entity<Gig>().ToTable("Gigs");
            modelBuilder.Entity<Package>().ToTable("Packages");

            modelBuilder.Entity<Category>()
                .HasMany<Gig>(g => g.Gigs)
                .WithRequired(s => s.category)
                .HasForeignKey<int>(s => s.catId);

            modelBuilder.Entity<Gig>()
                .HasMany<Package>(g => g.Packages)
                .WithRequired(s => s.gig)
                .HasForeignKey<int>(x => x.gigId);
        }

        public new static GigContext Create()
        {
            return new GigContext();
        }

        public DbQuery<T> Query<T>() where T : class
        {
            return Set<T>().AsNoTracking();
        }

        public System.Data.Entity.DbSet<HireATutor.Models.Schedule> Schedules { get; set; }

        public System.Data.Entity.DbSet<HireATutor.Models.Student> ApplicationUsers { get; set; }

        public System.Data.Entity.DbSet<HireATutor.Models.Timeframe> Timeframes { get; set; }
    }

}