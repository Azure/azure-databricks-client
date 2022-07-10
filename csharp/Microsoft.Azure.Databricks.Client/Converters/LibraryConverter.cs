using Microsoft.Azure.Databricks.Client.Models;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Converters
{
    public class LibraryConverter : JsonConverter<Library>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(Library).IsAssignableFrom(typeToConvert);
        }

        public override bool HandleNull => true;

        public override Library Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var library = JsonDocument.ParseValue(ref reader).RootElement;
            
            if (library.TryGetProperty("jar", out _))
            {
                return library.Deserialize<JarLibrary>();
            }

            if (library.TryGetProperty("egg", out _))
            {
                return library.Deserialize<EggLibrary>();
            }

            if (library.TryGetProperty("whl", out _))
            {
                return library.Deserialize<WheelLibrary>();
            }

            if (library.TryGetProperty("maven", out _))
            {
                return library.Deserialize<MavenLibrary>();
            }

            if (library.TryGetProperty("pypi", out _))
            {
                return library.Deserialize<PythonPyPiLibrary>();
            }

            if (library.TryGetProperty("cran", out _))
            {
                return library.Deserialize<RCranLibrary>();
            }

            throw new NotSupportedException("Library not recognized");
        }

        public override void Write(Utf8JsonWriter writer, Library value, JsonSerializerOptions options)
        {
            var node = value switch
            {
                JarLibrary jar => JsonSerializer.SerializeToNode(jar),
                EggLibrary egg => JsonSerializer.SerializeToNode(egg),
                WheelLibrary wheel => JsonSerializer.SerializeToNode(wheel),
                MavenLibrary maven => JsonSerializer.SerializeToNode(maven),
                PythonPyPiLibrary pypi => JsonSerializer.SerializeToNode(pypi),
                RCranLibrary rcran => JsonSerializer.SerializeToNode(rcran),
                _ => throw new NotImplementedException($"JsonConverter not implemented for type {value.GetType()}")
            };

            node!.WriteTo(writer);
        }
    }
}