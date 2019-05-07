using FluentAssertions;

namespace TestMe.Presentation.API.Tests.Utils
{
    public static class AssertExt
    {
        public static void AreEquivalent(object expected, object actual)
        {
            actual.Should().BeEquivalentTo(expected, o => o.ExcludingMissingMembers());
        }
    }
}
