using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Univ_Manage.Infrastructure.Models.Supervision;
using Univ_Manage.Infrastructure.Models.Univ;

namespace Univ_Manage.Infrastructure.SqlServer
{
    public class Univ_ManageDBContext : IdentityDbContext<UserSet, RoleSet, int,
    UserClaimSet, UserRoleSet, UserloginSet, RoleClaimSet, UserTokenSet>
    {
        #region DbSets

        #region Security

        //public DbSet<RoleSet> Roles { get; set; }
        //public DbSet<UserRoleSet> UserRoles { get; set; }
        //public DbSet<UserSet> Users { get; set; }
        #endregion

        #region Supervision

        public DbSet<TransactionSet> Transactions { get; set; }
        public DbSet<SettingSet> Settings { get; set; }
        #endregion

        #region Univ
        public DbSet<DepartmetSet> Departments { get; set; }
        public DbSet<ExamSet> Exams { get; set; }
        public DbSet<SchedulingSet> Schedulings { get; set; }
        public DbSet<SemesterSet> Semesters { get; set; }
        public DbSet<SubjectSet> Subjects { get; set; }
        public DbSet<UserSemesterSet> UserSemesters { get; set; }
        public DbSet<YearSet> Years { get; set; }
        #endregion

        #endregion

        #region Constructors

        public Univ_ManageDBContext(DbContextOptions options) : base(options)
        {

        }
        #endregion

       
        #region Methods

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var mutableForeignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                mutableForeignKey.DeleteBehavior = DeleteBehavior.NoAction;
            }

            modelBuilder.Entity<UserRoleSet>(userRole =>
            {
                userRole.HasKey(ur => new { ur.Id });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                modelBuilder.Entity<SettingSet>(s => s.HasIndex(s => s.Name).IsUnique());
            });
            modelBuilder.Entity<RoleClaimSet>().ToTable("RoleClaims", "Security");
            modelBuilder.Entity<RoleSet>().ToTable("Roles", "Security");
            modelBuilder.Entity<UserClaimSet>().ToTable("UserClaims", "Security");
            modelBuilder.Entity<UserloginSet>().ToTable("Userlogins", "Security");
            modelBuilder.Entity<UserRoleSet>().ToTable("UserRoles", "Security");
            modelBuilder.Entity<UserSet>().ToTable("Users", "Security");
            modelBuilder.Entity<UserTokenSet>().ToTable("UserTokens", "Security");

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        #endregion

    }
    public class YourDbContextFactory : IDesignTimeDbContextFactory<Univ_ManageDBContext>
    {
        public Univ_ManageDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<Univ_ManageDBContext>();
            //optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=Univ_Manage_LocalDB;Integrated Security=True");
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Univ_Manage_LocalDB;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new Univ_ManageDBContext(optionsBuilder.Options);
        }
    }
}
