﻿using System;
using System.ComponentModel.DataAnnotations;

namespace TestMe.BuildingBlocks.App
{
    public class CursorPagination
    {
        public int Cursor { get; set; } = 0;
        [Range(-100, 100)]
        public int FetchNext { get; set; } = 10;
    }
}
