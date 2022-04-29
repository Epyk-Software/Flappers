using System;
using Xunit;

namespace Flappers.Core.Tests
{
    public partial class FlapperFuncTests
    {
        [Fact]
        public void Flapper_Throws_WhenFuncIsNull()
        {
            // given
            Func<int> execute = null;

            // when
            var exception = Record.Exception(() => new Flapper<int>(execute));

            //then
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void Flapper_DoesNotThrow_WhenActionIsNotNull()
        {
            // given
            Func<object> execute = () => new { };

            // when
            var exception = Record.Exception(() => new Flapper<object>(execute));

            //then
            Assert.Null(exception);
        }
    }
}