using Newtonsoft.Json;

namespace BuilderHelperOnWPF.Models.SaveModels
{
    [JsonObject]
    public class InterfaceSettings
    {
        #region Public Properties

        [JsonProperty("showBeautifiedCommandLine")]
        public bool ShowBeautifiedCommandLine { get; set; } = true;

        #endregion Public Properties
    }
}