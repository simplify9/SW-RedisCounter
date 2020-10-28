using System;
using System.Collections.Generic;
using System.Text;

namespace SW.RedisCounter
{
    public class RedisOptions
    {
        public const string ConfigurationSection = "Redis";

        public string ApplicationName { get; set; }
        public string Password { get; set; }
        public string Server { get; set; }
    }
}
