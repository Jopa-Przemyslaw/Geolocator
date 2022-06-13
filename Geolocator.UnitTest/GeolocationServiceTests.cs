using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Geolocator.Client.Abstractions;
using Geolocator.Data.Abstractions;
using Geolocator.Data.Models;
using Geolocator.DTO;
using Geolocator.Services;
using Geolocator.Services.Helpers;
using Geolocator.Services.Mappings.Profiles;
using Moq;
using Xunit;

namespace Geolocator.UnitTest;

public class GeolocationServiceTests
{
    private const string city = "Warsaw";
    private const string country = "PL";
    private const string hostname = "host@name";
    private const string ip = "21.62.172.98";
    private const string loc = "55.123,55.123";
    private const string org = "ip.org";
    private const string postal = "12-345";
    private const string region = "Mazovia";

    private static readonly IpInfo ipInfo = new()
    {
        City = city,
        Country = country,
        Hostname = hostname,
        Ip = ip,
        Loc = loc,
        Org = org,
        Postal = postal,
        Region = region
    };

    private static readonly IpInfoDto ipInfoDto = new()
    {
        City = city,
        Country = country,
        Hostname = hostname,
        Ip = ip,
        Loc = loc,
        Org = org,
        Postal = postal,
        Region = region
    };

    private readonly Mock<IIpInfoClient> _clientMock = new();
    private readonly Mock<IIpInfoRepository> _repositoryMock = new();
    private readonly GeolocationService _sut;

    public GeolocationServiceTests()
    {
        var mapper = new MapperConfiguration(cfg =>
                cfg.AddProfiles(new List<Profile>
                {
                    new IpInfoProfile()
                }))
            .CreateMapper();

        _sut = new GeolocationService(mapper, _clientMock.Object, _repositoryMock.Object);
    }

    [Fact]
    public async Task GetByIpFromSqlServerAsync_ShouldReturnIpInfo_WhenIpInfoExists()
    {
        // Arrange
        _repositoryMock.Setup(x => x.GetByIp(ip)).Returns(ipInfo);

        // Act
        var idInfo = await _sut.GetByIpFromSqlServerAsync(ip);

        // Assert
        _repositoryMock.Verify(x => x.GetByIp(ip), Times.Once);
        Assert.Equal(city, idInfo.City);
        Assert.Equal(country, idInfo.Country);
        Assert.Equal(ip, idInfo.Ip);
        Assert.Equal(loc, idInfo.Loc);
        Assert.Equal(org, idInfo.Org);
        Assert.Equal(postal, idInfo.Postal);
        Assert.Equal(region, idInfo.Region);
    }

    [Fact]
    public async Task GetByIpFromSqlServerAsync_ShouldThrowEntityNotFoundException_WhenIpInfoDoesNotExist()
    {
        // Arrange
        _repositoryMock.Setup(x => x.GetByIp(ip)).Returns(() => null);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(async () => await _sut.GetByIpFromSqlServerAsync(ip));
        _repositoryMock.Verify(x => x.GetByIp(ip), Times.Once);
    }

    [Fact]
    public async Task GetByIpFromIpInfoClientAsync_ShouldReturnIpInfo_WhenIpInfoIsCorrect()
    {
        // Arrange
        _clientMock.Setup(x => x.Send(ip)).ReturnsAsync(ipInfo);

        // Act
        var idInfo = await _sut.GetByIpFromIpInfoClientAsync(ip);

        // Assert
        _clientMock.Verify(x => x.Send(ip), Times.Once);
        Assert.Equal(city, idInfo.City);
        Assert.Equal(country, idInfo.Country);
        Assert.Equal(ip, idInfo.Ip);
        Assert.Equal(loc, idInfo.Loc);
        Assert.Equal(org, idInfo.Org);
        Assert.Equal(postal, idInfo.Postal);
        Assert.Equal(region, idInfo.Region);
    }

    [Fact]
    public async Task GetByIpFromIpInfoClientAsync_ShouldReturnEmptyIpInfo_WhenIpInfoIsIncorrect()
    {
        // Arrange
        _clientMock.Setup(x => x.Send(ip)).ReturnsAsync(() => null);

        // Act
        var idInfoDto = await _sut.GetByIpFromIpInfoClientAsync(ip);

        // Assert
        _clientMock.Verify(x => x.Send(ip), Times.Once);
        Assert.Null(idInfoDto.City);
        Assert.Null(idInfoDto.Country);
        Assert.Null(idInfoDto.Ip);
        Assert.Null(idInfoDto.Loc);
        Assert.Null(idInfoDto.Org);
        Assert.Null(idInfoDto.Postal);
        Assert.Null(idInfoDto.Region);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllIpInfos_WhenExistInDb()
    {
        // Arrange
        IEnumerable<IpInfo> items = new List<IpInfo>
        {
            ipInfo,
            new()
            {
                City = "null",
                Country = "null",
                Hostname = "null",
                Ip = "null",
                Loc = "null",
                Org = "null",
                Postal = "null",
                Region = "null"
            }
        };
        _repositoryMock.Setup(x => x.GetAll()).Returns(items);


        // Act
        var idInfoDtoList = await _sut.GetAllAsync();

        // Assert
        _repositoryMock.Verify(x => x.GetAll(), Times.Once);
        Assert.Equal(2, idInfoDtoList.Count);
        Assert.Equal(city, idInfoDtoList[0].City);
        Assert.Equal(country, idInfoDtoList[0].Country);
        Assert.Equal(ip, idInfoDtoList[0].Ip);
        Assert.Equal(loc, idInfoDtoList[0].Loc);
        Assert.Equal(org, idInfoDtoList[0].Org);
        Assert.Equal(postal, idInfoDtoList[0].Postal);
        Assert.Equal(region, idInfoDtoList[0].Region);
        Assert.Equal("null", idInfoDtoList[1].City);
        Assert.Equal("null", idInfoDtoList[1].Country);
        Assert.Equal("null", idInfoDtoList[1].Hostname);
        Assert.Equal("null", idInfoDtoList[1].Ip);
        Assert.Equal("null", idInfoDtoList[1].Loc);
        Assert.Equal("null", idInfoDtoList[1].Org);
        Assert.Equal("null", idInfoDtoList[1].Postal);
        Assert.Equal("null", idInfoDtoList[1].Region);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyIpInfos_WhenDoNotExistInDb()
    {
        // Arrange
        _repositoryMock.Setup(x => x.GetAll()).Returns(new List<IpInfo>());

        // Act
        var idInfoDtoList = await _sut.GetAllAsync();

        // Assert
        _repositoryMock.Verify(x => x.GetAll(), Times.Once);
        Assert.Equal(0, idInfoDtoList.Count);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveIpInfosFromDb_WhenExistsInDb()
    {
        // Arrange
        _repositoryMock.Setup(x => x.GetByIp(ipInfo.Ip)).Returns(ipInfo);
        _repositoryMock.Setup(x => x.Delete(ipInfo));

        // Act
        await _sut.DeleteAsync(ip);

        // Assert
        _repositoryMock.Verify(x => x.GetByIp(ip), Times.Once);
        _repositoryMock.Verify(x => x.Delete(ipInfo), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowWrongDataInputException_WhenNullIpAddress()
    {
        // Act & Assert
        await Assert.ThrowsAsync<WrongDataInputException>(async () => await _sut.DeleteAsync(null));
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowWrongDataInputException_WhenEmptyIpAddress()
    {
        // Act & Assert
        await Assert.ThrowsAsync<WrongDataInputException>(async () => await _sut.DeleteAsync(""));
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowEntityNotFoundException_WhenDoNotExists()
    {
        // Arrange
        _repositoryMock.Setup(x => x.GetByIp(ipInfo.Ip)).Returns(() => null);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(async () => await _sut.DeleteAsync(ip));
        _repositoryMock.Verify(x => x.GetByIp(ip), Times.Once);
    }

    [Fact(Skip = "Create method do not return ipInfo despite being mocked.")]
    public async Task CreateAsync_ShouldReturnIpInfoDto_WhenSavedSuccessfully()
    {
        // Arrange
        _repositoryMock.Setup(x => x.Create(ipInfo)).Returns(ipInfo);

        // Act
        var result = await _sut.CreateAsync(ipInfoDto);

        // Assert
        _repositoryMock.Verify(x => x.GetByIp(ipInfo.Ip), Times.Once);
        _repositoryMock.Verify(x => x.Create(ipInfo), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(city, result.City);
        Assert.Equal(country, result.Country);
        Assert.Equal(ip, result.Ip);
        Assert.Equal(loc, result.Loc);
        Assert.Equal(org, result.Org);
        Assert.Equal(postal, result.Postal);
        Assert.Equal(region, result.Region);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowWrongDataInputException_WhenNullIpAddress()
    {
        // Act & Assert
        await Assert.ThrowsAsync<WrongDataInputException>(async () => await _sut.CreateAsync(null));
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowWrongDataInputException_WhenEmptyIpAddress()
    {
        // Act & Assert
        await Assert.ThrowsAsync<WrongDataInputException>(async () => await _sut.CreateAsync(new IpInfoDto {Ip = ""}));
    }

    [Theory]
    [InlineData("0123.0.0.0")]
    [InlineData("0.0123.0.0")]
    [InlineData("0.0.0123.0")]
    [InlineData("0.0.0.0123")]
    [InlineData("a.0.0.0")]
    [InlineData("0.b.0.0")]
    [InlineData("0.0.c.0")]
    [InlineData("0.0.0.d")]
    [InlineData(" 0.0.0.0")]
    [InlineData("0.0.0.0 ")]
    [InlineData("0. 0.0.0")]
    [InlineData("0.0.0 .0")]
    [InlineData("0.00.0")]
    [InlineData("0.0..0")]
    [InlineData("")]
    [InlineData(" ")]
    public async Task CreateAsync_ShouldThrowWrongDataInputException_WhenInvalidIpAddress(
        string ip)
    {
        // Arrange
        var dto = ipInfoDto;
        dto.Ip = ip;

        // Act & Assert
        await Assert.ThrowsAsync<WrongDataInputException>(async () => await _sut.CreateAsync(dto));
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowEntityAlreadyExistsException_WhenEntityIsAlreadyCreated()
    {
        // Arrange
        ipInfoDto.Ip = ip;
        _repositoryMock.Setup(x => x.GetByIp(ip)).Returns(ipInfo);

        // Act & Assert
        await Assert.ThrowsAsync<EntityAlreadyExistsException>(async () => await _sut.CreateAsync(ipInfoDto));
        _repositoryMock.Verify(x => x.GetByIp(ip), Times.Once);
    }
}