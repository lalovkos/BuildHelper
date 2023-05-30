using System.Windows.Controls;

namespace BuilderHelperOnWPF.Models
{
    internal interface ISaveable<T>
    {
        void LoadFromSave(T save);
        T GetSave();
    }
}