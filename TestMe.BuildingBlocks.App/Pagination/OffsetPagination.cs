using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TestMe.BuildingBlocks.App
{
    public class OffsetPagination
    {
        public int Offset { get; set; } = 0;
        [Range(0, 100)]
        public int Limit { get; set; } = 10;
    }
}
