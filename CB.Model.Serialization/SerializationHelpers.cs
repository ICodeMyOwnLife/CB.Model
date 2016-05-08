using System;
using System.IO;
using System.Web.Script.Serialization;
using System.Xml.Serialization;


namespace CB.Model.Serialization
{
    public class SerializationHelpers

    {
        #region Methods
        public static T Load<T>(string filePath, SerializationType type)
        {
            IPersistModel modelPersister;
            switch (type)
            {
                case SerializationType.Json:
                    modelPersister = new JsonModelSerializer();
                    break;
                case SerializationType.Xml:
                    modelPersister = new XmlModelSerializer();
                    break;
                case SerializationType.Xaml:
                    modelPersister = new XamlModelSerializer();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            return modelPersister.ReadFromFile<T>(filePath);
        }

        public static T Load<T>(string filePath)
            => Load<T>(filePath, GetSerializationType(filePath));

        public static T ParseJson<T>(string jsonContents) => CreateJsonSerializer<T>().Deserialize<T>(jsonContents);

        public static T ParseJsonFile<T>(string jsonFile) => ParseJson<T>(File.ReadAllText(jsonFile));

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

        public static void ToJsonFile<T>(T obj, string jsonFile) => File.WriteAllText(jsonFile, ToJson(obj));
        #endregion


        #region Implementation
        private static JavaScriptSerializer CreateJsonSerializer<T>() => new JavaScriptSerializer();

        private static SerializationType GetSerializationType(string filePath)
        {
            var extension = Path.GetExtension(filePath);
            switch (extension?.ToLower())
            {
                case ".json":
                    return SerializationType.Json;
                case ".xml":
                    return SerializationType.Xml;
                case ".xaml":
                    return SerializationType.Xaml;
                default:
                    throw new NotSupportedException(extension);
            }
        }

        private static T ParseXml<T>(TextReader reader)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            return (T)xmlSerializer.Deserialize(reader);
        }
        #endregion
    }

    public enum SerializationType
    {
        Json,
        Xml,
        Xaml
    }
}