namespace Flappers.TryCatch.Tests;

using System;
using Xunit;

public class ActionExtensiosnsTests
{
    [Fact]
    public void TryCatchFlapper_Throws_WhenNoCatchHandlersForThrownException()
    {
        // given
        Action execute = () => throw new NotImplementedException();
        TryCatchFlapper flapper = execute.Catch<NotSupportedException>(ex => { });

        // when
        var exception = Record.Exception(flapper.Execute);

        // then
        Assert.IsType<NotImplementedException>(exception);
    }
}