using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.App.Tests.Input
{
    public class CreateTest
    {      
        public long UserId { get; set; }
        public long CatalogId { get; set; }
        public string Title { get; set; } = string.Empty;
    }
}
