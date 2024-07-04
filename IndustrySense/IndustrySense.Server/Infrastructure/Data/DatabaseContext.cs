using IndustrySense.Server.Infrastructure.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System;

namespace IndustrySense.Server.Infrastructure.Data
{
    public class DatabaseContext :DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public DbSet<Device> Device { get; set; }
        public DbSet<Record> Record { get; set; }
        public DbSet<ParsingRule> ParsingRule { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 设置数据表之间的关系
            modelBuilder.Entity<Record>()
                .HasOne(r => r.Device)
                .WithMany()
                .HasForeignKey(r => r.DeviceId);
        }
    }
}
