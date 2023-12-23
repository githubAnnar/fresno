using LanterneRouge.Fresno.Core.Entity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace LanterneRouge.Fresno.Core.Infrastructure
{
    public class StepTestContext : DbContext
    {
        public StepTestContext()
        { }

        public DbSet<User> Users { get; set; }

        public DbSet<StepTest> StepTests { get; set; }

        public DbSet<Measurement> Measurements { get; set; }

        public IDbConnection? Connection { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (Connection is DbConnection newConnection)
            {
                optionsBuilder.UseSqlite(newConnection);
            }

            else
            {
                optionsBuilder.UseSqlite();
            }
        }
    }
}
