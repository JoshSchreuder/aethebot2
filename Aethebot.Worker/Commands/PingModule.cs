using Aethebot.Worker.Resources;
using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
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

        /// <summary>
        /// ...th-that's lewd
        /// </summary>
        /// <returns></returns>
        [Command("drill me")]
        [Alias("drillme")]
        public Task Dril()
        {
            var embedBuilder = new EmbedBuilder
            {
                Author = new EmbedAuthorBuilder()
                {
                    Name = "dril",
                    IconUrl = "https://cdn.discordapp.com/attachments/293954139845820416/697810031256666152/VXyQHfn0_bigger.jpg",
                    Url = "https://twitter.com/dril/status/922321981"
                },
                Description = "no",
                Footer = new EmbedFooterBuilder
                {
                    Text = "Twitter",
                    IconUrl = "https://cdn.discordapp.com/attachments/293954139845820416/697810035769606206/twitter-footer.png"
                },
                Color = new Color(29, 161, 242),
                Fields = new List<EmbedFieldBuilder>
                {
                    new EmbedFieldBuilder
                    {
                        Name = "Retweets",
                        Value = "59902",
                        IsInline = true
                    },
                    new EmbedFieldBuilder
                    {
                        Name = "Likes",
                        Value = "118179",
                        IsInline = true
                    }
                }
            };
            return ReplyAsync(message: Context.User.Mention, embed: embedBuilder.Build());
        }
    }
}
