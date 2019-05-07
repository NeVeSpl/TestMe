using System.ComponentModel.DataAnnotations;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.App.Catalogs.Input
{
    public class CreateCatalog
    {
        [Required]
        [StringLength(maximumLength: Catalog.NameMaxLength, MinimumLength = Catalog.NameMinLength)]
        public string Name { get; set; }
       
    }
}
