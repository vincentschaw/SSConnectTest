using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SSConnectTest
{
    public class SSConfig
    {
        [JsonProperty("server")]
        public string Server { get; set; }

        [JsonProperty("server_port")]
        public string ServerPort { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("plugin")]
        public string Plugin { get; set; }

        [JsonProperty("plugin_opts")]
        public string PluginOpts { get; set; }

        [JsonProperty("plugin_args")]
        public string PluginArgs { get; set; }

        [JsonProperty("remarks")]
        public string Remarks { get; set; }

        [JsonProperty("timeout")]
        public int Timeout { get; set; }
    }
}
