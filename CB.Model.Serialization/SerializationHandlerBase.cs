using System;
using System.Configuration;
using System.IO;
using System.Reflection;


namespace CB.Model.Serialization
{
    public class SerializationHandlerBase<TObject>
    {
        #region Fields
        private readonly string _defaultFilePath;
        private readonly string _defaultSerializerType;
        private readonly string _fileKey;
        private string _fileValue;
        private ModelSerializerBase _serializer;
        private readonly string _serializerKey;
        #endregion


        #region  Constructors & Destructor
        public SerializationHandlerBase(string fileKey = "file", string serializerKey = "serializer",
            string defaultFilePath = "serialized.dat",
            string defaultSerializerType = "CB.Model.Serialization,CB.Model.Serialization.XmlModelSerializer")
        {
            _fileKey = fileKey;
            _serializerKey = serializerKey;
            _defaultFilePath = defaultFilePath;
            _defaultSerializerType = defaultSerializerType;
        }
        #endregion


        #region Methods
        public TObject Load()
        {
            var file = GetFile();
            var serializer = GetSerializer();
            if (string.IsNullOrEmpty(file) || !File.Exists(file) || serializer == null)
                return default(TObject);

            return serializer.ReadFromFile<TObject>(file);
        }

        public void Save(TObject obj)
        {
            var file = GetFile() ?? SetDefaultFile();

            var directory = Path.GetDirectoryName(file);
            if (directory != null && !Directory.Exists(directory)) Directory.CreateDirectory(directory);

            var serializer = GetSerializer() ?? SetDefaultSerializer();

            serializer.WriteToFile(file, obj);
        }
        #endregion


        #region Implementation
        private static ModelSerializerBase CreateSerializer(string typeString)
        {
            var commaIndex = typeString.IndexOf(",", StringComparison.Ordinal);
            var assemblyName = typeString.Substring(0, commaIndex);
            var typeName = typeString.Substring(commaIndex + 1);
            var asm = Assembly.Load(new AssemblyName(assemblyName));
            return asm.CreateInstance(typeName) as ModelSerializerBase;
        }

        private string GetFile() => _fileValue ?? (_fileValue = GetFileSetting());

        private string GetFileSetting()
            => ConfigurationManager.AppSettings[_fileKey];

        private ModelSerializerBase GetSerializer()
            => _serializer ?? (_serializer = GetSerializerSetting());

        private ModelSerializerBase GetSerializerSetting()
        {
            var setting = ConfigurationManager.AppSettings[_serializerKey];
            return string.IsNullOrEmpty(setting) ? null : CreateSerializer(setting);
        }

        private static void SaveSetting(string key, string value)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Add(key, value);
            config.Save(ConfigurationSaveMode.Modified);
        }

        private string SetDefaultFile()
        {
            _fileValue = _defaultFilePath;
            SaveSetting(_fileKey, _fileValue);
            return _fileValue;
        }

        private ModelSerializerBase SetDefaultSerializer()
        {
            _serializer = CreateSerializer(_defaultSerializerType);
            SaveSetting(_serializerKey, _defaultSerializerType);
            return _serializer;
        }
        #endregion
    }
}