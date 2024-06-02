using Microsoft.EntityFrameworkCore;

namespace SignalRChat.Models
{
    public class MnDbContext : DbContext
    {
        public DbSet<Notice> Notices { get; set; }

        public MnDbContext(DbContextOptions<MnDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 将 Notice 实体映射到 Notice 表
            modelBuilder.Entity<Notice>().ToTable("Notice");
        }
    }
}


