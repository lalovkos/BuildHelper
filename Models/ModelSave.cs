using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace BuilderHelperOnWPF.Models
{
    [JsonObject]
    public class ModelSave
    {
        [JsonProperty("sourceFiles")]
        public List<FileInfo> SourceFiles { get; set; }

        [JsonProperty("targetFolders")]
        public List<FolderNode> TargetFolders { get; set; }
    }
}