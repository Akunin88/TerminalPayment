using Core.EventModels;
using Core.Helpers;
using System.Runtime.Serialization;
using MS = Core.Serializers;

namespace Core.Configuration
{
    public class Config : PropertyObject
    {
        private static string configs_path => Path.Combine(Environment.CurrentDirectory, "CFG");
        private static string configPath => Path.Combine(configs_path, "Config.json");
        private static Config config;

        public int TerminalId { get; set; } = 1234;
        public int DelayForMainMenuReturnSec { get; set; } = 120;

        [IgnoreDataMember] public static string TxtLogFileName => Path.Combine(configs_path, "Log.txt");

        public Config()
        {
            configs_path.CreateDirIfNotExists();
        }

        public static Config Read()
        {
            //config = new Config();
            //Save();
            if (config == null)
                config = MS.JsonSerializer.Deserialize<Config>(configPath);
            return config;
        }

        public static void Save()
        {
            if (config != null)
                MS.JsonSerializer.Serialize(config, configPath);
        }
    }
}
