namespace TestMe.BuildingBlocks.App
{
    public interface IHaveConcurrencyToken
    {
        uint ConcurrencyToken { get; }
    }
}
