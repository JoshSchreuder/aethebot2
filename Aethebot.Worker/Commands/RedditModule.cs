using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;

namespace Aethebot.Worker.Commands
{
    public class RedditModule : ModuleBase<SocketCommandContext>
    {
        private HttpClient _client;

        public RedditModule(IHttpClientFactory factory)
        {
            _client = factory.CreateClient("reddit");
        }

        [Command("vreddit")]
        [Summary("Reupload shitty Reddit video links")]
        public Task VReddit(string url)
        {
            var dashPlaylist = new Uri($"{url}/DASHPlaylist.mpd");
            return GetDashPlaylist(new Uri(url), dashPlaylist);
        }

        private async Task<List<PlaylistSegment>> GetDashPlaylist(Uri baseUri, Uri playlistUrl)
        {
            var segments = new List<PlaylistSegment>();
            var content = await _client.GetStringAsync(playlistUrl);
            var xml = new XmlDocument();
            xml.LoadXml(content);

            var periods = xml["MPD"]?["Period"];
            if (periods == null)
            {
                return segments;
            }

            var adaptionSets = periods.GetElementsByTagName("AdaptationSet");
            if (adaptionSets == null)
            {
                return segments;
            }

            foreach (XmlElement adaptionSet in adaptionSets)
            {
                var representations = adaptionSet.GetElementsByTagName("Representation");
                if (representations == null)
                {
                    continue;
                }

                foreach (XmlNode representation in representations)
                {
                    segments.Add(new PlaylistSegment
                    {
                        Bandwidth = int.Parse(representation.Attributes["bandwidth"]?.Value, CultureInfo.InvariantCulture),
                        Codecs = representation.Attributes["codecs"]?.Value,
                        MimeType = adaptionSet.Attributes["mimeType"]?.Value,
                        Uri = new Uri($"{baseUri}/{representation["BaseURL"]?.InnerText}")
                    });
                }
            }

            return segments;
        }

        internal class PlaylistSegment
        {
            internal int Bandwidth { get; set; }
            internal string Codecs { get; set; }
            internal string MimeType { get; set; }
            internal Uri Uri { get; set; }
        }
    }
}
