using System.Management.Automation;

namespace SchemataPreview
{
	public class SchematicModel : DirectoryModel
	{
		public override void PresetConfiguration()
		{
			base.PresetConfiguration();
			TextModel schema = new("Get-ModelSchema.ps1");
			AddChildren(
				new ExcludeModel("*.ps1"),
				schema
			);
			if (schema.Exists)
			{
				using PowerShell instance = PowerShell.Create().AddScript(schema.FullName);
				foreach (PSObject obj in instance.Invoke())
				{
					if (obj.BaseObject is Model model)
					{
						AddChildren(model);
					}
				}
			}
		}
	}
}
