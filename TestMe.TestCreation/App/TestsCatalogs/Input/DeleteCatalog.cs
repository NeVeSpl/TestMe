using System;
using System.Collections.Generic;
using System.Text;

namespace TestMe.TestCreation.App.TestsCatalogs.Input
{
    public class DeleteCatalog
    {
        public long UserId { get; set; }
        public long CatalogId { get; set; }


        public DeleteCatalog(long userId, long catalogId)
        {
            UserId = userId;
            CatalogId = catalogId;
        }
    }
}
