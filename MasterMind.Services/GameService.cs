using System;
using System.Collections.Generic;
using System.Linq;

namespace MasterMind.Services
{
    public class GameService : IGameService
    {
        private readonly IRandom random;

        public GameService(IRandom random)
        {
            this.random = random;
        }

        public char[] GenerateSecretNumber(int size, int minDigit, int maxDigit)
        {
            var list = new List<Char>();
            for (int i = 0; i < size; i++)
            {
                var digit = random.Next(minDigit, maxDigit);
                var firstDigitchar = digit.ToString()[0];
                list.Add(firstDigitchar);
            }
            return list.ToArray();
        }

        public char[] ValidateSecretAttempt(char[] secret, char[] attempt)
        {
            var tempSecret = secret.ToList();
            var tempAttempt = attempt.ToList();

            var result = new List<Char>();

            for (int i = 0; i < attempt.Length; i++)
            {
                if (secret[i] == attempt[i])
                {
                    result.Add(GameCharacters.ExactPosition);
                    tempSecret[i] = Char.MinValue;
                    tempAttempt[i] = Char.MinValue;
                }
            }

            for (int i = 0; i < attempt.Length; i++)
            {
                var attemptChar = attempt[i];
                if (tempAttempt.Contains(attemptChar) && 
                    tempSecret.Contains(attemptChar))
                {
                    result.Add(GameCharacters.IncorrectPosition);
                    tempSecret.Remove(attemptChar);
                    tempAttempt.Remove(attemptChar);
                }
            }


            return result.ToArray();
        }
    }
}
