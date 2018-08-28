using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
         public static bool IsTrue(string input)
        {
            if (input == "true" || input == "True")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool CategoryAllowed(string input)
        {
            bool returnValue = false;
            foreach (string command in Config.AllowedCategories)
            {
                if (command == input)
                {
                    returnValue = true;
                }
            }
            return returnValue;
        }

        public static bool CommandDisabled(string input)
        {
            bool match = false;
            foreach (string command in Config.DisabledCommands)
            {
                if (command == input)
                {
                    match = true;
                }
            }
            return match;
        }

        public static bool CategoryDisallowed(string input)
        {
            bool returnValue = false;
            foreach (string command in Config.DisallowedCategories)
            {
                if (command == input)
                {
                    returnValue = true;
                }
            }
            return returnValue;
        }
    }

    static class Config
    {
        static string configFile = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "RexBot.conf"));

        static Conf config = JsonConvert.DeserializeObject<Conf>(configFile);

        public static string Key = config.Key;

        public static string[] AllowedCategories = config.AllowedCategories;

        public static string[] DisallowedCategories = config.DisallowedCategories;

        public static bool AllowHardcoded = config.AllowHardcoded;

        public static string Name = config.Name;

        public static string Prefix = config.Prefix;

        public static string ControlChannel = config.ControlChannel;

        public static string[] DisabledCommands = config.DisabledCommands;
    }
}
