using System.Text.RegularExpressions;
using AutoMapper;
using Geolocator.Client.Abstractions;
using Geolocator.Data.Abstractions;
using Geolocator.Data.Models;
using Geolocator.DTO;
using Geolocator.Services.Abstractions;
using Geolocator.Services.Helpers;

namespace Geolocator.Services;

public class GeolocationService : IGeolocationService
{
    private const string Pattern = @"^\d{1,3}.\d{1,3}.\d{1,3}.\d{1,3}$";
    private readonly IIpInfoClient _ipInfoClient;
    private readonly IMapper _mapper;
    private readonly IIpInfoRepository _repository;

    public GeolocationService(IMapper mapper, IIpInfoClient ipInfoClient, IIpInfoRepository repository)
    {
        _mapper = mapper;
        _ipInfoClient = ipInfoClient;
        _repository = repository;
    }

    public async Task<IpInfoDto> CreateAsync(IpInfoDto ipInfoDto)
    {
        if (ipInfoDto == null || string.IsNullOrEmpty(ipInfoDto.Ip))
            throw ExceptionHelper.WrongDataInputException("Provided dto or ip address not valid.");

        ValidateIpAddress(ipInfoDto.Ip);

        var existingIpInfo = _repository.GetByIp(ipInfoDto.Ip);
        if (existingIpInfo != null)
            throw ExceptionHelper.EntityAlreadyExistsException(nameof(IpInfo), ipInfoDto.Ip);

        var ipInfo = _mapper.Map<IpInfo>(ipInfoDto);

        var savedIpInfo = _repository.Create(ipInfo);
        _repository.SaveChanges();

        ipInfoDto = _mapper.Map<IpInfoDto>(savedIpInfo);

        return ipInfoDto;
    }

    public async Task<bool> DeleteAsync(string ip)
    {
        if (string.IsNullOrEmpty(ip))
            throw ExceptionHelper.WrongDataInputException($"Provided id: {ip} is invalid.");

        var existingIpInfo = ValidateIpInfoIfExists(ip);

        _repository.Delete(existingIpInfo);
        _repository.SaveChanges();
        return true;
    }

    public async Task<IpInfoDto> GetByIpFromIpInfoClientAsync(string ip)
    {
        var ipInfo = await _ipInfoClient.Send(ip);
        if (ipInfo == null) return new IpInfoDto();

        var ipInfoDto = _mapper.Map<IpInfoDto>(ipInfo);

        return ipInfoDto;
    }

    public async Task<IpInfoDto> GetByIpFromSqlServerAsync(string ip)
    {
        var existingIpInfo = ValidateIpInfoIfExists(ip);

        var ipInfoDto = _mapper.Map<IpInfoDto>(existingIpInfo);

        return ipInfoDto;
    }

    public async Task<List<IpInfoDto>> GetAllAsync()
    {
        var ipInfosList = _repository.GetAll().ToList();
        var ipInfoListDto = _mapper.Map<List<IpInfoDto>>(ipInfosList);

        return ipInfoListDto;
    }

    private IpInfo ValidateIpInfoIfExists(string ip)
    {
        var existingIpInfo = _repository.GetByIp(ip);

        if (existingIpInfo == null)
            throw ExceptionHelper.EntityNotFoundException(nameof(IpInfo), ip);
        return existingIpInfo;
    }

    private void ValidateIpAddress(string ip)
    {
        var regex = new Regex(Pattern);
        var match = regex.Match(ip, 0);
        if (!match.Success)
            throw ExceptionHelper.WrongDataInputException($"Provided ip address: {ip} is not valid.");
    }
}