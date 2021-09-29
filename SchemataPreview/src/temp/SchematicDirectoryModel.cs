using System.Collections.Generic;
using System.Diagnostics;
using System.Management.Automation;

namespace SchemataPreview
{
	public class SchematicDirectoryModel : StrictDirectoryModel
	{
		public SchematicDirectoryModel(ImmutableSchema schema)
			: base(schema)
		{
			PipeAssembly[PipeOption.Mount].OnProcessed += (_, _) =>
			{
				Children.Add(
					new Schema<StrictTextModel>
					{
						{ "Name", "Get-CurrentDirectorySchema.ps1" },
						{ "Contents", new string[] { "#Requires -Module SchemataPreview", "using namespace SchemataPreview" } }
					}
				);
				Children.Add<ExcludeModel>("*.ps1");

				Model schematic = Children["Get-CurrentDirectorySchema.ps1"];
				Debug.Assert(schematic is StrictTextModel && schematic.Exists);

				using PowerShell instance = PowerShell.Create();
				List<Schema> schemata = new();
				foreach (PSObject @object in instance.AddScript(schematic).Invoke())
				{
					if (@object.BaseObject is Schema s)
					{
						schemata.Add(s);
					}
				}
				Children.Add(schemata.ToArray());
			};
		}
	}
}
