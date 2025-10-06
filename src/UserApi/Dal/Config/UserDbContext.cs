using Microsoft.EntityFrameworkCore;
using UserApi.Dal.Models;

namespace UserApi.Dal.Config
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        public DbSet<UserDal> Users { get; set; }
        public DbSet<SupportMetricsDal> SupportMetrics { get; set; }
        public DbSet<UserRoleDal> UserRoles { get; set; }
    }
}
