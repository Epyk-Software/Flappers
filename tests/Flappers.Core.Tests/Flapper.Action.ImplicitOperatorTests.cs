namespace Flappers.Core.Tests;

using System;
using Xunit;

public partial class FlapperFuncTests
{
    [Fact]
    public void NonNullAction_ImplicitlyToFlapper_DoesNotThrow()
    {
        // given
        Action execute = () => { throw new Exception(); };
        Flapper flapper = null;

        // when
        var exception = Record.Exception(() => flapper = execute);

        //then
        Assert.Null(exception);
        Assert.NotNull(flapper);
    }

    [Fact]
    public void NonNullFlapper_ImplicitlyToAction_DoesNotThrow()
    {
        // given
        Action action = null;
        Flapper flapper = new Flapper(() => { });

        // when
        var exception = Record.Exception(() => action = flapper);

        //then
        Assert.Null(exception);
        Assert.NotNull(action);
    }
}