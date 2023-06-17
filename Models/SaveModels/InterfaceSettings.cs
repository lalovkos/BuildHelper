using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace BuilderHelperOnWPF.Models.SaveModels
{
    [JsonObject]
    public class InterfaceSettings
    {
        [JsonProperty("showBeautifiedCommandLine")]
        public bool ShowBeautifiedCommandLine { get; set; } = true;
    }
}