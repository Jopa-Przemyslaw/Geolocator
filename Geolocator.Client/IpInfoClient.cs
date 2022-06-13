using Geolocator.Client.Abstractions;
using Geolocator.Data.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Geolocator.Client;

public class IpInfoClient : IIpInfoClient
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public IpInfoClient(HttpClient httpClient, IConfiguration configuration)
    {
        _configuration = configuration;
        _httpClient = httpClient;
    }

    public async Task<IpInfo?> Send(string ip)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, _configuration.GetConnectionString("IpInfoClient") + ip);
        var response = await _httpClient.SendAsync(request);
        using var reader = new StreamReader(await response.Content.ReadAsStreamAsync());
        var responseBody = await reader.ReadToEndAsync();

        var ipInfo = JsonConvert.DeserializeObject<IpInfo>(responseBody);

        return ipInfo;
    }
}