using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Tempor.modules
{
    // Create a module with the 'test' prefix
    [Group("test")]
    public class Test : ModuleBase
    {
        // °test square 20 -> 400
        [Command("square"), Summary("Squares a number.")]
        public async Task Square([Summary("The number to square.")] int num)
        {
            // We can also access the channel from the Command Context.
            await Context.Channel.SendMessageAsync($"{num}^2 = {Math.Pow(num, 2)}");
        }

        // ~test userinfo --> foxbot#0282
        // ~test userinfo @Khionu --> Khionu#8708
        // ~test userinfo Khionu#8708 --> Khionu#8708
        // ~test userinfo Khionu --> Khionu#8708
        // ~test userinfo 96642168176807936 --> Khionu#8708
        // ~test whois 96642168176807936 --> Khionu#8708
        [Command("userinfo"), Summary("Returns info about the current user, or the user parameter, if one passed.")]
        [Alias("user", "whois")]
        public async Task UserInfo([Summary("The (optional) user to get info for")] IUser user = null)
        {
            var userInfo = user ?? Context.Client.CurrentUser;
            await ReplyAsync($"{userInfo.Username}#{userInfo.Discriminator}");
        }
        
        // °test say hello -> hello
        [Command("say"), Summary("Echos a message.")]
        public async Task Say([Remainder, Summary("The text to echo")] string echo)
        {
            await ReplyAsync(echo);
        }
        
        // °introduction -> hello
        [Command("introduction"), Summary("Introduces the bot.")]
        public async Task Say()
        {
            await ReplyAsync("Hi, I'm Tempor! A space-themed bot that does (useful) stuff.");
        }
    }

}