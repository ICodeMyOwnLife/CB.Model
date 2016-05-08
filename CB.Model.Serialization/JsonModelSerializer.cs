using System.Web.Script.Serialization;


namespace CB.Model.Serialization
{
    public class JsonModelSerializer: ModelSerializerBase
    {
        #region Fields
        private readonly JavaScriptSerializer _serializer = new JavaScriptSerializer();
        #endregion


        #region Override
        public override T Deserialize<T>(string contents)
            => _serializer.Deserialize<T>(contents);

        public override string Serialize<T>(T obj)
            => _serializer.Serialize(obj);
        #endregion
    }
}