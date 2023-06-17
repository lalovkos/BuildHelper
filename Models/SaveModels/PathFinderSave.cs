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
        public List<FileInfo> SourceFiles { get; set; } = new List<FileInfo>();

        [JsonProperty("targetFolders")]
        public List<FolderNode> TargetFolders { get; set; } = new List<FolderNode>();

        [JsonProperty("copyFilesWithSamePath")]
        public bool CopyFilesWithSamePath { get; set; } = false;

        [JsonProperty("removeDuplicates")]
        public bool RemoveDuplicates { get; set; } = true;

        #endregion Public Properties
    }
}