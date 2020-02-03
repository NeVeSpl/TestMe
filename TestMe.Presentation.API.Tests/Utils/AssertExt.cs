using System.Net.Http;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace TestMe.Presentation.API.Tests.Utils
{
    internal static class AssertExt
    {
        public static void AreEquivalent(object expected, object actual)
        {
            actual.Should().BeEquivalentTo(expected, o => o.ExcludingMissingMembers());
        }

        public static void EnsureSuccessStatusCode(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                string parsedContent = string.IsNullOrEmpty(content) ? string.Empty : JToken.Parse(content).ToString();
                Assert.Fail($"Response status code does not indicate success: {(int)response.StatusCode}\r\n{parsedContent}");
            }
        }
    }
}
