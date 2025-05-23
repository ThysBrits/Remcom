using Microsoft.EntityFrameworkCore;
using Remcom.Api.Models;

namespace Remcom.Api.Data
{
    public class RemcomDbContext : DbContext
    {
        public RemcomDbContext(DbContextOptions<RemcomDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Notice> Notices { get; set; }
        public DbSet<WarrantOfArrest> WarrantsOfArrest { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<ReconciliationFile> ReconciliationFiles { get; set; }
    }
}
