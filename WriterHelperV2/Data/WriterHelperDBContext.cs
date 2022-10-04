using Microsoft.EntityFrameworkCore;
using WriterHelperV2.Models.Domain;

namespace WriterHelperV2.Data
{
    public class WriterHelperDBContext : DbContext
    {
        public WriterHelperDBContext(DbContextOptions options) : base(options)
        {
        }


        public DbSet<EntryName> EntryNames { get; set; }
    }
}
