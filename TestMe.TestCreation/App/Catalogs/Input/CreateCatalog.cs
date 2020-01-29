using System.ComponentModel.DataAnnotations;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.App.Catalogs.Input
{
    public class CreateCatalog
    {
        public long UserId { get; set; }
        public string Name { get; set; } = string.Empty;
       
    }
}
