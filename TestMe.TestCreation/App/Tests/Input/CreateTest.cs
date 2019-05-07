using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.App.Tests.Input
{
    public class CreateTest
    {
        [Required]
        public long? CatalogId { get; set; }

        [StringLength(maximumLength: Test.TitleMaxLength)]
        public string Title { get; set; }
    }
}
