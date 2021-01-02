using Discord;
using System.Collections.Generic;
using System.Globalization;

namespace Aethebot.Worker.EmbedTemplates
{
    public class Dril : Twitter
    {
        private const string DEFAULT_URL = "https://twitter.com/dril";

        public static Model.Twit NO { get; } = new Model.Twit("no", "922321981", 59902, 118179);
        public static Model.Twit LOGOFF { get; } = new Model.Twit("who the fuck is scraeming \"LOG OFF\" at my house. show yourself, coward. i will never log off", "247222360309121024", 16379, 34926);

        public static EmbedBuilder Get(Model.Twit twit)
        {
            return BUILDER()
                .WithAuthor(new EmbedAuthorBuilder()
                {
                    Name = "dril",
                    IconUrl = "https://cdn.discordapp.com/attachments/293954139845820416/697810031256666152/VXyQHfn0_bigger.jpg",
                    Url = DEFAULT_URL + (twit.Id != null ? $"/status/{twit.Id}" : string.Empty)
                })
                .WithFooter(new EmbedFooterBuilder
                {
                    Text = "Twitter",
                    IconUrl = "https://cdn.discordapp.com/attachments/293954139845820416/697810035769606206/twitter-footer.png"
                })
                .WithDescription(twit.Content)
                .WithFields(new List<EmbedFieldBuilder>
                {
                    new EmbedFieldBuilder
                    {
                        Name = "Retweets",
                        Value = twit.Retweets.ToString("N0", CultureInfo.InvariantCulture),
                        IsInline = true
                    },
                    new EmbedFieldBuilder
                    {
                        Name = "Likes",
                        Value = twit.Likes.ToString("N0", CultureInfo.InvariantCulture),
                        IsInline = true
                    }
                });
        }
    }
}
