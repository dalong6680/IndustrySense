using IndustrySense.Server.Infrastructure.Repository.Entity;
using Microsoft.EntityFrameworkCore;
using System;

namespace IndustrySense.Server.Infrastructure.Repository
{
    public class DatabaseContext :DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public DbSet<Device> Devices { get; set; }
        public DbSet<Record> Records { get; set; }

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
