using Microsoft.EntityFrameworkCore;
using MiniIdentity.Enterprise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MiniIdentity.Adapters
{
    public class MiniIdentityDbContext : DbContext
    {
        public MiniIdentityDbContext(DbContextOptions<MiniIdentityDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MiniIdentityDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
