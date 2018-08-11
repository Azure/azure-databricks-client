using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
// ReSharper disable NonReadonlyMemberInGetHashCode

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

        public override bool Equals(object obj)
        {
            return obj is JarLibrary library &&
                   Jar == library.Jar;
        }

        public override int GetHashCode()
        {
            return 581150978 + EqualityComparer<string>.Default.GetHashCode(Jar);
        }

        public override string ToString()
        {
            return "Jar://" + Jar;
        }
    }

    public class EggLibrary : Library
    {
        /// <summary>
        /// URI of the egg to be installed. Only DBFS and S3 URIs are supported. For example: { "egg": "dbfs:/my/egg" } or { "egg": "s3://my-bucket/egg" }. If S3 is used, make sure the cluster has read access on the library. You may need to launch the cluster with an IAM role to access the S3 URI.
        /// </summary>
        [JsonProperty(PropertyName = "egg")]
        public string Egg { get; set; }

        public override bool Equals(object obj)
        {
            return obj is EggLibrary library &&
                   Egg == library.Egg;
        }

        public override int GetHashCode()
        {
            return -1932603306 + EqualityComparer<string>.Default.GetHashCode(Egg);
        }

        public override string ToString()
        {
            return "Egg://" + Egg;
        }
    }

    public class WheelLibrary : Library
    {
        /// <summary>
        /// URI of the wheel or zipped wheels to be installed. Only DBFS URIs are supported. For example: { "whl": "dbfs:/my/whl" }.
        /// </summary>
        [JsonProperty(PropertyName = "whl")]
        public string Wheel { get; set; }

        public override bool Equals(object obj)
        {
            return obj is WheelLibrary library &&
                   Wheel == library.Wheel;
        }

        public override int GetHashCode()
        {
            return -1948851972 + EqualityComparer<string>.Default.GetHashCode(Wheel);
        }

        public override string ToString()
        {
            return "Whl://" + Wheel;
        }
    }

    public class PythonPyPiLibrary : Library
    {
        [JsonProperty(PropertyName = "pypi")]
        public PythonPyPiLibrarySpec PythonPyPiLibrarySpec { get; set; }

        public override bool Equals(object obj)
        {
            return obj is PythonPyPiLibrary library &&
                   EqualityComparer<PythonPyPiLibrarySpec>.Default.Equals(PythonPyPiLibrarySpec,
                       library.PythonPyPiLibrarySpec);
        }

        public override int GetHashCode()
        {
            return -657381907 + EqualityComparer<PythonPyPiLibrarySpec>.Default.GetHashCode(PythonPyPiLibrarySpec);
        }

        public override string ToString()
        {
            return "Python://" + PythonPyPiLibrarySpec.Repo + ":" + PythonPyPiLibrarySpec.Package;
        }
    }

    /// <summary>
    /// specification of a PyPi library to be installed. For example: { "package": "simplejson" }
    /// </summary>
    public class PythonPyPiLibrarySpec
    {
        /// <summary>
        /// The repository where the package can be found. If not specified, the default pip index is used.
        /// </summary>
        [JsonProperty(PropertyName = "repo")]
        public string Repo { get; set; }

        /// <summary>
        /// The name of the pypi package to install. An optional exact version specification is also supported. Examples: “simplejson” and “simplejson==3.8.0”. This field is required.
        /// </summary>
        [JsonProperty(PropertyName = "package")]
        public string Package { get; set; }

        public override bool Equals(object obj)
        {
            return obj is PythonPyPiLibrarySpec spec &&
                   Repo == spec.Repo &&
                   Package == spec.Package;
        }

        public override int GetHashCode()
        {
            var hashCode = -587645984;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Repo);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Package);
            return hashCode;
        }
    }


    public class RCranLibrary : Library
    {
        [JsonProperty(PropertyName = "cran")]
        public RCranLibrarySpec RCranLibrarySpec { get; set; }

        public override bool Equals(object obj)
        {
            return obj is RCranLibrary library &&
                   EqualityComparer<RCranLibrarySpec>.Default.Equals(RCranLibrarySpec, library.RCranLibrarySpec);
        }

        public override int GetHashCode()
        {
            return 167115749 + EqualityComparer<RCranLibrarySpec>.Default.GetHashCode(RCranLibrarySpec);
        }

        public override string ToString()
        {
            return "cran://" + RCranLibrarySpec.Repo + ":" + RCranLibrarySpec.Package;
        }
    }

    /// <summary>
    /// specification of a CRAN library to be installed as part of the library
    /// </summary>
    public class RCranLibrarySpec
    {
        /// <summary>
        /// The repository where the package can be found. If not specified, the default CRAN repo is used.
        /// </summary>
        [JsonProperty(PropertyName = "repo")]
        public string Repo { get; set; }

        /// <summary>
        /// The name of the CRAN package to install. This field is required.
        /// </summary>
        [JsonProperty(PropertyName = "package")]
        public string Package { get; set; }

        public override bool Equals(object obj)
        {
            return obj is RCranLibrarySpec spec &&
                   Repo == spec.Repo &&
                   Package == spec.Package;
        }

        public override int GetHashCode()
        {
            var hashCode = -587645984;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Repo);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Package);
            return hashCode;
        }
    }


    public class MavenLibrary : Library
    {
        [JsonProperty(PropertyName = "maven")]
        public MavenLibrarySpec MavenLibrarySpec { get; set; }

        public override bool Equals(object obj)
        {
            return obj is MavenLibrary library &&
                   EqualityComparer<MavenLibrarySpec>.Default.Equals(MavenLibrarySpec, library.MavenLibrarySpec);
        }

        public override int GetHashCode()
        {
            return -2067436108 + EqualityComparer<MavenLibrarySpec>.Default.GetHashCode(MavenLibrarySpec);
        }
        
        public override string ToString()
        {
            return "maven://" + MavenLibrarySpec.Repo + ":" + MavenLibrarySpec.Coordinates;
        }
    }

    /// <summary>
    /// specification of a maven library to be installed. For example: { "coordinates": "org.jsoup:jsoup:1.7.2" }
    /// </summary>
    public class MavenLibrarySpec
    {
        /// <summary>
        /// Maven repo to install the Maven package from. If omitted, both Maven Central Repository and Spark Packages are searched.
        /// </summary>
        [JsonProperty(PropertyName = "repo")]
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
        [JsonProperty(PropertyName = "exclusions")]
        public IEnumerable<string> Exclusions { get; set; }

        public override bool Equals(object obj)
        {
            return obj is MavenLibrarySpec spec &&
                   Repo == spec.Repo &&
                   Coordinates == spec.Coordinates &&
                   Exclusions.SequenceEqual(spec.Exclusions);
        }

        public override int GetHashCode()
        {
            var hashCode = -2091977555;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Repo);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Coordinates);
            hashCode = hashCode * -1521134295 + EqualityComparer<IEnumerable<string>>.Default.GetHashCode(Exclusions);
            return hashCode;
        }
    }
}