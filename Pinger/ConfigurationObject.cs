using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinger
{
    public class ConfigurationObject
    {
        public string BotToken { get; set; }
        public List<CommandObject> Commands;

        public ConfigurationObject()
        {
            Commands = new List<CommandObject>();
        }
    }
}
