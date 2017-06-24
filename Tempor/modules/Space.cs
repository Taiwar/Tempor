using System;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using HtmlAgilityPack;

namespace Tempor.modules
{
    [Group("space")]
    public class Space : ModuleBase
    {
        // °space pictureoftheday -> *image url*
        [Command("pictureoftheday"), Summary("Posts the NASA Astronomy picture of the day.")]
        [Alias("apod", "potd", "astronomypictureoftheday")]
        public async Task Potd()
        {
            const string url = "https://apod.nasa.gov/apod/astropix.html";
            var web = new HtmlWeb();
            var doc = web.Load(url);

            var relImageUrl = doc.DocumentNode.SelectSingleNode("//img[@src]").Attributes["src"].Value;
            var root = doc.DocumentNode.SelectSingleNode("//b[.=' Explanation: ']").ParentNode;
            var ex = new StringBuilder();
			
            foreach (var node in root.ChildNodes)
            {
                if (!node.HasChildNodes)
                {
                    var text = node.InnerText;
                    if (!string.IsNullOrEmpty(text))
                        ex.AppendLine(text.Trim());
                } else {
                    foreach (var innerNode in node.ChildNodes)
                    {
                        var text = innerNode.InnerText;
                        if (!string.IsNullOrEmpty(text))
                            ex.AppendLine(text.Trim());
                    }
                }
            }
            var explanation = ex.ToString();
						
            var imageUrl = "https://apod.nasa.gov/apod/" + relImageUrl;
		
            var indexExplanation = explanation.IndexOf("Explanation", StringComparison.Ordinal);
            explanation = explanation.Substring(indexExplanation, explanation.Length - indexExplanation);
		
            var indexTomorrow = explanation.IndexOf("Tomorrow", StringComparison.Ordinal);
            explanation = explanation.Substring(0, indexTomorrow);
            
            var embed = new EmbedBuilder();
            embed.WithTitle("Astronomy Picture of the Day");
            embed.WithColor(new Color(0x5324b5));
            embed.WithDescription(explanation);
            embed.WithImageUrl(imageUrl);
            embed.WithCurrentTimestamp();
            embed.WithThumbnailUrl("https://upload.wikimedia.org/wikipedia/commons/thumb/e/e5/NASA_logo.svg/200px-NASA_logo.svg.png");
            
            await Context.Channel.SendMessageAsync("", false, embed);
        }
    }
}