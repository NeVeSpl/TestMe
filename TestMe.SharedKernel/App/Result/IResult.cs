namespace TestMe.SharedKernel.App
{
    public interface IResult
    {
        ResultStatus Status { get; }

        bool HasValue();
        object GetValue();        
        string GetError();
    }
}
