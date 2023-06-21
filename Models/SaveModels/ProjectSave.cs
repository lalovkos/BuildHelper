using Newtonsoft.Json;

namespace BuilderHelperOnWPF.Models.SaveModels
{
    [JsonObject]
    public class ProjectSave
    {
        #region Public Properties

        [JsonProperty("commandLineWorkerSettingsSave")]
        public CommandLineWorkerSettingsSave CommandLineWorkerSettingsSave { get; set; } = new CommandLineWorkerSettingsSave();

        [JsonProperty("interfaceSettings")]
        public InterfaceSettings InterfaceSettings { get; set; } = new InterfaceSettings();

        [JsonProperty("pathFinderSave")]
        public PathFinderSave PathFinderSave { get; set; } = new PathFinderSave();

        #endregion Public Properties
    }
}