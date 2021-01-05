using Discord;
using System;

namespace Aethebot.Worker.EmbedTemplates
{
    public abstract class TwitterEmbed
    {
        public static Func<EmbedBuilder> BUILDER { get; } = () => new EmbedBuilder
        {
            Footer = new EmbedFooterBuilder
            {
                Text = "Twitter",
                IconUrl = "https://cdn.discordapp.com/attachments/293954139845820416/697810035769606206/twitter-footer.png"
            },
            Color = new Color(29, 161, 242)
        };
    }
}
