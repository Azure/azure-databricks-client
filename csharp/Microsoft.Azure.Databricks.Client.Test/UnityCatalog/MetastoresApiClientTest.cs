using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using Microsoft.Azure.Databricks.Client.UnityCatalog;
using Moq;
using Moq.Contrib.HttpClient;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Microsoft.Azure.Databricks.Client.Test.UnityCatalog;

[TestClass]
public class MetastoresApiClientTest : UnityCatalogApiClientTest
{
    [TestMethod]
    public async Task TestList()
    {
        var requestUri = $"{BaseApiUri}metastores";

        var expectedResponse = @"
        {
            ""metastores"": [
            {
                ""name"": ""string"",
                ""storage_root"": ""string"",
                ""default_data_access_config_id"": ""string"",
                ""storage_root_credential_id"": ""string"",
                ""delta_sharing_scope"": ""INTERNAL"",
                ""delta_sharing_recipient_token_lifetime_in_seconds"": 1,
                ""delta_sharing_organization_name"": ""string"",
                ""owner"": ""string"",
                ""privilege_model_version"": ""string"",
                ""region"": ""string"",
                ""metastore_id"": ""string"",
                ""created_at"": 0,
                ""created_by"": ""string"",
                ""updated_at"": 0,
                ""updated_by"": ""string"",
                ""storage_root_credential_name"": ""string"",
                ""cloud"": ""string"",
                ""global_metastore_id"": ""string""
            }
            ]
        }
        ";

        var expected = JsonNode.Parse(expectedResponse)?["metastores"].Deserialize<IEnumerable<Metastore>>(Options);

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new MetastoresApiClient(mockClient);
        var response = await client.List();

        var responseJson = JsonSerializer.Serialize(response, Options);
        CollectionAssert.AreEqual(expected?.ToList(), response?.ToList());
    }

    [TestMethod]
    public async Task TestGetSummary()
    {
        var requestUri = $"{BaseApiUri}metastore_summary";

        var expectedResponse = @"
        {
          ""metastore_id"": ""string"",
          ""name"": ""string"",
          ""default_data_access_config_id"": ""string"",
          ""storage_root_credential_id"": ""string"",
          ""cloud"": ""string"",
          ""region"": ""string"",
          ""global_metastore_id"": ""string"",
          ""storage_root_credential_name"": ""string"",
          ""privilege_model_version"": ""string"",
          ""delta_sharing_scope"": ""INTERNAL"",
          ""delta_sharing_recipient_token_lifetime_in_seconds"": 1,
          ""delta_sharing_organization_name"": ""string"",
          ""storage_root"": ""string"",
          ""owner"": ""string"",
          ""created_at"": 0,
          ""created_by"": ""string"",
          ""updated_at"": 0,
          ""updated_by"": ""string""
        }
        ";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new MetastoresApiClient(mockClient);
        var response = await client.GetSummary();

        var responseJson = JsonSerializer.Serialize(response, Options);
        AssertJsonDeepEquals(expectedResponse, responseJson);
    }

    [TestMethod]
    public async Task TestGet()
    {
        var metastoreId = "2422B3C8-664B-4EDC-9E65-F164F2B2F2BA";
        var requestUri = $"{BaseApiUri}metastores/{metastoreId}";

        var expectedResponse = @"
        {
          ""metastore_id"": ""string"",
          ""name"": ""string"",
          ""default_data_access_config_id"": ""string"",
          ""storage_root_credential_id"": ""string"",
          ""cloud"": ""string"",
          ""region"": ""string"",
          ""global_metastore_id"": ""string"",
          ""storage_root_credential_name"": ""string"",
          ""privilege_model_version"": ""string"",
          ""delta_sharing_scope"": ""INTERNAL"",
          ""delta_sharing_recipient_token_lifetime_in_seconds"": 1,
          ""delta_sharing_organization_name"": ""string"",
          ""storage_root"": ""string"",
          ""owner"": ""string"",
          ""created_at"": 0,
          ""created_by"": ""string"",
          ""updated_at"": 0,
          ""updated_by"": ""string""
        }
        ";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new MetastoresApiClient(mockClient);
        var response = await client.Get(metastoreId);

        var responseJson = JsonSerializer.Serialize(response, Options);
        AssertJsonDeepEquals(expectedResponse, responseJson);
    }

    [TestMethod]
    public async Task TestCreate()
    {
        var requestUri = $"{BaseApiUri}metastores";

        var expectedRequest = @"
        {
            ""name"": ""string"",
            ""storage_root"": ""string"",
            ""region"": ""string""
        }
        ";

        var expectedResponse = @"
        {
          ""name"": ""string"",
          ""storage_root"": ""string"",
          ""default_data_access_config_id"": ""string"",
          ""storage_root_credential_id"": ""string"",
          ""delta_sharing_scope"": ""INTERNAL"",
          ""delta_sharing_recipient_token_lifetime_in_seconds"": 1,
          ""delta_sharing_organization_name"": ""string"",
          ""owner"": ""string"",
          ""privilege_model_version"": ""string"",
          ""region"": ""string"",
          ""metastore_id"": ""string"",
          ""created_at"": 0,
          ""created_by"": ""string"",
          ""updated_at"": 0,
          ""updated_by"": ""string"",
          ""storage_root_credential_name"": ""string"",
          ""cloud"": ""string"",
          ""global_metastore_id"": ""string""
        }
        ";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new MetastoresApiClient(mockClient);
        var response = await client.Create(
            "string",
            "string",
            "string");

        var responseJson = JsonSerializer.Serialize(response, Options);
        AssertJsonDeepEquals(expectedResponse, responseJson);

        handler.VerifyRequest(
            HttpMethod.Post,
            requestUri,
            GetMatcher(expectedRequest),
            Times.Once());
    }

    [TestMethod]
    public async Task TestUpdate()
    {
        var metastoreId = "2422B3C8-664B-4EDC-9E65-F164F2B2F2BA";
        var requestUri = $"{BaseApiUri}metastores/{metastoreId}";

        var expectedRequest = @"
        {
          ""name"": ""string"",
          ""storage_root_credential_id"": ""string"",
          ""delta_sharing_scope"": ""INTERNAL"",
          ""delta_sharing_recipient_token_lifetime_in_seconds"": 1,
          ""delta_sharing_organization_name"": ""string"",
          ""owner"": ""string"",
          ""privilege_model_version"": ""string""
        }
";

        var expectedResponse = @"
        {
Change settings

1 chat with new messages
Chat



has context menu
has context menuYou were mentioned.SMS chat.Unread message.Last message:Group chat.Meeting Chat.Chat.You're starting a new conversation..This chat is muted.You can't send messages because you are not a member of the chat.
Chat with anyone


TT

Chat

Files

Has context menu

Meet now


17

Luke’s been practicing….. Image by Sacha Mahon
Sacha Mahon
28/06 2:17 pm


Luke’s been practicing…..
Image

😆
3 Laugh reactions.
3

❤️
1 Heart reaction.
1
  nbc lonely at the top GIF by Good Girls (... by Luke Oswald
Luke Oswald
28/06 2:47 pm


 


 


😆
1 Laugh reaction.
1
Image by Sacha Mahon
Sacha Mahon
28/06 2:48 pm


Image
Begin Reference, 📷, Luke Oswald, 28/06/202... by Matthew Alexander
Matthew Alexander
28/06 2:48 pm


Luke Oswald
28/06/2024 2:47 pm
📷
If past performance is an indicator that is a problem that will not trouble you for long 

😆
2 Laugh reactions.
2

😮
1 Surprised reaction.
1
Begin Reference, 3 o'clock?, Toby Cook, 28/... by Luke Oswald
Luke Oswald
28/06 2:48 pm


Toby Cook
28/06/2024 2:16 pm
3 o'clock?
3.15
Begin Reference, 3.15, Luke Oswald, 28/06/2... by Sacha Mahon
Sacha Mahon
28/06 2:49 pm


Luke Oswald
28/06/2024 2:48 pm
3.15
3:00. Apparently you'll be able to come in 15 minutes late and still win.

😆
2 Laugh reactions.
2

❤️
1 Heart reaction.
1
Begin Reference, If past performance is an ... by Luke Oswald
Luke Oswald
28/06 3:16 pm


Matthew Alexander
28/06/2024 2:48 pm
If past performance is an indicator that is a problem that will not trouble you for long
you're a bit chirpy for someone who's on page 2 of the leaderboard 😄
Begin Reference, 3:00. Apparently you'll be... by Luke Oswald
Luke Oswald
28/06 3:17 pm

Sacha Mahon
28/06/2024 2:49 pm
3:00. Apparently you'll be able to come in 15 minutes late and still win.



  episode 2 sptv GIF by Sony Pictures Telev... by Luke Oswald
Luke Oswald
28/06 3:20 pm

 


 


❤️
1 Heart reaction.
1
Friday
3.00pm sharp by Toby Cook
Toby Cook
Friday 2:30 pm


3.00pm sharp


❤️
2 Heart reactions.
2

🩵
1 Light blue heart reaction.
1
sharpish? by Sacha Mahon
Sacha Mahon
Friday 3:00 pm


sharpish?

  The Ultimate Table Tennis Serve?! - JOOLA... by David Jones
David Jones
Friday 3:04 pm


 

The Ultimate Table Tennis Serve?! - JOOLA USA

 


😆
2 Laugh reactions.
2
  image   by John Koufalas
John Koufalas
Friday 3:17 pm


 

image

 


🤮
1 Vomiting reaction.
1
Image by Sacha Mahon
Sacha Mahon
Friday 3:18 pm


Image
neil lost enough ranking points today to re... by Toby Cook
Toby Cook
Friday 3:23 pm


neil lost enough ranking points today to reinstate luke at #1. not sure how i feel about this

image

 


❤️
1 Heart reaction.
1
the power of reputation by Luke Oswald
Luke Oswald
Friday 3:23 pm


the power of reputation


😆
1 Laugh reaction.
1

❤️
1 Heart reaction.
1
It's like Donald Trump! by Sacha Mahon
Sacha Mahon
Friday 3:23 pm


It's like Donald Trump!

  Rick And Morty Dont Hate The Player GIF (... by Luke Oswald
Luke Oswald
Friday 3:24 pm


 


 

Begin Reference, 📷 Image, Luke Oswald , 05... by Sacha Mahon
Sacha Mahon
Friday 3:24 pm


Luke Oswald
05/07/2024 3:24 pm
📷 Image
Image
Yesterday
  Ping Pong Sport GIF by War Child (GIF Ima... by Luke Oswald
Luke Oswald
Yesterday 3:13 pm


 


 


👍
2 Like reactions.
2

❤️
1 Heart reaction.
1
I’ll pass….. Image by Sacha Mahon
Sacha Mahon
Yesterday 3:26 pm


I’ll pass…..
Image

🩵
2 Light blue heart reactions.
2
you'll pass out by John Koufalas
John Koufalas
Yesterday 3:28 pm


you'll pass out


😆
4 Laugh reactions.
4
It’s like you know me! by Sacha Mahon
Sacha Mahon
Yesterday 3:29 pm


It’s like you know me!
scores please, we need to bump luke off bef... by John Koufalas
John Koufalas
Yesterday 4:43 pm


scores please, we need to bump luke off before fridays, all hands


😆
3 Laugh reactions.
3
Begin Reference, scores please, we need to ... by Sacha Mahon
Sacha Mahon
Yesterday 4:46 pm


John Koufalas
11/07/2024 4:43 pm
scores please, we need to bump luke off before fridays, all hands
Image
Last read
          ""name"": ""string"",
          ""storage_root"": ""string"",
          ""default_data_access_config_id"": ""string"",
          ""storage_root_credential_id"": ""string"",
          ""delta_sharing_scope"": ""INTERNAL"",
          ""delta_sharing_recipient_token_lifetime_in_seconds"": 1,
          ""delta_sharing_organization_name"": ""string"",
          ""owner"": ""string"",
          ""privilege_model_version"": ""string"",
          ""region"": ""string"",
          ""metastore_id"": ""string"",
          ""created_at"": 0,
          ""created_by"": ""string"",
          ""updated_at"": 0,
          ""updated_by"": ""string"",
          ""storage_root_credential_name"": ""string"",
          ""cloud"": ""string"",
          ""global_metastore_id"": ""string""
        }
";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Patch, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new MetastoresApiClient(mockClient);
        var response = await client.Update(
            metastoreId,
            "string",
            "string",
            DeltaSharingScope.INTERNAL,
            1,
            "string",
            "string",
            "string");

        var responseJson = JsonSerializer.Serialize(response, Options);
        AssertJsonDeepEquals(expectedResponse, responseJson);

        handler.VerifyRequest(
            HttpMethod.Patch,
            requestUri,
            GetMatcher(expectedRequest),
            Times.Once());
    }

    [TestMethod]
    public async Task TestDelete()
    {
        var metastoreId = "2422B3C8-664B-4EDC-9E65-F164F2B2F2BA";
        var requestUri = $"{BaseApiUri}metastores/{metastoreId}?force=false";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Delete, requestUri)
            .ReturnsResponse(HttpStatusCode.OK);

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new MetastoresApiClient(mockClient);
        await client.Delete(metastoreId);

        handler.VerifyRequest(
            HttpMethod.Delete,
            requestUri);
    }

    [TestMethod]
    public async Task TestGetAssignment()
    {
        var requestUri = $"{BaseApiUri}current-metastore-assignment";

        var expectedResponse = @"
        {
          ""metastore_id"": ""string"",
          ""workspace_id"": 1,
          ""default_catalog_name"": ""string""
        }
        ";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new MetastoresApiClient(mockClient);
        var response = await client.GetAssignment();

        var responseJson = JsonSerializer.Serialize(response, Options);
        AssertJsonDeepEquals(expectedResponse, responseJson);
    }

    [TestMethod]
    public async Task TestCreateAssignment()
    {
        var workspaceId = 1232412;
        var requestUri = $"{BaseApiUri}workspaces/{workspaceId}/metastore";

        var expectedRequest = @"
        {
          ""metastore_id"": ""string"",
          ""default_catalog_name"": ""string""
        }
        ";


        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Put, requestUri)
            .ReturnsResponse(HttpStatusCode.OK);

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new MetastoresApiClient(mockClient);
        await client.CreateAssignment(
            workspaceId,
            "string",
            "string");

        handler.VerifyRequest(
            HttpMethod.Put,
            requestUri,
            GetMatcher(expectedRequest),
            Times.Once());
    }

    [TestMethod]
    public async Task TestUpdateAssignment()
    {
        var workspaceId = 1232412;
        var requestUri = $"{BaseApiUri}workspaces/{workspaceId}/metastore";

        var expectedRequest = @"
        {
          ""metastore_id"": ""string"",
          ""default_catalog_name"": ""string""
        }
        ";


        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Patch, requestUri)
            .ReturnsResponse(HttpStatusCode.OK);

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new MetastoresApiClient(mockClient);
        await client.UpdateAssignment(
            workspaceId,
            "string",
            "string");

        handler.VerifyRequest(
            HttpMethod.Patch,
            requestUri,
            GetMatcher(expectedRequest),
            Times.Once());
    }

    [TestMethod]
    public async Task TestDeleteAssignment()
    {
        var metastoreId = "2422B3C8-664B-4EDC-9E65-F164F2B2F2BA";
        var workspaceId = 1232412;
        var requestUri = $"{BaseApiUri}workspaces/{workspaceId}/metastore?metastore_id={metastoreId}";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Delete, requestUri)
            .ReturnsResponse(HttpStatusCode.OK);

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new MetastoresApiClient(mockClient);
        await client.DeleteAssignment(
            workspaceId,
            metastoreId);

        handler.VerifyRequest(
            HttpMethod.Delete,
            requestUri);
    }
}
