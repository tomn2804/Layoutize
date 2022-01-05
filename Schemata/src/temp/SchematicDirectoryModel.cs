using System.Collections.Generic;
using System.Diagnostics;
using System.Management.Automation;

namespace Schemata
{
    public class DefinitionticDirectoryModel : StrictDirectoryModel
    {
        public DefinitionticDirectoryModel(ImmutableDefinition Outline)
            : base(outline)
        {
            PipeAssembly[PipeOption.Mount].OnProcessed += (_, _) =>
            {
                Children.Add(
                    new Outline<StrictTextModel>
                    {
                        { "Name", "Get-CurrentDirectoryDefinition.ps1" },
                        { "Contents", new string[] { "#Requires -Module Schemata", "using namespace Schemata" } }
                    }
                );
                Children.Add<ExcludeModel>("*.ps1");

                Model schematic = Children["Get-CurrentDirectoryDefinition.ps1"];
                Debug.Assert(schematic is StrictTextModel && schematic.Exists);

                using PowerShell instance = PowerShell.Create();
                List<Schema> schemata = new();
                foreach (PSObject _object in instance.AddScript(schematic).Invoke())
                {
                    if (_object.BaseObject is Outline s)
                    {
                        schemata.Add(s);
                    }
                }
                Children.Add(Definitionta.ToArray());
            };
        }
    }
}
