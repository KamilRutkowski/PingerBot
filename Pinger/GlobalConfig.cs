using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Discord;
using Discord.Audio;

namespace Pinger
{
    public static class GlobalConfig
    {
        public static IObservable<bool> CommandsUpdated { get; set; }
        public static string BotToken
        {
            get
            {
                return _confObj.BotToken;
            }
            set
            {
                _confObj.BotToken = value;
            }
        }
        public static List<CommandObject> Commands
        {
            get => _confObj.Commands;
            set
            {
                _confObj.Commands = value;
                SerializeData();
                _sub.OnNext(true);
            }
        }

        public static AudioService AudioService { get; set; }

        private static void SerializeData()
        {
            using (StreamWriter sw = new StreamWriter(_fileSaveLocation)) {
                using (JsonWriter jw = new JsonTextWriter(sw))
                {
                    _serializer.Serialize(jw, _confObj);
                }
             }
        }

        private static void DeserializeData()
        {
            using (StreamReader sr = new StreamReader(_fileSaveLocation))
            {
                using (JsonReader jr = new JsonTextReader(sr))
                {
                    _confObj = JsonConvert.DeserializeObject<ConfigurationObject>(_serializer.Deserialize(jr).ToString());
                }
            }
        }

        
        private static string _fileSaveLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "configuration.json");
        private static JsonSerializer _serializer;
        private static ConfigurationObject _confObj;
        private static Subject<bool> _sub;

        static GlobalConfig()
        {
            _serializer = new JsonSerializer();
            DeserializeData();
            _sub = new Subject<bool>();
            CommandsUpdated = _sub.AsObservable();
            AudioService = new AudioService();
        }
    }
}
