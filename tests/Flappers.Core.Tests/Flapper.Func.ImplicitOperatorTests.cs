namespace Flappers.Core.Tests;

using System;
using Xunit;

public partial class FlapperFuncTests
{
    [Fact]
    public void NonNullFunc_ImplicitlyToFlapper_DoesNotThrow()
    {
        // given
        Func<object> execute = () => new { };
        Flapper<object> flapper = null;

        // when
        var exception = Record.Exception(() => flapper = execute);

        //then
        Assert.Null(exception);
        Assert.NotNull(flapper);
    }

    [Fact]
    public void NonNullFlapper_ImplicitlyToFunc_DoesNotThrow()
    {
        // given
        Func<object> func = null;
        Flapper<object> flapper = new Flapper<object>(() =>  new { });

        // when
        var exception = Record.Exception(() => func = flapper);

        //then
        Assert.Null(exception);
        Assert.NotNull(func);
    }
}