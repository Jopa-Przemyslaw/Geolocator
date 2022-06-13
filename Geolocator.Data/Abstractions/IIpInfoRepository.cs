using System.Collections.Generic;
using Geolocator.Data.Models;

namespace Geolocator.Data.Abstractions;

public interface IIpInfoRepository
{
    IpInfo Create(IpInfo ipInfo);
    void Delete(IpInfo ipInfo);
    IEnumerable<IpInfo> GetAll();
    IpInfo? GetByIp(string ip);
    bool SaveChanges();
}