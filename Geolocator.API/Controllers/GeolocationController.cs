using Geolocator.DTO;
using Geolocator.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Geolocator.API.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class GeolocationController : ControllerBase
{
    private readonly IGeolocationService _geolocationService;

    public GeolocationController(IGeolocationService geolocationService)
    {
        _geolocationService = geolocationService;
    }

    [HttpGet("ext/{ip}", Name = "GetGeolocation_FromExternalService_IpInfo")]
    public async Task<ActionResult<IpInfoDto>> GetFromIpInfo(string ip)
    {
        var result = await _geolocationService.GetByIpFromIpInfoClientAsync(ip);

        return Ok(result);
    }

    [HttpGet("{ip}", Name = "GetGeolocation_FromSqlDatabase")]
    public async Task<ActionResult<IpInfoDto>> GetFromSqlServer(string ip)
    {
        var result = await _geolocationService.GetByIpFromSqlServerAsync(ip);

        return Ok(result);
    }

    [HttpGet(Name = "GetAll")]
    public async Task<ActionResult<List<IpInfoDto>>> GetAll()
    {
        var result = await _geolocationService.GetAllAsync();

        return Ok(result);
    }

    [HttpPost(Name = "AddGeolocation")]
    public async Task<ActionResult<IpInfoDto>> Create(IpInfoDto ipInfo)
    {
        var result = await _geolocationService.CreateAsync(ipInfo);

        return Ok(result);
    }

    [HttpDelete("{ip}", Name = "DeleteGeolocation")]
    public async Task<ActionResult<IpInfoDto>> Delete(string ip)
    {
        var result = await _geolocationService.DeleteAsync(ip);

        return Ok(result);
    }
}