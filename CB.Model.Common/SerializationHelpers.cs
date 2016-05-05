using System.IO;
using System.Web.Script.Serialization;
using System.Xml.Serialization;


namespace CB.Model.Common
{
    public class SerializationHelpers
    {
        #region Methods
        public static T ParseJson<T>(string jsonContents)
        {
            var jsonSerializer = new JavaScriptSerializer();
            return jsonSerializer.Deserialize<T>(jsonContents);
        }

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
        #endregion


        #region Implementation
        private static T ParseXml<T>(TextReader reader)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            return (T)xmlSerializer.Deserialize(reader);
        }
        #endregion
    }
}