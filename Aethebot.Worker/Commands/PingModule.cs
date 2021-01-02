using Aethebot.Worker.Resources;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace Aethebot.Worker.Commands
{
    public class PingModule : ModuleBase<SocketCommandContext>
    {
        private Random _random;

        public PingModule()
        {
            _random = new Random();
        }

        [Command("ping")]
        public Task Pong()
        {
            var response = PingResources.PongResponse[_random.Next(PingResources.PongResponse.Length)];
            return ReplyAsync(message: $"{Context.User.Mention} {response}");
        }
    }
}
