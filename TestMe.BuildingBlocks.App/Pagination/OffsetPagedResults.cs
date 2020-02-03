using System.Collections.Generic;
using System.Linq;

namespace TestMe.BuildingBlocks.App
{
    public class OffsetPagedResults<T>
    {      
        public bool IsThereMore { get; set; }
        public IEnumerable<T> Result { get; set; } = new List<T>();

        public OffsetPagedResults(IEnumerable<T> items, int limit)
        {
            IsThereMore = items.Count() > limit;
            Result = items.Take(limit);
        }
        public OffsetPagedResults()
        {

        }
    }
}