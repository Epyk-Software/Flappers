namespace Flappers.Core.Tests;

using System;
using Xunit;

public partial class FlapperActionTests
{
    [Fact]
    public void Flapper_Throws_WhenActionIsNull()
    {
        // given
        Action execute = null;

        // when
        var exception = Record.Exception(() => new Flapper(execute));

        //then
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void Flapper_DoesNotThrow_WhenActionIsNotNull()
    {
        // given
        Action execute = () => { };


        // when
        var exception = Record.Exception(() => new Flapper(execute));

        //then
        Assert.Null(exception);
    }
}