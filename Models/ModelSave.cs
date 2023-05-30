using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace BuilderHelperOnWPF.Models
{
    [JsonObject]
    public class ModelSave
    {
        #region Public Properties

        [JsonProperty("sourceFiles")]
        public List<FileInfo> SourceFiles { get; set; }

        [JsonProperty("targetFolders")]
        public List<FolderNode> TargetFolders { get; set; }

        #endregion Public Properties
    }
}