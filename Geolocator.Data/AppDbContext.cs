using Geolocator.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Geolocator.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
    {
    }

    public DbSet<IpInfo> IpInfos { get; set; }
}