namespace Flappers.Core.Tests;

using System;
using Xunit;

public partial class FlapperActionTests
{
    [Fact]
    public void Flapper_ExecuteThrows_WhenActionThrows()
    {
        // given
        Action execute = () => { throw new Exception(); };
        Flapper flapper = new Flapper(execute);

        // when
        var exception = Record.Exception(flapper.Execute);

        //then
        Assert.NotNull(exception);
        Assert.IsType<Exception>(exception);
    }

    [Fact]
    public void Flapper_ExecuteDoesNotThrow_WhenActionDoesNotThrows()
    {
        // given
        int foo = 42;
        Action execute = () => ++foo;
        Flapper flapper = new Flapper(execute);

        // when
        var exception = Record.Exception(flapper.Execute);

        //then
        Assert.Null(exception);
    }
}