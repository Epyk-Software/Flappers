namespace Flappers.Switch.Tests;

using System;
using Xunit;

public class SwitchFlapperActionTests
{
    [Fact]
    public void TryCatchFlapper_ExecutesDefaultHandler_WhenNoCatchHandlers()
    {
        // given
        Action defaultHandler = () => throw new NotImplementedException();
        int value = 42;
        SwitchFlapper<int> flapper = new SwitchFlapper<int>(value, defaultHandler);

        // when
        var exception = Record.Exception(flapper.Execute);

        // then
        Assert.IsType<NotImplementedException>(exception);
    }

    [Fact]
    public void TryCatchFlapper_ExecutesHandler_WhenValueMatches()
    {
        // given
        Action defaultHandler = () => throw new NotImplementedException();
        int value = 41;
        SwitchFlapper<int> flapper = new SwitchFlapper<int>(value, defaultHandler);
        flapper.Case(41, () => ++value);

        // when
        var exception = Record.Exception(flapper.Execute);

        // then
        Assert.Null(exception);
        Assert.Equal(42, value);
    }
}