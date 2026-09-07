using FieldCollectionRegister.Core.Dtos;
using FieldCollectionRegister.Core.Interfaces;
using FieldCollectionRegister.Core.Models;
using FieldCollectionRegister.Services;
using Moq;
using Xunit;

namespace FieldCollectionRegister.Tests;

public class EntryServiceTests
{
    private readonly Mock<IEntryRepository> _entryRepoMock;
    private readonly Mock<IFieldRepository> _fieldRepoMock;
    private readonly EntryService _service;

    public EntryServiceTests()
    {
        _entryRepoMock = new Mock<IEntryRepository>();
        _fieldRepoMock = new Mock<IFieldRepository>();
        _service = new EntryService(_entryRepoMock.Object, _fieldRepoMock.Object);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public async Task CreateEntryAsync_WhenFieldIdIsZeroOrNegative_ReturnsFail(int invalidFieldId)
    {
        // Arrange
        var request = new CreateEntryRequest
        {
            FieldId = invalidFieldId,
            Quantity = 15.5m,
            CollectionDate = DateOnly.FromDateTime(DateTime.UtcNow)
        };

        // Act
        var result = await _service.CreateEntryAsync(request);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("A field must be selected.", result.Error);
        _entryRepoMock.Verify(r => r.CreateAsync(It.IsAny<CreateEntryRequest>()), Times.Never);
    }

    [Fact]
    public async Task CreateEntryAsync_WhenFieldDoesNotExist_ReturnsFail()
    {
        // Arrange
        var request = new CreateEntryRequest
        {
            FieldId = 99,
            Quantity = 20.0m,
            CollectionDate = DateOnly.FromDateTime(DateTime.UtcNow)
        };
        _fieldRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Field?)null);

        // Act
        var result = await _service.CreateEntryAsync(request);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("The selected field does not exist.", result.Error);
        _entryRepoMock.Verify(r => r.CreateAsync(It.IsAny<CreateEntryRequest>()), Times.Never);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-0.1)]
    [InlineData(-50)]
    public async Task CreateEntryAsync_WhenQuantityIsZeroOrNegative_ReturnsFail(decimal invalidQuantity)
    {
        // Arrange
        var request = new CreateEntryRequest
        {
            FieldId = 1,
            Quantity = invalidQuantity,
            CollectionDate = DateOnly.FromDateTime(DateTime.UtcNow)
        };
        _fieldRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Field { Id = 1, Name = "Field 1", EstateId = 1 });

        // Act
        var result = await _service.CreateEntryAsync(request);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Quantity must be greater than zero.", result.Error);
        _entryRepoMock.Verify(r => r.CreateAsync(It.IsAny<CreateEntryRequest>()), Times.Never);
    }

    [Fact]
    public async Task CreateEntryAsync_WhenCollectionDateIsDefault_ReturnsFail()
    {
        // Arrange
        var request = new CreateEntryRequest
        {
            FieldId = 1,
            Quantity = 10.0m,
            CollectionDate = default
        };
        _fieldRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Field { Id = 1, Name = "Field 1", EstateId = 1 });

        // Act
        var result = await _service.CreateEntryAsync(request);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("A collection date is required.", result.Error);
        _entryRepoMock.Verify(r => r.CreateAsync(It.IsAny<CreateEntryRequest>()), Times.Never);
    }

    [Fact]
    public async Task CreateEntryAsync_WhenCollectionDateIsInFuture_ReturnsFail()
    {
        // Arrange
        var futureDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(2));
        var request = new CreateEntryRequest
        {
            FieldId = 1,
            Quantity = 10.0m,
            CollectionDate = futureDate
        };
        _fieldRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Field { Id = 1, Name = "Field 1", EstateId = 1 });

        // Act
        var result = await _service.CreateEntryAsync(request);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Collection date cannot be in the future.", result.Error);
        _entryRepoMock.Verify(r => r.CreateAsync(It.IsAny<CreateEntryRequest>()), Times.Never);
    }

    [Fact]
    public async Task CreateEntryAsync_WhenRequestIsValid_CreatesAndReturnsOk()
    {
        // Arrange
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var request = new CreateEntryRequest
        {
            FieldId = 5,
            Quantity = 125.75m,
            CollectionDate = today,
            Note = "Morning batch"
        };
        var createdEntry = new CollectionEntry
        {
            Id = 42,
            FieldId = 5,
            Quantity = 125.75m,
            CollectionDate = today,
            Note = "Morning batch",
            FieldName = "Field 5",
            EstateName = "Green Valley",
            RegionName = "Highlands"
        };

        _fieldRepoMock.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(new Field { Id = 5, Name = "Field 5", EstateId = 2 });
        _entryRepoMock.Setup(r => r.CreateAsync(request)).ReturnsAsync(createdEntry);

        // Act
        var result = await _service.CreateEntryAsync(request);

        // Assert
        Assert.True(result.Success);
        Assert.Null(result.Error);
        Assert.NotNull(result.Value);
        Assert.Equal(42, result.Value.Id);
        Assert.Equal(125.75m, result.Value.Quantity);
        Assert.Equal("Morning batch", result.Value.Note);
        _entryRepoMock.Verify(r => r.CreateAsync(request), Times.Once);
    }

    [Fact]
    public async Task GetEntriesAsync_PassesFilterToRepositoryAndReturnsEntries()
    {
        // Arrange
        var filter = new EntryFilter { RegionId = 1, EstateId = 2, FieldId = 3 };
        var expectedEntries = new List<CollectionEntry>
        {
            new() { Id = 1, FieldId = 3, Quantity = 50.0m, CollectionDate = DateOnly.FromDateTime(DateTime.UtcNow) },
            new() { Id = 2, FieldId = 3, Quantity = 75.0m, CollectionDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)) }
        };

        _entryRepoMock.Setup(r => r.GetAsync(filter)).ReturnsAsync(expectedEntries);

        // Act
        var result = await _service.GetEntriesAsync(filter);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _entryRepoMock.Verify(r => r.GetAsync(filter), Times.Once);
    }
}
