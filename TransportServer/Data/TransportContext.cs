using Microsoft.EntityFrameworkCore;
using TransportServer.Models;

namespace TransportServer.Data
{
    public class TransportContext : DbContext
    {
        public TransportContext(DbContextOptions<TransportContext> options) :
            base(options)
        { }

        public DbSet<Item> Items { get; set; }
    }
}
