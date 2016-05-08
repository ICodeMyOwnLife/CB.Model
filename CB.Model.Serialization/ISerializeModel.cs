namespace CB.Model.Serialization
{
    public interface ISerializeModel
    {
        #region Abstract
        T Deserialize<T>(string contents);
        string Serialize<T>(T obj);
        #endregion
    }
}