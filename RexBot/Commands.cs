using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RexBot.Utilities;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace RexBot.Commands
{
    public class Command
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string[] Content { get; set; }
    }

    public class MessageInfo
    {
        public string Command { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public string Channel { get; set; }
    }

    public class SendInfo
    {
        public string Content { get; set; }
        public bool File { get; set; }
    }

    public static class Extensions
    {
        public static string KeywordReplace(this string input, string author, string content)
        {
            return input.Replace("_AUTHOR_", author).Replace("_BOT_", Config.Name).Replace("_CONTENT_", content);
        }

        public static bool HasTrigger(this string input)
        {
            if (input.Substring(0, Config.Prefix.Length) == Config.Prefix)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    class CommandContent
    {
        Random randGen = new Random();

        string[] subsFile = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "Subs.conf"));

        string[] commandFiles = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "Commands"), "*.json");

        Dictionary<string, Command> commands = new Dictionary<string, Command>();

        Dictionary<char, string> subsitutes = new Dictionary<char, string>();

        List<string> commandList = new List<string>();

        Random rnd = new Random();

        string name = Config.Name;

        public void LoadData()
        {
            foreach (string line in subsFile)
            {
                string[] splitLine = line.Split('|');
                char main = splitLine[0][0];
                string sub = splitLine[1];
                subsitutes.Add(main, sub);
            }

            foreach (string file in commandFiles)
            {
                string content = File.ReadAllText(file);

                Command command = JsonConvert.DeserializeObject<Command>(content);

                string name = command.Name;

                string category = command.Category;

                if (Config.CategoryAllowed(category))
                {
                    Console.WriteLine("Command {0} is in an allowed category, adding.", name);
                    commands.Add(name, command);
                }
                else if (Config.CategoryDisallowed(category))
                {
                    Console.WriteLine("Command {0} is not in an allowed category, skiping.", name);
                }
                else
                {
                    Console.WriteLine("Commands {0} has no valid category, skiping.", name);
                }

            }

            foreach (KeyValuePair<string, Command> entry in commands)
            {
                name = entry.Value.Name;
                commandList.Add(name);
            }

            if (Config.AllowHardcoded)
            {
                commandList.Add("flip");
                commandList.Add("dieroll");
            }

            commandList.Add("reload");
            commandList.Add("list");

            for (int i = 0; i < commandList.Count; i++)
            {
                string name = commandList[i];
                if (Config.CommandDisabled(name))
                {
                    int index = commandList.IndexOf(name);
                    commandList.RemoveAt(index);
                }
            }
        }

        public MessageInfo ParseMessage(string messageText, string author, string channel)
        {
            messageText = messageText.Remove(0, Config.Prefix.Length);
            string name = String.Empty;
            string content = String.Empty;
            if (messageText.Contains(" "))
            {
                string[] parts = messageText.Split(new[] { ' ' }, 2);
                name = parts[0];
                if (parts[1] == string.Empty)
                {
                    content = "PlAcEhOlDeReXt";
                }
                else
                {
                    content = parts[1];
                }
            }
            else
            {
                name = messageText;
                content = "PlAcEhOlDeReXt";
            }
            MessageInfo result = new MessageInfo
            {
                Command = name,
                Content = content,
                Author = author,
                Channel = channel
            };
            return result;
        }

        public SendInfo TryCommand(MessageInfo message)
        {
            string name = message.Command;
            SendInfo returnText;
           if (commandList.Contains(name))
            {
                switch (name)
                {
                    case "dieroll":
                        returnText = DieRoll(message.Content);
                        break;
                    case "flip":
                        returnText = Flip(message.Content);
                        break;
                    case "list":
                        returnText = List();
                        break;
                    case "reload":
                        returnText = Reload(message);
                        break;
                    default:
                        returnText = TryCC(message);
                        break;
                }
                return returnText;
            }
            else
            {
                SendInfo info = new SendInfo
                {
                    Content = "Command does not exist or is not enabled.",
                    File = false
                };
                return info;
            }


        }

        public SendInfo TryCC(MessageInfo message)
        {
            Regex isFileCommand = new Regex(@"_FILE[^_]+_");
            Regex extractName = new Regex(@"(?<=:)[^_]+");         
            string botName = Config.Name;
            Command command = commands[message.Command];
            string category = command.Category;
            int contentLength = command.Content.Length;
            int choice = rnd.Next(0, contentLength - 1);
            if (contentLength < 1)
            {
                SendInfo info = new SendInfo
                {
                    Content = "No options for command",
                    File = false
                };
                return info;
            }
            else
            {

                string contentText = command.Content[choice];
                Console.WriteLine("options: " + command.Content.Length);
                Console.WriteLine("chose: ", choice);
                Match isFile = isFileCommand.Match(contentText);

                if (isFile.Success) {
                    Console.WriteLine("Commands is a file command.");
                    Console.WriteLine(isFile.Value);
                }

                if (contentText.Contains("_CONTENT_") & message.Content == "PlAcEhOlDeReXt")
                {
                    SendInfo info = new SendInfo
                    {
                        Content = "Invalid usage.",
                        File = false
                    };
                    return info;
                }
                else if (isFile.Success)
                {
                    string files = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "Files");
                    string fileName = string.Empty;
                    string matchText = isFile.Value;
                    Match name = extractName.Match(matchText);
                    if (name.Success)
                    {
                        fileName = name.Value;
                    }
                    string filePath = Path.Combine(files, fileName);
                    if (File.Exists(filePath))
                    {
                        SendInfo info = new SendInfo
                        {
                            Content = filePath,
                            File = true
                        };
                        return info;
                    }
                    else
                    {
                        SendInfo info = new SendInfo
                        {
                            Content = "File does not exist.",
                            File = false
                        };
                        return info;
                    }
                }
                else
                {
                    SendInfo info = new SendInfo
                    {
                        Content = contentText.KeywordReplace(message.Author, message.Content),
                        File = false
                    };
                    return info;
                }

            }
        }

        public SendInfo Flip(string input)
        {
            if (input == "PlAcEhOlDeReXt")
            {
                SendInfo info = new SendInfo
                {
                    Content = "Invalid usage.",
                    File = false
                };
                return info;
            }
            else
            {
                int index = 0;
                char[] outputArray = input.ToCharArray();
                Array.Reverse(outputArray);
                string output = new string(outputArray);
                var outputBuilder = new StringBuilder(output);
                foreach (char c in output)
                {
                    if (subsitutes.ContainsKey(c))
                    {
                        outputBuilder.Remove(index, 1);
                        outputBuilder.Insert(index, subsitutes[c]);
                        output = outputBuilder.ToString();
                    }
                    index++;
                }
                SendInfo info = new SendInfo
                {
                    Content = output,
                    File = false
                };
                return info;
            }
        }

        public SendInfo DieRoll(string input)
        {
            int sides;
            bool isNumeric = int.TryParse(input, out sides);
            if (isNumeric && sides > 1)
            {
                SendInfo info = new SendInfo
                {
                    Content = "**Rolled: " + randGen.Next(1, sides).ToString() + "**",
                    File = false
                };
                return info;
            }
            else
            {
                SendInfo info = new SendInfo
                {
                    Content = "Please try again.",
                    File = false
                };
                return info;
            }
        }

        public SendInfo List()
        {
            string commandListStr = string.Join(", ", commandList.ToArray());

            SendInfo info = new SendInfo
            {
                Content = "**The following commands are available: **" + commandListStr,
                File = false
            };
            return info;
        }

        public SendInfo Reload(MessageInfo message)
        {
            if (message.Channel == Config.ControlChannel)
            {
                LoadData();
                SendInfo info = new SendInfo
                {
                    Content = "Custom commands reloaded.",
                    File = false
                };
                return info;

            }
            else
            {
                SendInfo info = new SendInfo
                {
                    Content = "Message not sent in bot control channel.",
                    File = false
                };
                return info;
            }
        }
    }
}
