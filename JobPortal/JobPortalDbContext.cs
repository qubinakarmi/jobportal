using Microsoft.EntityFrameworkCore;
using JobPortal.Models;


namespace JobPortal
{
    public class JobPortalDbContext : DbContext
    {
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<User> Users { get; set; }
        public string DbPath { get; }
        public JobPortalDbContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "jobportal_new.db");
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        // use this command to download SqlLite :dotnet add package Microsoft.EntityFrameworkCore.Sqlite
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={DbPath}");
        }
        public DbSet<JobPortal.Models.UserViewModel> UserViewModel { get; set; } = default!;
    }

    public class Job
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }

    public class Organization
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int ContactNo { get; set; }
        public List<Job> Job = new();
    }

    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }

    }
    //Run these commands to ensure the migration
    //dotnet tool install --global dotnet-ef *not required
    //dotnet add package Microsoft.EntityFrameworkCore.Design
    //dotnet ef migrations add InitialCreate
    //dotnet ef database update
    //dotnet ef migrations add yourMigrationName
    //Alternative : Goto PM Console -> Install-Package Microsoft.EntityFrameworkCore.Tools & Add-Migration InitailCreate
}
