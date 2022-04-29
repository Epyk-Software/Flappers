namespace Flappers.TryCatch.Tests;

using System;
using Xunit;

public class FuncExtensiosnsTests
{
    [Fact]
    public void TryCatchFlapper_Throws_WhenNoCatchHandlersForThrownException()
    {
        // given
        Func<object> execute = () => throw new NotImplementedException();
        TryCatchFlapper<object> flapper = execute.Catch<NotSupportedException, object>(ex => new { });

        // when
        var exception = Record.Exception(flapper.Execute);

        // then
        Assert.IsType<NotImplementedException>(exception);
    }
}