using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace BuilderHelperOnWPF.Models.SaveModels
{
    [JsonObject]
    public class PathFinderSave
    {
        #region Public Properties

        [JsonProperty("sourceFiles")]
        public List<FileInfo> SourceFiles { get; set; }

        [JsonProperty("targetFolders")]
        public List<FolderNode> TargetFolders { get; set; }

        [JsonProperty("copyFilesWithSamePath")]
        public bool CopyFilesWithSamePath { get; set; }

        [JsonProperty("removeDuplicates")]
        public bool RemoveDuplicates { get; set; }

        #endregion Public Properties
    }
}