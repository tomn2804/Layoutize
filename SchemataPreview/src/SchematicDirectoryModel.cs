using System.Collections.Generic;
using System.Diagnostics;
using System.Management.Automation;

namespace SchemataPreview
{
	public class SchematicDirectoryModel : StrictDirectoryModel
	{
		public SchematicDirectoryModel(ReadOnlySchema schema)
			: base(schema)
		{
			PipeAssembly[PipelineOption.Mount].OnProcessed += () =>
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
				foreach (PSObject obj in instance.AddScript(schematic).Invoke())
				{
					if (obj.BaseObject is Schema s)
					{
						schemata.Add(s);
					}
				}
				Children.Add(schemata.ToArray());
			};
		}
	}
}
