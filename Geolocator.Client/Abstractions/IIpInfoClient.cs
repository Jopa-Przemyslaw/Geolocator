using Geolocator.Data.Models;

namespace Geolocator.Client.Abstractions;

public interface IIpInfoClient
{
    public Task<IpInfo?> Send(string ip);
}