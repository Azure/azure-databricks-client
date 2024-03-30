using Microsoft.Azure.Databricks.Client.Models;
using System.Text.Json;

namespace Microsoft.Azure.Databricks.Client.Test.Converters
{
    [TestClass]
    public class InitScriptInfoConverterTest
    {
        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            WriteIndented = false
        };

        [TestMethod]
        public void TestDeserialization()
        {
            string json1 = @"{
                                ""workspace"": {
                                    ""destination"": ""string""
                                }
                            }";

            var initScriptInfo1 = JsonSerializer.Deserialize<InitScriptInfo>(json1);
            Assert.IsTrue(initScriptInfo1!.StorageDestination is WorkspaceStorageInfo);
            Assert.AreEqual("string", initScriptInfo1.StorageDestination!.Destination);

            string json2 = @"{
                                ""dbfs"": {
                                    ""destination"": ""string""
                                }
                            }";

            var initScriptInfo2 = JsonSerializer.Deserialize<InitScriptInfo>(json2);
            Assert.IsTrue(initScriptInfo2!.StorageDestination is DbfsStorageInfo);
            Assert.AreEqual("string", initScriptInfo2.StorageDestination!.Destination);

            string json3 = @"{
                                ""volumes"": {
                                    ""destination"": ""string""
                                }
                            }";

            var initScriptInfo3 = JsonSerializer.Deserialize<InitScriptInfo>(json3);
            Assert.IsTrue(initScriptInfo3!.StorageDestination is VolumesStorageInfo);
            Assert.AreEqual("string", initScriptInfo3.StorageDestination!.Destination);

            string json4 = @"{
                                ""abfss"": {
                                    ""destination"": ""string""
                                }
                            }";

            var initScriptInfo4 = JsonSerializer.Deserialize<InitScriptInfo>(json4);
            Assert.IsTrue(initScriptInfo4!.StorageDestination is AbfssStorageInfo);
            Assert.AreEqual("string", initScriptInfo4.StorageDestination!.Destination);

        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestInvalidProperty()
        {
            string json = @"{
                                ""invalid"": {
                                    ""destination"": ""string""
                                }
                            }";

            JsonSerializer.Deserialize<InitScriptInfo>(json);
        }

        [TestMethod]
        public void TestWrite()
        {
            var obj1 = new InitScriptInfo { StorageDestination = new WorkspaceStorageInfo { Destination = "string" } };
            var json1 = JsonSerializer.Serialize(obj1, Options);
            Assert.AreEqual(@"{""workspace"":{""destination"":""string""}}", json1);

            var obj2 = new InitScriptInfo { StorageDestination = new DbfsStorageInfo { Destination = "string" } };
            var json2 = JsonSerializer.Serialize(obj2, Options);
            Assert.AreEqual(@"{""dbfs"":{""destination"":""string""}}", json2);

            var obj3 = new InitScriptInfo { StorageDestination = new VolumesStorageInfo { Destination = "string" } };
            var json3 = JsonSerializer.Serialize(obj3, Options);
            Assert.AreEqual(@"{""volumes"":{""destination"":""string""}}", json3);

            var obj4 = new InitScriptInfo { StorageDestination = new AbfssStorageInfo() { Destination = "string" } };
            var json4 = JsonSerializer.Serialize(obj4, Options);
            Assert.AreEqual(@"{""abfss"":{""destination"":""string""}}", json4);
        }

        private record InvalidStorageDestination : StorageInfo { }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void TestWriteInvalid()
        {
            var obj = new InitScriptInfo { StorageDestination = new InvalidStorageDestination { Destination = "string" } };
            JsonSerializer.Serialize(obj, Options);
        }
    }
}
