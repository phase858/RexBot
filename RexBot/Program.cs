using Discord;
using Discord.WebSocket;
using System;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using RexBot.CommandContent;
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

            if (message.Content.Substring(0,prefix.Length) == prefix)
            {
                string content = message.Content.Remove(0, prefix.Length);
                if (Utils.CommandDisabled(content.Split(new[] { ' ' }, 2)[0]))
                {
                    await message.Channel.SendMessageAsync("Command disabled.");
                }
                else
                {
                    if (content.Split(new[] { ' ' }, 2)[0] == "dieroll" & Config.AllowHardcoded)
                    {
                        string commandReturn = commands.DieRoll(message.Content.Split(new[] { ' ' }, 2)[1]);
                        await message.Channel.SendMessageAsync(commandReturn);
                    }
                    
                    else if (content.Split(new[] { ' ' }, 2)[0] == "flip" & Config.AllowHardcoded)
                    {
                        string commandReturn = commands.Flip(message.Content.Split(new[] { ' ' }, 2)[1]);
                        await message.Channel.SendMessageAsync(commandReturn);
                    }

                    else if (content == "list" & Config.AllowHardcoded)
                    {
                        string commandReturn = commands.List();
                        await message.Channel.SendMessageAsync(commandReturn);
                    }

                    else if (content == "reload")
                    {
                        if (message.Channel.Name == Config.ControlChannel)
                        {
                            commands.LoadData();
                            await message.Channel.SendMessageAsync("Command files reloaded.");
                        }
                        else
                        {
                            await message.Channel.SendMessageAsync("Command not sent in bot control channel.");
                        }                       
                    }
                    else
                    {
                        string name = content.Split(new[] { ' ' }, 2)[0];
                        string messageContent = content;
                        if (messageContent.Contains(" "))
                        {
                            messageContent = content.Split(new[] { ' ' }, 2)[1];
                        }
                        else
                        {
                            messageContent = "placeholderText";
                        }
                        string username = message.Author.Mention;
                        string botname = Config.Name;;
                        string commandReturn = commands.TryCommand(name, messageContent, username, botname);
                        await message.Channel.SendMessageAsync(commandReturn);
                    }
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