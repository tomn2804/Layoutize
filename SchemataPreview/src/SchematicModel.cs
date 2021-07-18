using System.Management.Automation;

namespace SchemataPreview
{
	public class SchematicModel : DirectoryModel
	{
		public override void Build(Builder builder)
		{
			base.Build(builder);
			TextModel schema = new() { Name = "Get-CurrentDirectorySchema.ps1" };
			builder.AddChildren(
				new ExcludeModel() { Name = "*.ps1" },
				schema
			);
			if (schema.Exists)
			{
				using PowerShell instance = PowerShell.Create().AddScript(schema.FullName);
				foreach (PSObject obj in instance.Invoke())
				{
					if (obj.BaseObject is Model model)
					{
						builder.AddChildren(model);
					}
				}
			}
		}
	}
}
