﻿// This file was autogenerated by http://frhagn.github.io/Typewriter/
${
    using Typewriter.Extensions.WebApi;
    using System.Text;

    string GenServiceName(Class c) => c.Name.Replace("Controller", "Service");
  
    Template(Settings settings)
    {
        settings.IncludeProject("Presentation.API");
        settings.OutputFilenameFactory = file => 
        {       
            return $"services/{file.Name.Replace("Controller.cs", "Service.tsx")}";
        };
    }

    public class ImportItem
    {
        public string Name { get; set; }
        public string From { get; set; }

        public ImportItem(Type type)
        {
            Name = type.OriginalName;
            if (type.IsEnum)
            {
                From = $"../enums/{type.Namespace}.{Name}";
                return;
            }
            if (Name.EndsWith("DTO"))
            {
                From = $"../dtos/{type.Namespace}.{Name}";
                return;
            }
            From = $"../base/{Name}";
        }            
    }
    
    string GenImport(Class c)
    {
        var toImport = new List<ImportItem>();
        foreach(var m in c.Methods)
        {           
           TraverseRecursively(toImport, m.Type);
           
           foreach(var p in m.Parameters)
           {
              if ((!p.Type.IsPrimitive) && (ParametersFilter(p)))
              {
                  toImport.Add(new ImportItem(p.Type));
              }
           }
        }

        List<ImportItem> distinct = toImport.GroupBy(x => x.Name).Select(g => g.First()).ToList();     
        return String.Join("\n", distinct.Select(x => $"import {{ {x.Name} }} from \"{x.From}\";"))  + "\n" +
        String.Join("\n", distinct.Select(x => $"export * from \"{x.From}\";"));        
    }

    void TraverseRecursively(List<ImportItem> toImport, Type type)
    {
        if (type.OriginalName != "Task" && type.OriginalName != "ActionResult" && type.OriginalName != "ValueType")
        {
            if ((!type.IsPrimitive) && (!type.IsEnumerable))
            {
                toImport.Add(new ImportItem(type));
            }
        }
        if (type.IsGeneric)
        {
            foreach(var ta in type.TypeArguments)
            {
                TraverseRecursively(toImport, ta);
            }
        }
        foreach(var p in type.Properties)
        {
            TraverseRecursively(toImport, p.Type);
        }
        if ((type.BaseClass != null) && (!type.IsPrimitive))
        {
            TraverseRecursively(toImport, (Type)type.BaseClass);
        }
    }

    string GenMethodReturn(Method m)
    {
         if (m.Type.IsGeneric)
         {
             return $"<{m.Type.TypeArguments.FirstOrDefault()}>";
         }
         return String.Empty;
    }
    string GenPromiseMe(Method m)
    {
        var methodReturn = GenMethodReturn(m);
        if (String.IsNullOrEmpty(methodReturn))
        {
           return methodReturn;
        }
        return $": Promise{methodReturn}";
    }
    bool ParametersFilter(Parameter p)
    {
        if (p.Attributes.Any(x => x.Name == "FromServices" || x.Name == "FromBody"))
        {
            return false;
        }
        return true;
    }
    string GenPayloadParameterName(Method m)
    {
        string result = "null";
        foreach(var p in m.Parameters)
        {
           if (p.Type.IsPrimitive)
           {
               continue;
           }
           if (p.Attributes.Any(x => x.Name == "FromServices" || x.Name == "FromQuery"))
           {
              continue;
           }
           result = p.name;
           break;
        }
        return result;
    }
    string GenFromRoute(Method m)
    {
        string url = UrlExtensions.Url(m);
        string connector = url.Contains("?") ? "&" : "?";
        var builder = new StringBuilder();
        foreach(Parameter p in m.Parameters)
        {
           if ((!p.Type.IsPrimitive) && p.Attributes.Any(x => x.Name == "FromQuery"))
           {
              foreach(var prop in p.Type.Properties)
              {              
                  builder.Append(connector);
                  builder.Append($"{prop.name}=${{{p.name}.{prop.name}}}");
                  connector = "&";
              }
           }
        }
        return builder.ToString();
    }
}
$Classes(TestMe.Presentation.API.Controllers.*Controller)[
import { ApiBaseService } from "../base/index";
$GenImport
export * from "../base/index";

export class $GenServiceName extends ApiBaseService
{
    $Methods[$name($Parameters($ParametersFilter)[$name: $Type][, ]) $GenPromiseMe
    {
        return this.MakeRequest$GenMethodReturn("$HttpMethod", `$Url$GenFromRoute`, $GenPayloadParameterName);
    }
    ]       
}]