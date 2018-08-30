using Discord;
using Discord.WebSocket;
using System;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using RexBot.Commands;
using RexBot.Utilities;

namespace DiscordBot
{
    public class Program
    {
        CommandContent commands = new CommandContent();

        public static void Main(string[] args)
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        public async Task MainAsync()
        {
            commands.LoadData();

            var client = new DiscordSocketClient();

            client.Log += Log;
            client.MessageReceived += MessageReceived;

            string token = Config.Key;
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private async Task MessageReceived(SocketMessage message)
        {
            string prefix = Config.Prefix;

            if (message.Content.HasTrigger())
            {
                MessageInfo messageInfo = commands.ParseMessage(message.Content, message.Author.Mention, message.Channel.Name);
                SendInfo commandReturn = commands.TryCommand(messageInfo);
                if (commandReturn.File)
                {
                    await message.Channel.SendFileAsync(commandReturn.Content);
                }
                else
                {
                    await message.Channel.SendMessageAsync(commandReturn.Content);
                }
            }
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}