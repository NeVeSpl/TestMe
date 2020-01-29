namespace TestMe.BuildingBlocks.App
{
    public interface IResult
    {
        ResultStatus Status { get; }

        bool HasValue();
        object? GetValue();        
        string GetError();
    }
}
