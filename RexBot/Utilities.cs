using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace RexBot.Utilities
{
    class Conf
    {
        public string Key { get; set; }
        public string[] AllowedCategories { get; set; }
        public string[] DisallowedCategories { get; set; }
        public bool AllowHardcoded { get; set; }
        public bool AllowSay { get; set; }
        public string Name { get; set; }
        public string Prefix { get; set; }
        public string ControlChannel { get; set; }
        public string[] DisabledCommands { get; set; }
    }
    class ConfigSettings
    {

    }
    public static class Utils
    {
        public static bool CategoryAllowed(string input)
        {
            return Config.AllowedCategories.Contains(input);
        }

        public static bool CommandDisabled(string input)
        {
            return Config.DisabledCommands.Contains(input);
        }

        public static bool CategoryDisallowed(string input)
        {
            return Config.DisallowedCategories.Contains(input);
        }
    }

    static class Config
    {
        static string configFile = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "RexBot.conf"));

        static Conf config = JsonConvert.DeserializeObject<Conf>(configFile);

        public static string Key = config.Key;

        public static List<string> AllowedCategories = config.AllowedCategories.ToList();

        public static List<string> DisallowedCategories = config.DisallowedCategories.ToList();

        public static bool AllowHardcoded = config.AllowHardcoded;

        public static string Name = config.Name;

        public static string Prefix = config.Prefix;

        public static string ControlChannel = config.ControlChannel;

        public static List<string> DisabledCommands = config.DisabledCommands.ToList();
    }
}
