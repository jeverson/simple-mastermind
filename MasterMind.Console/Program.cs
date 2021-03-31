using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.Linq;
using System.Threading.Tasks;
using MasterMind.ConsoleApp.Cli;
using MasterMind.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MasterMind.ConsoleApp
{
    class Program
    {
        
        private static IServiceProvider serviceProvider;

        private const int SecretSize = 4;

        static async Task<int> Main(string[] args)
        {
            try
            {
                RegisterServices();

                var parser = BuildParser();

                return await parser.InvokeAsync(args).ConfigureAwait(false);
            }
            finally {
                DisposeServices();
            }
        }

        private static Parser BuildParser()
        {
            var cliBuilder = new CommandLineBuilder();
            foreach (var cmd in serviceProvider.GetServices<Command>())
                cliBuilder.AddCommand(cmd);

            return cliBuilder.UseDefaults().Build();

        }



        private static void RegisterServices()
        {
            var collection = new ServiceCollection();
            collection.AddScoped<IRandom, RandomGenerator>();
            collection.AddScoped<IGameService, GameService>();
            collection.AddCliCommands();

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
