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
			UseChildren(
				new ExcludeModel("*.ps1"),
				new TextModel("Get-ModelSchema.ps1")
			);
			OnMount(() =>
			{
				using PowerShell instance = PowerShell.Create().AddScript(SelectChild("Get-ModelSchema.ps1").FullName);
				foreach (PSObject obj in instance.Invoke())
				{
					if (obj.BaseObject is Model model)
					{
						UseChildren(model);
					}
				}
			});
		}
	}
}
