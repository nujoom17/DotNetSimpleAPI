using Microsoft.EntityFrameworkCore;
using WebApplicationTest.Entities;

public class DbContextMain : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<MSTR_UserType> MSTR_UserTypes { get; set; }

    public DbSet<MSTR_Profession> Professions { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<UserProfessionCompany> UserProfessionCompanies { get; set; }

    public DbSet<MSTR_Location> MSTR_Locations { get; set; }



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();
            var connectionString = configuration.GetConnectionString("DbCoreConnectionString");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure many-to-many relationship
        modelBuilder.Entity<UserProfessionCompany>()
            .HasKey(upc => new { upc.UserId, upc.ProfessionId, upc.CompanyId });

        modelBuilder.Entity<UserProfessionCompany>()
            .HasOne(upc => upc.User)
            .WithMany(u => u.UserProfessionCompanies)
            .HasForeignKey(upc => upc.UserId);

        modelBuilder.Entity<UserProfessionCompany>()
            .HasOne(upc => upc.Profession)
            .WithMany(p => p.UserProfessionCompanies)
            .HasForeignKey(upc => upc.ProfessionId);

        modelBuilder.Entity<UserProfessionCompany>()
            .HasOne(upc => upc.Company)
            .WithMany(c => c.UserProfessionCompanies)
            .HasForeignKey(upc => upc.CompanyId);
    }



}