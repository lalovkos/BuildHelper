using Newtonsoft.Json;

namespace BuilderHelperOnWPF.Models.SaveModels
{
    [JsonObject]
    internal class ProjectSave
    {
        #region Public Properties

        [JsonProperty("commandLineWorkerSettingsSave")]
        public CommandLineWorkerSettingsSave CommandLineWorkerSettingsSave { get; set; }

        [JsonProperty("pathFinderSave")]
        public PathFinderSave PathFinderSave { get; set; }

        #endregion Public Properties
    }
}