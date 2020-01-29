namespace TestMe.UserManagement.App
{
    public interface ICorrelationIdProvider
    {
        string CorrelationId { get; }
    }
}
