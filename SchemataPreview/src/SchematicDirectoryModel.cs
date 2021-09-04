﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Management.Automation;

namespace SchemataPreview
{
	public class SchematicDirectoryModel : StrictDirectoryModel
	{
		public override void Mount()
		{
			base.Mount();
			Children.Add(
				new Schema<ExcludeModel> { { "Name", "*.ps1" } },
				new Schema<StrictTextModel> {
					{ "Name", "Get-CurrentDirectorySchema.ps1" },
					{ "Contents", new string[] { "#Requires -Module SchemataPreview", "using namespace SchemataPreview" } }
				}
			);

			Model? schematic = Children["Get-CurrentDirectorySchema.ps1"];
			Debug.Assert(schematic != null && schematic.Exists);

			using PowerShell shell = PowerShell.Create();
			List<Schema> schemata = new();
			foreach (PSObject obj in shell.AddScript(schematic).Invoke())
			{
				if (obj.BaseObject is Schema schema)
				{
					schemata.Add(schema);
				}
			}
			Children.Add(schemata.ToArray());
		}
	}
}
