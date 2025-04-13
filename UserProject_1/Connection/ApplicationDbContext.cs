using Microsoft.EntityFrameworkCore;
using UserProject_1.Models;


namespace UserProject_1.Connection
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option)
        {

        }
        
        public DbSet<CustomerDetails> CustDetail {  get; set; }
        public DbSet<CustomerStatus> CustStatus { get; set; }

        // Add the OnModelCreating method here
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerDetails>()
                .HasMany(cd => cd.CustomerStatuses)  // 'CustomerStatuses' should be the property in 'CustomerDetail'
                .WithOne(cs => cs.customerDetails)    // Each 'CustomerStatus' has a navigation property 'CustomerDetail'
                .HasForeignKey(cs => cs.CustomerDetailId)  // Foreign key in 'CustomerStatus'
                .OnDelete(DeleteBehavior.Cascade);  // Cascade delete

            base.OnModelCreating(modelBuilder);
        }

    }
}   
