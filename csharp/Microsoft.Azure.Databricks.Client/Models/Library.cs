using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models
{
    public abstract record Library;

    public record JarLibrary : Library
    {
        /// <summary>
        /// URI of the jar to be installed. Only DBFS and S3 URIs are supported. For example: { "jar": "dbfs:/mnt/databricks/library.jar" } or { "jar": "s3://my-bucket/library.jar" }. If S3 is used, make sure the cluster has read access on the library. You may need to launch the cluster with an IAM role to access the S3 URI.
        /// </summary>
        [JsonPropertyName("jar")]
        public string Jar { get; set; }

        public override string ToString()
        {
            return "Jar://" + Jar;
        }
    }

    public record EggLibrary : Library
    {
        /// <summary>
        /// URI of the egg to be installed. Only DBFS and S3 URIs are supported. For example: { "egg": "dbfs:/my/egg" } or { "egg": "s3://my-bucket/egg" }. If S3 is used, make sure the cluster has read access on the library. You may need to launch the cluster with an IAM role to access the S3 URI.
        /// </summary>
        [JsonPropertyName("egg")]
        public string Egg { get; set; }

        public override string ToString()
        {
            return "Egg://" + Egg;
        }
    }

    public record WheelLibrary : Library
    {
        /// <summary>
        /// URI of the wheel or zipped wheels to be installed. Only DBFS URIs are supported. For example: { "whl": "dbfs:/my/whl" }.
        /// </summary>
        [JsonPropertyName("whl")]
        public string Wheel { get; set; }

        public override string ToString()
        {
            return "Whl://" + Wheel;
        }
    }

    public record PythonPyPiLibrary : Library
    {
        [JsonPropertyName("pypi")]
        public PythonPyPiLibrarySpec PythonPyPiLibrarySpec { get; set; }

        public override string ToString()
        {
            return "Python://" + PythonPyPiLibrarySpec.Repo + ":" + PythonPyPiLibrarySpec.Package;
        }
    }

    /// <summary>
    /// specification of a PyPi library to be installed. For example: { "package": "simplejson" }
    /// </summary>
    public record PythonPyPiLibrarySpec
    {
        /// <summary>
        /// The repository where the package can be found. If not specified, the default pip index is used.
        /// </summary>
        [JsonPropertyName("repo")]
        public string Repo { get; set; }

        /// <summary>
        /// The name of the pypi package to install. An optional exact version specification is also supported. Examples: “simplejson” and “simplejson==3.8.0”. This field is required.
        /// </summary>
        [JsonPropertyName("package")]
        public string Package { get; set; }
    }


    public record RCranLibrary : Library
    {
        [JsonPropertyName("cran")]
        public RCranLibrarySpec RCranLibrarySpec { get; set; }

        public override string ToString()
        {
            return "cran://" + RCranLibrarySpec.Repo + ":" + RCranLibrarySpec.Package;
        }
    }

    /// <summary>
    /// specification of a CRAN library to be installed as part of the library
    /// </summary>
    public record RCranLibrarySpec
    {
        /// <summary>
        /// The repository where the package can be found. If not specified, the default CRAN repo is used.
        /// </summary>
        [JsonPropertyName("repo")]
        public string Repo { get; set; }

        /// <summary>
        /// The name of the CRAN package to install. This field is required.
        /// </summary>
        [JsonPropertyName("package")]
        public string Package { get; set; }
    }


    public record MavenLibrary : Library
    {
        [JsonPropertyName("maven")]
        public MavenLibrarySpec MavenLibrarySpec { get; set; }

        public override string ToString()
        {
            return "maven://" + MavenLibrarySpec.Repo + ":" + MavenLibrarySpec.Coordinates;
        }
    }

    /// <summary>
    /// specification of a maven library to be installed. For example: { "coordinates": "org.jsoup:jsoup:1.7.2" }
    /// </summary>
    public record MavenLibrarySpec
    {
        /// <summary>
        /// Maven repo to install the Maven package from. If omitted, both Maven Central Repository and Spark Packages are searched.
        /// </summary>
        [JsonPropertyName("repo")]
        public string Repo { get; set; }

        /// <summary>
        /// Gradle-style maven coordinates. For example: “org.jsoup:jsoup:1.7.2”. This field is required.
        /// </summary>
        [JsonPropertyName("coordinates")]
        public string Coordinates { get; set; }

        /// <summary>
        /// List of dependences to exclude. For example: ["slf4j:slf4j", "*:hadoop-client"].
        /// </summary>
        /// <remarks>
        /// Maven dependency exclusions: https://maven.apache.org/guides/introduction/introduction-to-optional-and-excludes-dependencies.html.
        /// </remarks>
        [JsonPropertyName("exclusions")]
        public IEnumerable<string> Exclusions { get; set; }
    }
}