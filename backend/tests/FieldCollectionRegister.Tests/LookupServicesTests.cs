using FieldCollectionRegister.Core.Interfaces;
using FieldCollectionRegister.Core.Models;
using FieldCollectionRegister.Services;
using Moq;
using Xunit;

namespace FieldCollectionRegister.Tests;

public class LookupServicesTests
{
    [Fact]
    public async Task EstateService_GetEstatesForRegionAsync_ReturnsEstatesFromRepository()
    {
        // Arrange
        var mockRepo = new Mock<IEstateRepository>();
        var sampleEstates = new List<Estate>
        {
            new() { Id = 1, Name = "Alpha Estate", RegionId = 10 },
            new() { Id = 2, Name = "Beta Estate", RegionId = 10 }
        };
        mockRepo.Setup(r => r.GetByRegionIdAsync(10)).ReturnsAsync(sampleEstates);

        var service = new EstateService(mockRepo.Object);

        // Act
        var result = await service.GetEstatesForRegionAsync(10);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        mockRepo.Verify(r => r.GetByRegionIdAsync(10), Times.Once);
    }

    [Fact]
    public async Task FieldService_GetFieldsForEstateAsync_ReturnsFieldsFromRepository()
    {
        // Arrange
        var mockRepo = new Mock<IFieldRepository>();
        var sampleFields = new List<Field>
        {
            new() { Id = 1, Name = "North Field", EstateId = 5 },
            new() { Id = 2, Name = "South Field", EstateId = 5 }
        };
        mockRepo.Setup(r => r.GetByEstateIdAsync(5)).ReturnsAsync(sampleFields);

        var service = new FieldService(mockRepo.Object);

        // Act
        var result = await service.GetFieldsForEstateAsync(5);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        mockRepo.Verify(r => r.GetByEstateIdAsync(5), Times.Once);
    }

    [Fact]
    public async Task RegionService_GetAllRegionsAsync_ReturnsRegionsFromRepository()
    {
        // Arrange
        var mockRepo = new Mock<IRegionRepository>();
        var sampleRegions = new List<Region>
        {
            new() { Id = 1, Name = "Highlands" },
            new() { Id = 2, Name = "Lowlands" }
        };
        mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(sampleRegions);

        var service = new RegionService(mockRepo.Object);

        // Act
        var result = await service.GetAllRegionsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        mockRepo.Verify(r => r.GetAllAsync(), Times.Once);
    }
}
