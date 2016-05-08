using System.IO;
using System.Web.Script.Serialization;
using System.Xaml;
using System.Xml.Serialization;


namespace CB.Model.Common
{
    public interface ISerializeModel
    {
        #region Abstract
        T Deserialize<T>(string contents);
        string Serialize<T>(T obj);
        #endregion
    }

    public interface IPersistModel
    {
        #region Abstract
        T ReadFromFile<T>(string path);
        void WriteToFile<T>(string path, T obj);
        #endregion
    }

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

    public class SerializationHelpers
    {
        #region Methods
        public static T ParseJson<T>(string jsonContents)
            => CreateJsonSerializer<T>().Deserialize<T>(jsonContents);

        public static T ParseJsonFile<T>(string jsonFile)
            => ParseJson<T>(File.ReadAllText(jsonFile));

        public static T ParseXml<T>(string xmlContents)
        {
            using (var reader = new StringReader(xmlContents))
                return ParseXml<T>(reader);
        }

        public static T ParseXmlFile<T>(string file)
        {
            using (var reader = new StreamReader(file))
                return ParseXml<T>(reader);
        }

        public static string ToJson<T>(T obj)
        {
            var jsonSerializer = CreateJsonSerializer<T>();
            return jsonSerializer.Serialize(obj);
        }

        public static void ToJsonFile<T>(T obj, string jsonFile)
            => File.WriteAllText(jsonFile, ToJson(obj));
        #endregion


        #region Implementation
        private static JavaScriptSerializer CreateJsonSerializer<T>()
            => new JavaScriptSerializer();

        private static T ParseXml<T>(TextReader reader)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            return (T)xmlSerializer.Deserialize(reader);
        }
        #endregion
    }
}