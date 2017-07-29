using System.IO;
using System.Net;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Newtonsoft.Json.Linq;

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
            var apiKey = File.ReadAllText("../../api_key.txt");
            var url = "https://api.nasa.gov/planetary/apod?api_key=" + apiKey;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "text/json";
            JObject response;
            
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                response = JObject.Parse(streamReader.ReadToEnd());    
            }

            var explanation = response["explanation"];	
            var imageUrl = response["url"];
            var title = response["title"];
            
            var embed = new EmbedBuilder();
            embed.WithTitle("APOD: " + title);
            embed.WithColor(new Color(0x5324b5));
            embed.WithDescription(explanation.ToString());
            embed.WithImageUrl(imageUrl.ToString());
            embed.WithCurrentTimestamp();
            embed.WithThumbnailUrl("https://upload.wikimedia.org/wikipedia/commons/thumb/e/e5/NASA_logo.svg/200px-NASA_logo.svg.png");

            await Context.Message.DeleteAsync();
            await Context.Channel.SendMessageAsync("", false, embed);
        }
    }
}