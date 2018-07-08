using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.Azure.Databricks.Client
{
    public class LibraryConverter : JsonConverter
    {
        /// <inheritdoc />
        public override bool CanWrite => false;


        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            JObject library = JObject.Load(reader);

            if (library.ContainsKey("jar"))
            {
                return library.ToObject<JarLibrary>();
            }

            if (library.ContainsKey("egg"))
            {
                return library.ToObject<EggLibrary>();
            }

            if (library.ContainsKey("maven"))
            {
                return library.ToObject<MavenLibrary>();
            }

            if (library.ContainsKey("pypi"))
            {
                return library.ToObject<PythonPyPiLibrary>();
            }

            if (library.ContainsKey("cran"))
            {
                return library.ToObject<RCranLibrary>();
            }

            throw new NotSupportedException("Library not recognized");
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(Library).IsAssignableFrom(objectType);
        }
    }
}