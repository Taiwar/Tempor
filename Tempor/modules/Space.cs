using System.Text;
using System.Threading.Tasks;
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
            var explanation = doc.DocumentNode.SelectSingleNode("//img[@src]").Attributes["src"].Value;
            
            var sb = new StringBuilder();
            var imageUrl = "https://apod.nasa.gov/apod/" + relImageUrl;
            
            await ReplyAsync(imageUrl);
        }
    }
}