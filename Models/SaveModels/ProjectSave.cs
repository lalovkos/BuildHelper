using Newtonsoft.Json;

namespace BuilderHelperOnWPF.Models.SaveModels
{
    [JsonObject]
    public class ProjectSave
    {
        #region Public Properties
        [JsonProperty("interfaceSettings")]
        public InterfaceSettings InterfaceSettings { get; set; }

        [JsonProperty("commandLineWorkerSettingsSave")]
        public CommandLineWorkerSettingsSave CommandLineWorkerSettingsSave { get; set; }

        [JsonProperty("pathFinderSave")]
        public PathFinderSave PathFinderSave { get; set; }

        #endregion Public Properties
    }
}