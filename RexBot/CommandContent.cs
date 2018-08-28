using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RexBot.Utilities;
using Newtonsoft.Json;

namespace RexBot.CommandContent
{
    public class Command
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string[] Content { get; set; }
    }

    class CommandContent
    {
        Random randGen = new Random();

        string[] subsFile = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "Subs.conf"));

        string[] commandFiles = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "Commands"));

        Dictionary<string, Command> commands = new Dictionary<string, Command>();

        Dictionary<char, string> subsitutes = new Dictionary<char, string>();

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

                if (Utils.CategoryAllowed(category))
                {
                    Console.WriteLine("Command {0} is in an allowed category, adding.\n", name);
                    commands.Add(name, command);
                }
                else if (Utils.CategoryDisallowed(category))
                {
                    Console.WriteLine("Command {0} is not in an allowed category, skiping.\n", name);
                }
                else
                {
                    Console.WriteLine("Commands {0} has no valid category, skiping.\n", name);
                }

            }
        }

        public string TryCommand(string commandName, string content, string username, string botname)
        {
            if (commands.ContainsKey(commandName))
            {
                Command command = commands[commandName];
                string category = command.Category;
                int contentLength = command.Content.Length;
                if (contentLength < 1)
                {
                    return "No options for command";
                }
                else
                {
                    int choice = rnd.Next(0, contentLength - 1);

                    string contentText = command.Content[choice];

                    if (contentText.Contains("_INPUT_") & content == "placeholderText")
                    {
                        return "Invalid usage.";
                    }
                    else
                    {
                        return contentText.Replace("_USERNAME_", username).Replace("_BOTNAME_", botname).Replace("_INPUT_", content);
                    }           
                }
            }
            else
            {
                return "Command not found.";
            }
        }

        public string Flip(string input)
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
            return output;
        }

        public string DieRoll(string input)
        {
            int sides;
            bool isNumeric = int.TryParse(input, out sides);
            if (isNumeric && sides > 1)
            {
                return "**Rolled: " + randGen.Next(1, sides).ToString() + "**";
            }
            else
            {
                return "Please try again.";
            }
        }

        public string List()
        {
            List<string> commandList = new List<string>();

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

            string commandListStr = string.Join(", ", commandList.ToArray());

            return "**The following commands are available: **" + commandListStr;
        }
    }
}
