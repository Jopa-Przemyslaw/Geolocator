using Geolocator.DTO;

namespace Geolocator.Services.Abstractions;

public interface IGeolocationService
{
    Task<IpInfoDto> CreateAsync(IpInfoDto ipInfoDto);
    Task<bool> DeleteAsync(string ip);
    Task<IpInfoDto> GetByIpFromIpInfoClientAsync(string ip);
    Task<IpInfoDto> GetByIpFromSqlServerAsync(string ip);
    Task<List<IpInfoDto>> GetAllAsync();
}