using Geolocator.Data.Abstractions;
using Geolocator.Data.Models;

namespace Geolocator.Data.Repositories;

public class IpInfoRepository : IIpInfoRepository
{
    private readonly AppDbContext _context;

    public IpInfoRepository(AppDbContext context)
    {
        _context = context;
    }

    public IpInfo Create(IpInfo ipInfo)
    {
        var entityEntry = _context.IpInfos.Add(ipInfo);
        return entityEntry.Entity;
    }

    public void Delete(IpInfo ipInfo)
    {
        _context.IpInfos.Remove(ipInfo);
    }

    public IEnumerable<IpInfo> GetAll()
    {
        return _context.IpInfos;
    }

    public IpInfo? GetByIp(string ip)
    {
        return _context.IpInfos.FirstOrDefault(x => x.Ip.Equals(ip));
    }

    public bool SaveChanges()
    {
        return _context.SaveChanges() >= 0;
    }
}