﻿using System.ComponentModel.DataAnnotations;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.App.QuestionsCatalogs.Input
{
    public class CreateCatalog
    {
        public long UserId { get; set; }
        public string Name { get; set; } = string.Empty;
       
    }
}
