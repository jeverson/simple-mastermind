using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;

namespace MasterMind.ConsoleApp.Cli
{
    public static class CliCommandCollectionExtensions
    {
        public static IServiceCollection AddCliCommands(this IServiceCollection services)
        {

            var classicCommandType = typeof(ClassicCommand);
            var commandType = typeof(Command);

            var commands = classicCommandType
                .Assembly
                .GetExportedTypes()
                .Where(x =>
                    x.Namespace == classicCommandType.Namespace &&
                    commandType.IsAssignableFrom(x)
                );

            foreach (var cmd in commands)
                services.AddSingleton(commandType, cmd);


            return services;
        }
    }
}
