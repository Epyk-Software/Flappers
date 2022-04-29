namespace Flappers.TryCatch.Tests;

using System;
using Xunit;

public class TryCatchFlapperFuncTests
{
    [Fact]
    public void TryCatchFlapper_Throws_WhenNoCatchHandlersForThrownException()
    {
        // given
        Func<object> execute = () => throw new NotImplementedException();
        TryCatchFlapper<object> flapper = new TryCatchFlapper<object>(execute);

        // when
        var exception = Record.Exception(flapper.Execute);

        // then
        Assert.IsType<NotImplementedException>(exception);
    }

    [Fact]
    public void TryCatchFlapper_DoesNotThrow_WhenCatchHandlerHandlesForThrownException()
    {
        // given
        Func<object> execute = () => throw new NotImplementedException();
        TryCatchFlapper<object> flapper = new TryCatchFlapper<object>(execute);
        flapper.Catch<NotImplementedException>(ex => new { });

        // when
        var exception = Record.Exception(flapper.Execute);

        // then
        Assert.Null(exception);
    }

    [Fact]
    public void TryCatchFlapper_Throws_WhenExecutedCatchHandlerThrows()
    {
        // given
        Func<object> execute = () => throw new NotImplementedException();
        TryCatchFlapper<object> flapper = new TryCatchFlapper<object>(execute);
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