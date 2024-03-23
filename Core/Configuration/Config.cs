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
        public int PanelLeft { get; set; } = 400;
        public int PanelTop { get; set; } = 900;
        public string ServerAddress { get; set; } = "http://94.103.87.38/terminal.php";
        public int ServerRequestTimeoutSec { get; set; } = 15;

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
