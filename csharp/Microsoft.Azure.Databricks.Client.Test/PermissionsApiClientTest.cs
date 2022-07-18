// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Azure.Databricks.Client.Models;
using Moq;
using Moq.Contrib.HttpClient;
using System.Net;
using System.Text.Json;

namespace Microsoft.Azure.Databricks.Client.Test;

[TestClass]
public class PermissionsApiClientTest : ApiClientTest
{
    private static readonly Uri PermissionsApiUri = new(BaseApiUri, "2.0/permissions/");

    [TestMethod]
    public async Task TestGetPermissionLevels()
    {
        const string clusterId = "test_cluster_id";
        var apiUri = new Uri(PermissionsApiUri, $"clusters/{clusterId}/permissionLevels");
        const string expectedResponse = @"
            {
              ""permission_levels"":
                [
                  {
                    ""permission_level"": ""CAN_MANAGE"",
                    ""description"": ""Can Manage permission on cluster""
                  },
                  {
                    ""permission_level"": ""CAN_RESTART"",
                    ""description"": ""Can Restart permission on cluster""
                  },
                  {
                    ""permission_level"": ""CAN_ATTACH_TO"",
                    ""description"": ""Can Attach To permission on cluster""
                  }
                ]
            }
        ";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json")
            .Verifiable();

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new PermissionsApiClient(hc);

        var permissionLevels = from level in await client.GetClusterPermissionLevels(clusterId)
                               select new { permission_level = level.Item1, description = level.Item2 };
        var actual = JsonSerializer.Serialize(new { permission_levels = permissionLevels }, Options);
        AssertJsonDeepEquals(expectedResponse, actual);
        handler.VerifyRequest(
            HttpMethod.Get,
            apiUri,
            Times.Once()
        );
    }

    [TestMethod]
    public async Task TestGetPermissions()
    {
        const string clusterId = "test_cluster_id";
        var apiUri = new Uri(PermissionsApiUri, $"clusters/{clusterId}");
        const string expectedResponse = @"
            {
              ""access_control_list"": [
                {
                  ""user_name"": ""jsmith@example.com"",
                  ""all_permissions"": [
                    {
                      ""permission_level"": ""CAN_RESTART"",
                      ""inherited"": true,
                      ""inherited_from_object"": [""/clusters/""]
                    }
                  ]
                },
                {
                  ""group_name"": ""admin_group"",
                  ""all_permissions"": [
                    {
                      ""permission_level"": ""CAN_MANAGE"",
                      ""inherited"": true,
                      ""inherited_from_object"": [""/clusters/""]
                    }
                  ]
                }
              ]
            }
        ";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json")
            .Verifiable();

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new PermissionsApiClient(hc);

        var aclItems = (await client.GetClusterPermissions(clusterId)).ToList();
        Assert.AreEqual(2, aclItems.Count);
        Assert.IsInstanceOfType(aclItems[0], typeof(UserAclItem));
        Assert.AreEqual("jsmith@example.com", aclItems[0].Principal);
        Assert.AreEqual(PermissionLevel.CAN_RESTART, aclItems[0].PermissionLevel);
        Assert.IsTrue(aclItems[0].Inherited);
        CollectionAssert.AreEquivalent(new[] { "/clusters/" }, aclItems[0].InheritedFromObject.ToArray());

        Assert.IsInstanceOfType(aclItems[1], typeof(GroupAclItem));
        Assert.AreEqual("admin_group", aclItems[1].Principal);
        Assert.AreEqual(PermissionLevel.CAN_MANAGE, aclItems[1].PermissionLevel);
        Assert.IsTrue(aclItems[0].Inherited);
        CollectionAssert.AreEquivalent(new[] { "/clusters/" }, aclItems[0].InheritedFromObject.ToArray());

        handler.VerifyRequest(
            HttpMethod.Get,
            apiUri,
            Times.Once()
        );
    }

    [TestMethod]
    public async Task TestUpdatePermissions()
    {
        const string clusterId = "test_cluster_id";
        var apiUri = new Uri(PermissionsApiUri, $"clusters/{clusterId}");
        const string expectedRequest = @"
            {
              ""access_control_list"": [
                {
                  ""user_name"": ""jsmith@example.com"",
                  ""permission_level"": ""CAN_RESTART""
                }
              ]
            }
        ";
        const string expectedResponse = @"
            {
              ""access_control_list"": [
                {
                  ""user_name"": ""jsmith@example.com"",
                  ""all_permissions"": [
                    {
                      ""permission_level"": ""CAN_RESTART"",
                      ""inherited"": true,
                      ""inherited_from_object"": [""/clusters/""]
                    }
                  ]
                },
                {
                  ""group_name"": ""admin_group"",
                  ""all_permissions"": [
                    {
                      ""permission_level"": ""CAN_MANAGE"",
                      ""inherited"": true,
                      ""inherited_from_object"": [""/clusters/""]
                    }
                  ]
                }
              ]
            }
        ";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Patch, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json")
            .Verifiable();

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new PermissionsApiClient(hc);

        var aclItemsReq = new List<AclPermissionItem>
            {new UserAclItem {Principal = "jsmith@example.com", PermissionLevel = PermissionLevel.CAN_RESTART}};

        var aclItems = (await client.UpdateClusterPermissions(aclItemsReq, clusterId)).ToList();
        Assert.AreEqual(2, aclItems.Count);
        Assert.IsInstanceOfType(aclItems[0], typeof(UserAclItem));
        Assert.AreEqual("jsmith@example.com", aclItems[0].Principal);
        Assert.AreEqual(PermissionLevel.CAN_RESTART, aclItems[0].PermissionLevel);
        Assert.IsTrue(aclItems[0].Inherited);
        CollectionAssert.AreEquivalent(new[] { "/clusters/" }, aclItems[0].InheritedFromObject.ToArray());

        Assert.IsInstanceOfType(aclItems[1], typeof(GroupAclItem));
        Assert.AreEqual("admin_group", aclItems[1].Principal);
        Assert.AreEqual(PermissionLevel.CAN_MANAGE, aclItems[1].PermissionLevel);
        Assert.IsTrue(aclItems[0].Inherited);
        CollectionAssert.AreEquivalent(new[] { "/clusters/" }, aclItems[0].InheritedFromObject.ToArray());

        handler.VerifyRequest(
            HttpMethod.Patch,
            apiUri,
            GetMatcher(expectedRequest),
            Times.Once()
        );
    }

    [TestMethod]
    public async Task TestReplacePermissions()
    {
        const string clusterId = "test_cluster_id";
        var apiUri = new Uri(PermissionsApiUri, $"clusters/{clusterId}");
        const string expectedRequest = @"
            {
              ""access_control_list"": [
                {
                  ""user_name"": ""jsmith@example.com"",
                  ""permission_level"": ""CAN_RESTART""
                }
              ]
            }
        ";
        const string expectedResponse = @"
            {
              ""access_control_list"": [
                {
                  ""user_name"": ""jsmith@example.com"",
                  ""all_permissions"": [
                    {
                      ""permission_level"": ""CAN_RESTART"",
                      ""inherited"": true,
                      ""inherited_from_object"": [""/clusters/""]
                    }
                  ]
                }
              ]
            }
        ";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Put, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json")
            .Verifiable();

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new PermissionsApiClient(hc);

        var aclItemsReq = new List<AclPermissionItem>
            {new UserAclItem {Principal = "jsmith@example.com", PermissionLevel = PermissionLevel.CAN_RESTART}};

        var aclItems = (await client.ReplaceClusterPermissions(aclItemsReq, clusterId)).ToList();
        Assert.AreEqual(1, aclItems.Count);
        Assert.IsInstanceOfType(aclItems[0], typeof(UserAclItem));
        Assert.AreEqual("jsmith@example.com", aclItems[0].Principal);
        Assert.AreEqual(PermissionLevel.CAN_RESTART, aclItems[0].PermissionLevel);
        Assert.IsTrue(aclItems[0].Inherited);
        CollectionAssert.AreEquivalent(new[] { "/clusters/" }, aclItems[0].InheritedFromObject.ToArray());

        handler.VerifyRequest(
            HttpMethod.Put,
            apiUri,
            GetMatcher(expectedRequest),
            Times.Once()
        );
    }
}