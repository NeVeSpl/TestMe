namespace TestMe.BuildingBlocks.Tests
{
    public enum FakeDatabaseType 
    {
        ///<summary>Tests can run concurrently, new instance is created every time</summary>
        EFInMemory,
        ///<summary>Tests can run concurrently, new instance is created every time</summary>
        SQLiteInMemory,
        ///<summary>Tests cannot run concurrently, </summary>
        PostgreSQL
    }
}
