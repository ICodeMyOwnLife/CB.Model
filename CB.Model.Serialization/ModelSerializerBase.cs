using System.IO;


namespace CB.Model.Serialization
{
    public abstract class ModelSerializerBase: ISerializeModel, IPersistModel
    {
        #region Abstract
        public abstract T Deserialize<T>(string contents);
        public abstract string Serialize<T>(T obj);
        #endregion


        #region Methods
        public virtual T ReadFromFile<T>(string path)
            => Deserialize<T>(File.ReadAllText(path));

        public virtual void WriteToFile<T>(string path, T obj)
            => File.WriteAllText(path, Serialize(obj));
        #endregion
    }
}