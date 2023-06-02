using Newtonsoft.Json;

namespace BuilderHelperOnWPF.Models.SaveModels
{
    [JsonObject]
    public class CommandLineWorkerSettingsSave
    {
        #region Public Properties

        [JsonProperty("copyCommandString")]
        public string CopyCommandString { get; set; }

        [JsonProperty("iISStartString")]
        public string IISStartString { get; set; }

        [JsonProperty("iISStopString")]
        public string IISStopString { get; set; }

        [JsonProperty("restartIIS")]
        public bool RestartIIS { get; set; }

        #endregion Public Properties
    }
}