using System.Collections.Generic;

namespace TestMe.BuildingBlocks.App
{
    public class OffsetPagedResults<T>
    {      
        public int PageNumber { get; set; }       
        public int PageSize { get; set; }
        public int TotalNumberOfPages { get; set; }      
        public int TotalNumberOfRecords { get; set; }
        public IEnumerable<T> Result { get; set; } = new List<T>();
    }
}
