using System.IO;
using System.Reflection;
using Discord;
using Discord.Audio;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace Tempor
{
    using System;
    using System.Threading.Tasks;

    namespace Tempor
    {
        public class Program
        {
            private CommandService _commands;
            private DiscordSocketClient _client;
            private IServiceProvider _services;
            
            public static void Main(string[] args)
                => new Program().MainAsync().GetAwaiter().GetResult();

            public async Task MainAsync()
            {
                _client = new DiscordSocketClient();
                _commands = new CommandService();

                _client.Log += Log;

                var token = File.ReadAllText("../../token.txt");
                
                _services = new ServiceCollection()
                    .BuildServiceProvider();
                
                await InstallCommands();
                
                await _client.LoginAsync(TokenType.Bot, token);
                await _client.StartAsync();

                // Block this task until the program is closed.
                await Task.Delay(-1);
            }
            
            public async Task InstallCommands()
            {
                // Hook the MessageReceived Event into our Command Handler
                _client.MessageReceived += HandleCommand;
                // Discover all of the commands in this assembly and load them.
                await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
            }

            
            public async Task HandleCommand(SocketMessage messageParam)
            {
                // Don't process the command if it was a System Message
                var message = messageParam as SocketUserMessage;
                if (message == null) return;
                // Create a number to track where the prefix ends and the command begins
                var argPos = 0;
                // Determine if the message is a command, based on if it starts with '!' or a mention prefix
                if (!(message.HasCharPrefix('°', ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))) return;
                // Create a Command Context
                var context = new CommandContext(_client, message);
                // Execute the command. (result does not indicate a return value, 
                // rather an object stating if the command executed successfully)
                Console.WriteLine("Executing: " + message.ToString().Substring(1) + " for: " + messageParam.Author);
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                if (!result.IsSuccess)
                    Console.WriteLine(result.ErrorReason);
                    await context.Channel.SendMessageAsync(result.ErrorReason);
            }
            
            private static Task Log(LogMessage msg)
            {
                Console.WriteLine(msg.ToString());
                return Task.CompletedTask;
            }
        }
    } 

}