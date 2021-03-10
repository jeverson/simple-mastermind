using Moq;
using Xunit;

namespace MasterMind.Services.Tests
{
    public class GameServiceTest
    {
        [Fact]
        public void GenerateSecretSingleDigit()
        {
            var randomDigit = 2;
            var random = new Mock<IRandom>();
            random.Setup(r => r.Next(It.IsAny<int>(), It.IsAny<int>())).Returns(randomDigit);

            var sut = new GameService(random.Object);
            var actual = sut.GenerateSecretNumber(1, 1, 6);

            var expected = new char[] { '2' };
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GeneratesFourDigits()
        {
            var random = new Mock<IRandom>();
            random.SetupSequence(r => r.Next(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(1)
                .Returns(4)
                .Returns(5)
                .Returns(4)
                .Returns(1);

            var sut = new GameService(random.Object);
            var actual = sut.GenerateSecretNumber(4, 1, 6);

            var expected = new char[] { '1', '4', '5', '4' };
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ValidateSingleDigitCorrectAttempt()
        {
            var secret = new char[] { '1' };
            var attempt = new char[] { '1' };
            var expected = new char[] { GameCharacters.ExactPosition };

            ValidateAndAssert(secret, attempt, expected);
        }

        [Fact]
        public void ValidateSingleDigitIncorrectSecret()
        {
            var secret = new char[] { '1' };
            var attempt = new char[] { '2' };
            var expected = new char[] { };

            ValidateAndAssert(secret, attempt, expected);
        }

        [Fact]
        public void ValidateIncorrectPositionSecret()
        {
            var secret = new char[] { '1', '2', '3' };
            var attempt = new char[] { '2', '4', '5' };
            var expected = new char[] { GameCharacters.IncorrectPosition  };

            ValidateAndAssert(secret, attempt, expected);
        }

        [Fact]
        public void ValidateCorrectAndIncorrectPositionsSecret()
        {
            var secret = new char[] { '1', '2', '3' };
            var attempt = new char[] { '2', '4', '3' };
            var expected = new char[] {
                GameCharacters.ExactPosition,
                GameCharacters.IncorrectPosition
            };

            ValidateAndAssert(secret, attempt, expected);
        }

        [Fact]
        public void ValidateSameDigitInCorrectAndIncorrectPositions()
        {
            var secret = new char[] { '1', '2', '3' };
            var attempt = new char[] { '2', '2', '4' };
            var expected = new char[] {
                GameCharacters.ExactPosition
            };

            ValidateAndAssert(secret, attempt, expected);
        }

        [Fact]
        public void ValidateAllIncorrectPositions()
        {
            var secret = new char[] { '1', '2', '3', '4' };
            var attempt = new char[] { '4', '3', '2', '1' };
            var expected = new char[] {
                GameCharacters.IncorrectPosition,
                GameCharacters.IncorrectPosition,
                GameCharacters.IncorrectPosition,
                GameCharacters.IncorrectPosition
            };

            ValidateAndAssert(secret, attempt, expected);
        }

        [Fact]
        public void ValidatePartialPositions()
        {
            var secret = new char[] { '5', '6', '2', '5' };
            var attempt = new char[] { '5', '5', '2', '5' };
            var expected = new char[] {
                GameCharacters.ExactPosition,
                GameCharacters.ExactPosition,
                GameCharacters.ExactPosition,
            };

            ValidateAndAssert(secret, attempt, expected);
        }

        [Fact]
        public void ValidateAllExactPositions()
        {
            var secret = new char[] { '1', '2', '3', '4' };
            var attempt = new char[] { '1', '2', '3', '4' };
            var expected = new char[] {
                GameCharacters.ExactPosition,
                GameCharacters.ExactPosition,
                GameCharacters.ExactPosition,
                GameCharacters.ExactPosition
            };

            ValidateAndAssert(secret, attempt, expected);
        }

        private static void ValidateAndAssert(char[] secret, char[] attempt, char[] expected)
        {
            var random = new Mock<IRandom>();
            var sut = new GameService(random.Object);

            var actual = sut.ValidateSecretAttempt(secret, attempt);
            Assert.Equal(expected, actual);
        }
    }
}
