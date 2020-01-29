using System.Collections.Generic;

namespace TestMe.BuildingBlocks.App
{
    public class CursorPagedResults<T>
    {
        public long? Cursor { get; set; }
        public long? NextCursor { get; set; }
        public IEnumerable<T> Result { get; set; } = new List<T>();
    }
}
