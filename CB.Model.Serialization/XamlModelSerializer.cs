using System.Xaml;


namespace CB.Model.Serialization
{
    public class XamlModelSerializer: ModelSerializerBase
    {
        #region Override
        public override T Deserialize<T>(string contents)
            => (T)XamlServices.Parse(contents);

        public override T ReadFromFile<T>(string path)
            => (T)XamlServices.Load(path);

        public override string Serialize<T>(T obj)
            => XamlServices.Save(obj);

        public override void WriteToFile<T>(string path, T obj)
            => XamlServices.Save(path, obj);
        #endregion
    }
}