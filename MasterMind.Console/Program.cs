using System;
using System.Collections.Generic;
using System.Linq;
using MasterMind.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MasterMind.ConsoleApp
{
    class Program
    {
        
        private static IServiceProvider serviceProvider;

        private const int SecretSize = 4;
        private const int MinDigit = 1;
        private const int MaxDigit = 6;

        private const char MaskChar = '?';

        static void Main(string[] args)
        {

            var history = new List<string>();

            RegisterServices();

            var game = serviceProvider
                .GetService<IGameService>();

            char[] digits = InitNewSecret(history, game);

            var exit = false;
            while (!exit)
            {
                Console.Clear();
                history.ForEach(s => Console.WriteLine(s));

                var attempt = Console.ReadLine();
                if (!AttemptIsValid(attempt))
                {
                    Console.WriteLine($"Digite apenas {SecretSize} números.");
                    Console.ReadLine();
                    continue;
                }

                var result = game.ValidateSecretAttempt(digits, attempt.ToCharArray());
                history.Add($"{attempt} {String.Join("", result)}");

                if (AttemptIsCorrect(result)) {
                    Console.WriteLine("Parabéns! Você acertou!");
                    Console.ReadLine();
                    digits = InitNewSecret(history, game);
                }
            }

            DisposeServices();
        }

        private static bool AttemptIsCorrect(char[] result)
        {
            return 
                result.Length == SecretSize && 
                result.All(c => c == GameCharacters.ExactPosition);
        }

        private static char[] InitNewSecret(List<string> history, IGameService game)
        {
            var digits = game.GenerateSecretNumber(SecretSize, MinDigit, MaxDigit);
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

        private static bool AttemptIsValid(string attempt)
        {
            return attempt.Length == SecretSize && int.TryParse(attempt, out int num);
        }

        private static void RegisterServices()
        {
            var collection = new ServiceCollection();
            collection.AddScoped<IRandom, RandomGenerator>();
            collection.AddScoped<IGameService, GameService>();

            serviceProvider = collection.BuildServiceProvider();
        }

        private static void DisposeServices()
        {
            if (serviceProvider != null &&
                serviceProvider is IDisposable disposable)
                disposable.Dispose();
        }

    }
}
