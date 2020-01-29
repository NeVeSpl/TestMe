using System;
using System.Collections.Generic;
using System.Text;

namespace TestMe.TestCreation.Domain
{
    internal abstract class Catalog
    {
        public long CatalogId { get; private set; }
        
        public string Name { get;  set; }
        public long OwnerId { get; private set; }
        public bool IsDeleted { get; private set; }


        protected Catalog(string name, long ownerId)
        {
            Name = name;
            OwnerId = ownerId;
        }


        public virtual void Delete()
        {
            IsDeleted = true;
        }        
    }
}
