using System;
using System.Collections.Generic;
using System.Text;

namespace TestMe.TestCreation.Domain
{
    internal abstract class Catalog
    {
        public const int NameMaxLength = 2048;
        public const int NameMinLength = 1;


        public long CatalogId { get; private set; }
        // todo : add value object to enforce invariants on Name length
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
