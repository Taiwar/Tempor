using System;
using System.Net.Http;
using System.Threading.Tasks;
using Discord.Commands;
using Newtonsoft.Json.Linq;

namespace Tempor.modules
{
    [Group("countdown")]
    public class Countdown : ModuleBase
    {
        // °countdown bytitle -> *countdown info*
        [Command("bytitle"), Summary("Searches and posts a Muellersites countdown with the given name.")]
        public async Task ByTitle([Summary("A countdown title or the start of one.")] string title)
        {
            const string url = "https://muellersites.net/countdown/by_title/";
            var client = new HttpClient();
            
            var responseString = await client.GetStringAsync(url + "?title=" + title);
            
            Console.WriteLine(responseString);
            var joResponse = JObject.Parse(responseString);
            var id = Convert.ToInt32(joResponse["id"][0]);
            var fullTitle = Convert.ToString(joResponse["title"][0]);
            var endDate = Convert.ToString(joResponse["date"][0]);

            var cdownUrl = "https://muellersites.net/countdown/view/" + id;

            /*var reply = "```" +
                        "Title: " + fullTitle + "\n" +
                        "Ends: " + endDate + "\n" +
                        "Url: " + cdownUrl + "\n" +
                        "```";
                        */

            var reply = "__**" + fullTitle + "**__\n\n" +
                        "*Ends:* " + endDate + "\n" +
                        "*Url:* " + cdownUrl + "\n";

            await ReplyAsync(reply);
        }
        
        // °countdown byid -> *countdown info*
        [Command("byid"), Summary("Searches and posts a Muellersites countdown with the given id.")]
        public async Task ById([Summary("A countdown id")] string id)
        {
            const string url = "https://muellersites.net/countdown/by_id/";
            var client = new HttpClient();
            
            var responseString = await client.GetStringAsync(url + "?id=" + id);
            
            Console.WriteLine(responseString);
            var joResponse = JObject.Parse(responseString);
            var fullTitle = Convert.ToString(joResponse["title"]);
            var endDate = Convert.ToString(joResponse["date"]);

            var cdownUrl = "https://muellersites.net/countdown/view/" + id;

            /*var reply = "```" +
                        "Title: " + fullTitle + "\n" +
                        "Ends: " + endDate + "\n" +
                        "Url: " + cdownUrl + "\n" +
                        "```";
                        */

            var reply = "__**" + fullTitle + "**__\n\n" +
                        "*Ends:* " + endDate + "\n" +
                        "*Url:* " + cdownUrl + "\n";

            await ReplyAsync(reply);
        }
    }
}