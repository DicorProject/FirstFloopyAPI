using Floppy.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Floppy.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options){}
        public DbSet<Auth> tb_login { get; set; }
        public DbSet<Category> ClassificationMaster { get; set; }   
        public DbSet<SubCategory> ClassificationTrans { get; set; }
        public DbSet<Service> services { get; set; }
        public DbSet<RatingReview> rating_review { get; set; }
        public DbSet<Item> Itemmaster { get; set; }
        public DbSet<Metatag> MetaTags { get; set; }
        public DbSet<HomePageSlider> HomePageSlider { get; set; }   
        public DbSet<VendorRegistration> VendorRegistration {  get; set; }  
        public DbSet<LeadEntryMaster> LeadEntryMaster { get; set; } 
        public DbSet<Url> url { get; set; }    
        public DbSet<LeadTrans> LeadTrans { get; set; }
        public DbSet<Testimonial> TestimonialMaster { get; set; }   
        public DbSet<Footer> Footer { get; set; }
        public DbSet<Parameter> Parameter { get; set; }
        public DbSet<Tax> Tax { get; set; } 
        public DbSet<Cartmaster> cartmaster { get; set; }   
        public DbSet<PackageTrans> PackageTrans { get; set; }
        public DbSet<BlogMaster> BlogMaster { get; set; }   
        public DbSet<BlogTrans> BlogTrans { get; set; } 
        public DbSet<Packagemaster> Packagemaster { get; set; }
        public DbSet<CouponMaster> CouponMaster { get; set; } 
        public DbSet<CouponTrans> CouponTrans { get; set; } 
        public DbSet<RateCard> RateCard { get; set; }   
        public DbSet<TemplateMaster> TemplateMaster { get; set; }   
        public DbSet<SmtpMaster> SmtpMaster { get; set; }   
        public DbSet<AddressMaster> Addressmaster { get; set; } 
        public DbSet<LeadServices> LeadServices { get; set; }   
        public DbSet<LeadVandorTrans> LeadVandorTrans { get; set; } 
        public DbSet<MobileSMSTemplate> MobileSMSTemplate { get; set; }    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Auth>()
                .HasNoKey();
            modelBuilder.Entity<Category>()
                .HasNoKey();
            modelBuilder.Entity<SubCategory>()
                .HasNoKey();
            modelBuilder.Entity<Service>()
                .HasNoKey();
            modelBuilder.Entity<RatingReview>()
                .HasNoKey();
            modelBuilder.Entity<Item>()
                .HasNoKey();
            modelBuilder.Entity<Metatag>()
                .HasNoKey();
            modelBuilder.Entity<HomePageSlider>()
                .HasNoKey();
            modelBuilder.Entity<VendorRegistration>()
                .HasNoKey();
            modelBuilder.Entity<LeadEntryMaster>()
                .HasNoKey();
            modelBuilder.Entity<Url>()
                .HasNoKey();
            modelBuilder.Entity<LeadTrans>()
                .HasNoKey();
            modelBuilder.Entity<Testimonial>()  
                .HasNoKey();
            modelBuilder.Entity<Footer>()
                .HasNoKey();
            modelBuilder.Entity<Parameter>()
                .HasNoKey();
            modelBuilder.Entity<Tax>()
                .HasNoKey();
            modelBuilder.Entity<Cartmaster>()
                .HasNoKey();    
            modelBuilder.Entity<PackageTrans>() 
                .HasNoKey();
            modelBuilder.Entity<BlogMaster>()
                .HasNoKey();
            modelBuilder.Entity<BlogTrans>()
                .HasNoKey();   
            modelBuilder.Entity<Packagemaster>()
                .HasNoKey();   
            modelBuilder.Entity<CouponMaster>()
                .HasNoKey();
            modelBuilder.Entity<CouponTrans>()
                .HasNoKey();
            modelBuilder.Entity<RateCard>()
                .HasNoKey();
            modelBuilder.Entity<TemplateMaster>()
                .HasNoKey();
            modelBuilder.Entity<SmtpMaster>()
                .HasNoKey();
            modelBuilder.Entity<AddressMaster>()
                .HasNoKey();
            modelBuilder.Entity<LeadServices>()
                .HasNoKey();
            modelBuilder.Entity<LeadVandorTrans>()
                .HasNoKey();
            modelBuilder.Entity<MobileSMSTemplate>()
                .HasNoKey();
        }

    }
}
