using LanterneRouge.Fresno.Core.Entity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace LanterneRouge.Fresno.Core.Infrastructure
{
    public class StepTestContext : DbContext
    {
        public StepTestContext(IDbConnection connection)
        {
            Connection = connection;
        }

        private IDbConnection Connection { get; }

        public DbSet<User> Users { get; set; }

        public DbSet<StepTest> StepTests { get; set; }

        public DbSet<Measurement> Measurements { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (Connection is DbConnection newConnection)
            {
                optionsBuilder.UseSqlite(newConnection);
            }
        }
    }
}
