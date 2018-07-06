using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.Azure.Databricks.Client
{
    public abstract class Library
    {
    }

    public class JarLibrary : Library
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JarLibrary"/> class.
        /// </summary>
        public JarLibrary()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JarLibrary"/> class.
        /// </summary>
        /// <param name="jar">The jar.</param>
        public JarLibrary(string jar)
        {
            Jar = jar;
        }

        /// <summary>
        /// URI of the jar to be installed. Only DBFS and S3 URIs are supported. For example: { "jar": "dbfs:/mnt/databricks/library.jar" } or { "jar": "s3://my-bucket/library.jar" }. If S3 is used, make sure the cluster has read access on the library. You may need to launch the cluster with an IAM role to access the S3 URI.
        /// </summary>
        [JsonProperty(PropertyName = "jar")]
        public string Jar { get; set; }
    }

    public class EggLibrary : Library
    {
        /// <summary>
        /// URI of the egg to be installed. Only DBFS and S3 URIs are supported. For example: { "egg": "dbfs:/my/egg" } or { "egg": "s3://my-bucket/egg" }. If S3 is used, make sure the cluster has read access on the library. You may need to launch the cluster with an IAM role to access the S3 URI.
        /// </summary>
        [JsonProperty(PropertyName = "egg")]
        public string Egg { get; set; }
    }

    public class PythonPyPiLibrary : Library
    {
        [JsonProperty(PropertyName = "pypi")]
        public PythonPyPiLibrarySpec PythonPyPiLibrarySpec { get; set; }
    }

    /// <summary>
    /// specification of a PyPi library to be installed. For example: { "package": "simplejson" }
    /// </summary>
    public class PythonPyPiLibrarySpec
    {
        /// <summary>
        /// The repository where the package can be found. If not specified, the default pip index is used.
        /// </summary>
        [JsonProperty(PropertyName = "repo", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Repo { get; set; }

        /// <summary>
        /// The name of the pypi package to install. An optional exact version specification is also supported. Examples: “simplejson” and “simplejson==3.8.0”. This field is required.
        /// </summary>
        [JsonProperty(PropertyName = "package")]
        public string Package { get; set; }
    }


    public class RCranLibrary : Library
    {
        [JsonProperty(PropertyName = "cran")]
        public RCranLibrarySpec RCranLibrarySpec { get; set; }
    }

    /// <summary>
    /// specification of a CRAN library to be installed as part of the library
    /// </summary>
    public class RCranLibrarySpec
    {
        /// <summary>
        /// The repository where the package can be found. If not specified, the default CRAN repo is used.
        /// </summary>
        [JsonProperty(PropertyName = "repo", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Repo { get; set; }

        /// <summary>
        /// The name of the CRAN package to install. This field is required.
        /// </summary>
        [JsonProperty(PropertyName = "package")]
        public string Package { get; set; }
    }


    public class MavenLibrary : Library
    {
        [JsonProperty(PropertyName = "maven")]
        public MavenLibrarySpec MavenLibrarySpec { get; set; }
    }

    /// <summary>
    /// specification of a maven library to be installed. For example: { "coordinates": "org.jsoup:jsoup:1.7.2" }
    /// </summary>
    public class MavenLibrarySpec
    {
        /// <summary>
        /// Maven repo to install the Maven package from. If omitted, both Maven Central Repository and Spark Packages are searched.
        /// </summary>
        [JsonProperty(PropertyName = "repo", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Repo { get; set; }

        /// <summary>
        /// Gradle-style maven coordinates. For example: “org.jsoup:jsoup:1.7.2”. This field is required.
        /// </summary>
        [JsonProperty(PropertyName = "coordinates")]
        public string Coordinates { get; set; }

        /// <summary>
        /// List of dependences to exclude. For example: ["slf4j:slf4j", "*:hadoop-client"].
        /// </summary>
        /// <remarks>
        /// Maven dependency exclusions: https://maven.apache.org/guides/introduction/introduction-to-optional-and-excludes-dependencies.html.
        /// </remarks>
        [JsonProperty(PropertyName = "exclusions", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<string> Exclusions { get; set; }
    }

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