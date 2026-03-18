using Kairudev.Domain.Sprint;
using Xunit;

namespace Kairudev.Domain.Tests.Sprint;

public sealed class SprintNameTests
{
    [Fact]
    public void Should_CreateWithValue_When_ValueIsValid()
    {
        var result = SprintName.Create("Focus session", 1);

        Assert.True(result.IsSuccess);
        Assert.Equal("Focus session", result.Value.Value);
    }

    [Fact]
    public void Should_UseDefaultName_When_ValueIsEmpty()
    {
        var result = SprintName.Create("", 3);

        Assert.True(result.IsSuccess);
        Assert.Equal("Sprint #3", result.Value.Value);
    }

    [Fact]
    public void Should_UseDefaultName_When_ValueIsWhitespace()
    {
        var result = SprintName.Create("   ", 5);

        Assert.True(result.IsSuccess);
        Assert.Equal("Sprint #5", result.Value.Value);
    }

    [Fact]
    public void Should_UseDefaultName_When_ValueIsNull()
    {
        var result = SprintName.Create(null, 2);

        Assert.True(result.IsSuccess);
        Assert.Equal("Sprint #2", result.Value.Value);
    }

    [Fact]
    public void Should_Fail_When_ValueExceedsMaxLength()
    {
        var longName = new string('x', SprintName.MaxLength + 1);

        var result = SprintName.Create(longName, 1);

        Assert.True(result.IsFailure);
        Assert.Equal(SprintDomainErrors.Sprint.NameTooLong, result.Error);
    }

    [Fact]
    public void Should_Succeed_When_ValueIsExactlyMaxLength()
    {
        var maxName = new string('x', SprintName.MaxLength);

        var result = SprintName.Create(maxName, 1);

        Assert.True(result.IsSuccess);
        Assert.Equal(maxName, result.Value.Value);
    }
}
