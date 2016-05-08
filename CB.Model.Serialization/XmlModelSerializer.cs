using System.IO;
using System.Xml.Serialization;


namespace CB.Model.Serialization
{
    public class XmlModelSerializer: ModelSerializerBase
    {
        #region Override
        public override T Deserialize<T>(string contents)
        {
            using (var reader = new StringReader(contents))
            {
                return Deserialize<T>(reader);
            }
        }

        public override T ReadFromFile<T>(string path)
        {
            using (var reader = new StreamReader(path))
            {
                return Deserialize<T>(reader);
            }
        }

        public override string Serialize<T>(T obj)
        {
            using (var writer = new StringWriter())
            {
                Serialize(writer, obj);
                return writer.ToString();
            }
        }

        public override void WriteToFile<T>(string path, T obj)
        {
            using (var writer = new StreamWriter(path))
            {
                Serialize(writer, obj);
            }
        }
        #endregion


        #region Implementation
        private static XmlSerializer CreateSerializer<T>()
            => new XmlSerializer(typeof(T));

        private static T Deserialize<T>(TextReader reader)
        {
            var serializer = CreateSerializer<T>();
            return (T)serializer.Deserialize(reader);
        }

        private static void Serialize<T>(TextWriter writer, T obj)
        {
            var serializer = CreateSerializer<T>();
            serializer.Serialize(writer, obj);
        }
        #endregion
    }
}