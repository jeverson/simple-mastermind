using MasterMind.Services;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;

namespace MasterMind.ConsoleApp.Cli
{
    public class ClassicCommand : Command
    {

        private const int MinDigit = 1;
        private const int MaxDigit = 6;

        private const char MaskChar = '?';

        private readonly IGameService gameService;

        public ClassicCommand(IGameService gameService)
            : base("classic", "Runs the classic version.")
        {
            var size = new Option<int>("--size")
            {
                Name = "size",
                Description = "The number of digits in the secret generated",
                IsRequired = true
            };

            AddOption(size);

            this.Handler = CommandHandler.Create<int>(
                (size) => this.HandleCommand(size)
            );
            this.gameService = gameService;
        }

        private void HandleCommand(int size)
        {
            RunClassicGame(size);
        }

        private void RunClassicGame(int size)
        {

            var history = new List<string>();

            char[] secret = InitNewSecret(history, gameService, size);

            var exit = false;
            while (!exit)
            {
                Console.Clear();
                history.ForEach(s => Console.WriteLine(s));

                var attempt = Console.ReadLine();
                if (!AttemptIsValid(attempt, size))
                {
                    Console.WriteLine($"Digite apenas {size} números.");
                    Console.ReadLine();
                    continue;
                }

                var result = gameService.ValidateSecretAttempt(secret, attempt.ToCharArray());
                history.Add($"{attempt} {String.Join("", result)}");

                if (AttemptIsCorrect(result, size))
                {
                    Console.WriteLine("Parabéns! Você acertou!");
                    Console.ReadLine();
                    secret = InitNewSecret(history, gameService, size);
                }
            }
        }

        private static bool AttemptIsCorrect(char[] result, int secretSize)
        {
            return
                result.Length == secretSize &&
                result.All(c => c == GameCharacters.ExactPosition);
        }

        private static char[] InitNewSecret(List<string> history, IGameService game, int size)
        {
            var digits = game.GenerateSecretNumber(size, MinDigit, MaxDigit);
            // history.Add(digits.ToString());

            history.Clear();
            history.Add("Consegue acertar o número?");
            history.Add("Digite 'Ctrl + C' para sair.");
            history.Add(string.Empty);

            var mask = new String(MaskChar, digits.Length);
            history.Add(mask);
            history.Add(string.Empty);
            return digits;
        }

        private static bool AttemptIsValid(string attempt, int size)
        {
            return attempt.Length == size && int.TryParse(attempt, out int num);
        }
    }
}
