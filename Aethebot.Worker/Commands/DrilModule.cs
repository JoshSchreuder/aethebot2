using Aethebot.Worker.EmbedTemplates;
using Aethebot.Worker.Model;
using Aethebot.Worker.Resources.twits;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Aethebot.Worker.Commands
{
    public class DrilModule : ModuleBase<SocketCommandContext>
    {
        private Queue<int> _order = new Queue<int>();
        private readonly List<Twit> _twits;

        public DrilModule()
        {
            var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _twits = JsonSerializer.Deserialize<List<Twit>>(Twits.dril_nasa, opts)
                .Concat(JsonSerializer.Deserialize<List<Twit>>(Twits.dril, opts))
                .Concat(new List<Twit> { DrilEmbed.LOGOFF, DrilEmbed.NO })
                .ToList();

            QueueUp();
        }

        [Command("dril me")]
        [Alias("drilme", ":dril:")]
        [Summary("Return a random lewd twit")]
        public Task DrilMe()
        {
            var twit = _twits[_order.Dequeue()];
            if (_order.Count == 0)
            {
                QueueUp();
            }

            return ReplyAsync(message: Context.User.Mention, embed: DrilEmbed.Get(twit).Build());
        }

        [Command("log off")]
        [Alias("logoff")]
        [Summary("show yourself coward")]

        public Task LogOff()
        {
            return ReplyAsync(message: Context.User.Mention, embed: DrilEmbed.Get(DrilEmbed.LOGOFF).Build());
        }

        [Command("drill me")]
        [Alias("drillme")]
        [Summary("...th-that's lewd")]
        public Task DrillMe()
        {
            return ReplyAsync(message: Context.User.Mention, embed: DrilEmbed.Get(DrilEmbed.NO).Build());
        }

        private void QueueUp()
        {
            var indices = Enumerable.Range(0, _twits.Count).ToList();
            var random = new Random();
            for (int i = indices.Count - 1; i > 1; i--)
            {
                var rnd = random.Next(i + 1);
                _order.Enqueue(indices[rnd]);
            }
        }
    }
}
