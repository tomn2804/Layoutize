using System.Management.Automation;

namespace SchemataPreview
{
	public class SchematicModel : DirectoryModel
	{
		public SchematicModel(string name)
			: base(name)
		{
		}

		public override void Configure()
		{
			base.Configure();
			TextModel schema = new("Get-ModelSchema.ps1");
			UseChildren(
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
						UseChildren(model);
					}
				}
			}
		}
	}
}
