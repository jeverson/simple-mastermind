using Xunit;

namespace MasterMind.Services.Tests
{
    public class RandomGeneratorTest
    {
        [Theory]
        [InlineData(100, 1, 2)]
        [InlineData(200, 10, 100)]
        [InlineData(100, 100, 1000)]
        public void GenerateUnderMax(int numberOfTests, int min, int max)
        {
            IRandom sut = new RandomGenerator();
            for (int i = 0; i < numberOfTests; i++)
            {
                int currentRandom = sut.Next(min, max);
                Assert.InRange(currentRandom, 0, max);
            }
        }
    }
}
