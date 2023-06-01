namespace BuilderHelperOnWPF.Models
{
    public interface ISaveable<T>
    {
        #region Public Methods

        T GetSave();

        void LoadFromSave(T save);

        #endregion Public Methods
    }
}