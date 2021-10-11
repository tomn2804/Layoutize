using System.Collections.Generic;
using System.Diagnostics;
using System.Management.Automation;

namespace SchemataPreview
{
    public class DefinitionticDirectoryModel : StrictDirectoryModel
    {
        public DefinitionticDirectoryModel(ImmutableDefinition Props)
            : base(props)
        {
            PipeAssembly[PipeOption.Mount].OnProcessed += (_, _) =>
            {
                Children.Add(
                    new Props<StrictTextModel>
                    {
                        { "Name", "Get-CurrentDirectoryDefinition.ps1" },
                        { "Contents", new string[] { "#Requires -Module SchemataPreview", "using namespace SchemataPreview" } }
                    }
                );
                Children.Add<ExcludeModel>("*.ps1");

                Model schematic = Children["Get-CurrentDirectoryDefinition.ps1"];
                Debug.Assert(schematic is StrictTextModel && schematic.Exists);

                using PowerShell instance = PowerShell.Create();
                List<Schema> schemata = new();
                foreach (PSObject @object in instance.AddScript(schematic).Invoke())
                {
                    if (@object.BaseObject is Props s)
                    {
                        schemata.Add(s);
                    }
                }
                Children.Add(Definitionta.ToArray());
            };
        }
    }
}
