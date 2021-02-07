﻿// This file was autogenerated by http://frhagn.github.io/Typewriter/
${
    using Typewriter.Extensions.WebApi;
    using Typewriter.Extensions.Types;
    using System.Text;
   
    Template(Settings settings)
    {
        settings
            .IncludeProject("SharedKernel")
            .IncludeProject("TestCreation")           
            .IncludeProject("UserManagement")
            .IncludeProject("Presentation.API");
        settings.OutputFilenameFactory = file => 
        {       
            string prefix = file.Classes.FirstOrDefault()?.Namespace +"." ?? "";


            return $"dtos/{prefix}{file.Name.Replace(".cs", ".tsx")}";
        };
    }

    class ImportItem
    {
        public string Name { get; set; }
        public string From { get; set; }
        public bool ToImport { get; set;}

        public ImportItem(Type type)
        {
            Name = type.OriginalName;
            if (type.IsEnum)
            {
                From = $"../enums/{type.Namespace}.{Name}";
                ToImport = true;
                return;
            }
            if (Name.EndsWith("DTO"))
            {
                From = $"../dtos/{type.Namespace}.{Name}";
                ToImport = true;
                return;
            }
            From = $"../base/{Name}";
        }             
    }

    string GenImport(Class c)
    {
        var toImport = new List<ImportItem>();
        foreach(var p in c.Properties)
        {    
            if (((!p.Type.IsPrimitive) && (!p.Type.IsEnumerable)) || (p.Type.IsEnum))
            {
                toImport.Add(new ImportItem(p.Type));
            }           
            if (p.Type.IsGeneric)
            {
                foreach(var ta in p.Type.TypeArguments)
                {
                    if (!ta.IsPrimitive)
                    {
                        toImport.Add(new ImportItem(ta));
                    }
                }
            }
        }

        if (c.BaseClass != null)
        {
            toImport.Add(new ImportItem((Type)c.BaseClass));
        }

        List<ImportItem> distinct = toImport.GroupBy(x => x.Name).Select(g => g.First()).ToList();     
        return String.Join("\n", distinct.Select(x => $"import {{ {x.Name} }} from \"{x.From}\";"));
    }

    string GenPropertyType(Property p)
    {
        if (p.Type.IsNullable)
        {
           if (!p.Attributes.Any(x=> x.Name == "Required"))
           {
               return $"{p.Type} | null";
           }
        }

        return p.Type;
    }
    string GenPropertyDefault(Property p)
    {      
        if ((p.Type.IsNullable) && (!p.Attributes.Any(x=> x.Name == "Required")))
        {
            return "null";
        }
        if (p.Type.name == "string")
        {
            return "\"\"";
        }
        if (p.Type.name == "number")
        {
            return "0";
        }
        if ((!p.Type.IsPrimitive) && (!p.Type.IsEnum) && (!p.Type.IsEnumerable))
        {
            return $"{{}} as {p.Type.Name}";
        }
        if (p.Type.IsEnum)
        {
            return "0";
        }

        return p.Type.Default();
    }
   bool HasBaseClass(Class c)
   {
       return c.BaseClass != null;
   }
}
$Classes(*DTO)[$GenImport

export class $Name$TypeParameters$BaseClass[ extends $Name$TypeArguments]
{$Properties[
    $name: $GenPropertyType;]

    //eslint-disable-next-line
    constructor()
    {
    $HasBaseClass[    super();]$Properties[
        this.$name = $GenPropertyDefault;]
    }
}]