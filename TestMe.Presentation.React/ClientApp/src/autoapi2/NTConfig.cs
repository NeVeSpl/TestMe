using System;
using System.Collections.Generic;
using System.Text;
using NTypewriter.CodeModel;
using NTypewriter.CodeModel.Functions;
using NTypewriter.Editor.Config;

namespace TestMe.Presentation.React.ClientApp.src.autoapi2
{
    [NTEditorFile]
    public class NTConfig : EditorConfig
    {
        public override IEnumerable<string> GetNamespacesToBeSearched()
        {
            yield return "TestMe.SharedKernel";
            yield return "TestMe.TestCreation";
            yield return "TestMe.UserManagement";
            yield return "TestMe.Presentation.API";
        }
        public override IEnumerable<Type> GetTypesThatContainCustomFunctions()
        {
            yield return typeof(NTConfig);
        }


        public static string GenImport(IClass @class)
        {
            return GenerateListOfDependencies(@class, "import").Trim();
        }
        public static string GenExport(IClass @class)
        {
            return GenerateListOfDependencies(@class, "export").Trim();
        }
        private static string GenerateListOfDependencies(IClass @class, string operation)
        {
            var builder = new StringBuilder();
            var types = @class.AllReferencedTypes();

            foreach (var type in types)
            {
                string name = type.BareName;
                string from = "";
                if (type.IsEnum)
                {
                    from = "enums";
                }
                if (name.EndsWith("DTO"))
                {
                    from = "dtos";
                }
                if (!String.IsNullOrEmpty(from))
                {
                    if (operation == "import")
                    {
                        builder.AppendLine($"import {{ {name} }} from \"../{from}/{type.Namespace}.{name}\";");
                    }
                    if (operation == "export")
                    {
                        builder.AppendLine($"export * from \"../{from}/{type.Namespace}.{name}\";");
                    }
                }
            }

            return builder.ToString();
        }
    }
}