using System;
using Xunit;

namespace Flappers.Core.Tests
{
    public partial class FlapperFuncTests
    {
        [Fact]
        public void Flapper_ExecuteThrows_WhenActionThrows()
        {
            // given
            Func<int> execute = () => { throw new Exception(); };
            Flapper<int> flapper = new Flapper<int>(execute);

            // when
            var exception = Record.Exception(() => flapper.Execute());

            //then
            Assert.NotNull(exception);
            Assert.IsType<Exception>(exception);
        }

        [Fact]
        public void Flapper_ExecuteDoesNotThrow_WhenActionDoesNotThrows()
        {
            // given
            int theAnswer = 41;
            Func<int> execute = () => ++theAnswer;
            Flapper<int> flapper = new Flapper<int>(execute);

            // when
            var actual = flapper.Execute();

            //then
            const int expected = 42;
            Assert.Equal(expected, actual);
        }
    }
}