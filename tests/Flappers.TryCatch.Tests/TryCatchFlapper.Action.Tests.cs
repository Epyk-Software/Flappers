namespace Flappers.TryCatch.Tests;

using System;
using Xunit;

public class TryCatchFlapperActionTests
{
    [Fact]
    public void TryCatchFlapper_Throws_WhenNoCatchHandlersForThrownException()
    {
        // given
        Action execute = () => throw new NotImplementedException();
        TryCatchFlapper flapper = new TryCatchFlapper(execute);

        // when
        var exception = Record.Exception(flapper.Execute);

        // then
        Assert.IsType<NotImplementedException>(exception);
    }

    [Fact]
    public void TryCatchFlapper_DoesNotThrow_WhenCatchHandlerHandlesForThrownException()
    {
        // given
        Action execute = () => throw new NotImplementedException();
        TryCatchFlapper flapper = new TryCatchFlapper(execute);
        flapper.Catch<NotImplementedException>(ex => { });

        // when
        var exception = Record.Exception(flapper.Execute);

        // then
        Assert.Null(exception);
    }

    [Fact]
    public void TryCatchFlapper_Throws_WhenExecutedCatchHandlerThrows()
    {
        // given
        Action execute = () => throw new NotImplementedException();
        TryCatchFlapper flapper = new TryCatchFlapper(execute);
        flapper.Catch<NotImplementedException>(ex => throw new InvalidOperationException(string.Empty, ex));

        // when
        var exception = Record.Exception(flapper.Execute);

        // then
        Assert.NotNull(exception);
        Assert.IsType<InvalidOperationException>(exception);
        Assert.NotNull(exception.InnerException);
        Assert.IsType<NotImplementedException>(exception.InnerException);
    }
}