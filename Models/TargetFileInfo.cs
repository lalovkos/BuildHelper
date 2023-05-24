namespace BuilderHelperOnWPF.Models
{
    public class TargetFileInfo
    {
        public string FullPath { get; set; }

        public TargetFileInfo(string fullPath)
        {
            FullPath = fullPath;
        }
    }
}