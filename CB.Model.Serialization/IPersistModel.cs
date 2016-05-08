namespace CB.Model.Serialization
{
    public interface IPersistModel
    {
        #region Abstract
        T ReadFromFile<T>(string path);
        void WriteToFile<T>(string path, T obj);
        #endregion
    }
}