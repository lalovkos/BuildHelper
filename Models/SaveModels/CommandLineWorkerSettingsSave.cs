using Newtonsoft.Json;

namespace BuilderHelperOnWPF.Models.SaveModels
{
    [JsonObject]
    public class CommandLineWorkerSettingsSave
    {
        #region Public Properties

        [JsonProperty("copyCommandString")]
        public string CopyCommandString { get; set; } = "xcopy /Y /Q ";

        [JsonProperty("iISStartString")]
        public string IISStartString { get; set; } = "iisreset /start localhost";

        [JsonProperty("iISStopString")]
        public string IISStopString { get; set; } = "iisreset /stop /noforce localhost";

        [JsonProperty("restartIIS")]
        public bool RestartIIS { get; set; } = true;

        #endregion Public Properties
    }
}