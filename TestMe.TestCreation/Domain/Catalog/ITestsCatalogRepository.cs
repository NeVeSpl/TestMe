using System;
using System.Collections.Generic;
using System.Text;

namespace TestMe.TestCreation.Domain
{
    internal interface ITestsCatalogRepository
    {      
        TestsCatalog GetById(long id, bool includeTests = false);     
    }
}
