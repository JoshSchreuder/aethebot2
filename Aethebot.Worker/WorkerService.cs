using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

[assembly: CLSCompliant(false)]
namespace Aethebot.Worker
{
    public class WorkerService : BackgroundService
    {
        private string _discordToken;
        private DiscordSocketClient _client;
        private readonly ILogger<WorkerService> _logger;
        private readonly IServiceProvider services;
        private readonly CommandService commandService;

        public WorkerService(
            ILogger<WorkerService> logger,
            IConfiguration config,
            IServiceProvider services,
            CommandService commandService)
        {
            _logger = logger;
            _discordToken = config.GetValue<string>("DISCORD_TOKEN");
            _client = new DiscordSocketClient();
            this.services = services;
            this.commandService = commandService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Logging into Discord");
            await _client.LoginAsync(TokenType.Bot, _discordToken);

            _logger.LogInformation("Starting bot...");
            await _client.StartAsync();
            _logger.LogInformation("Bot started!");

            await commandService.AddModulesAsync(
                assembly: Assembly.GetExecutingAssembly(),
                services: services);

            // Hook the MessageReceived event into our command handler
            _client.MessageReceived += HandleCommandAsync;

            stoppingToken.Register(() => Stop());
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;
            if (message == null)
            {
                _logger.LogInformation("Command was a system message, ignoring.");
                return;
            }

            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;

            // Determine if the message is a command based on the prefix and make sure no bots trigger commands
            if (message.Content.Contains("v.redd.it", StringComparison.InvariantCultureIgnoreCase))
            {
                await commandService.Commands
                    .FirstOrDefault(x => x.Name == "vreddit")
                    .ExecuteAsync(new SocketCommandContext(_client, message), ParseResult.FromSuccess(new List<TypeReaderValue>() { new TypeReaderValue(message.Content, 1) }, new List<TypeReaderValue>()), services);
                return;
            }
            else if (message.HasMentionPrefix(_client.CurrentUser, ref argPos) || message.Channel is SocketDMChannel)
            {
                _logger.LogInformation("Command is valid, processing...");
            }
            else if (message.Author.IsBot)
            {
                _logger.LogInformation("Command was triggered by a bot, ignoring.");
                return;
            }
            else
            {
                _logger.LogInformation("Unknown command type, ignoring.");
                return;
            }

            // Execute the command with the command context we just
            // created, along with the service provider for precondition checks.
            _logger.LogInformation("Executing command!");
            var result = await commandService.ExecuteAsync(
                context: new SocketCommandContext(_client, message),
                argPos: argPos,
                services: services);
            _logger.LogInformation($"Success: {result.IsSuccess} - {result.ErrorReason}");
        }

        private Task Stop()
        {
            _logger.LogInformation("Bot stopped!");
            return _client.StopAsync();
        }
    }
}
