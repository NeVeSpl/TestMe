﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TestMe.TestCreation.Domain
{
    internal interface IOwnerRepository
    {
        Owner GetById(long ownerId);

        Owner GetByIdWithTestsCatalogs(long ownerId);

        public void Add(Owner owner);
    }
}
