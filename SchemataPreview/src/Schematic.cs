using System.Management.Automation;

namespace SchemataPreview.Models
{
	public class Schematic : Directory
	{
		public Schematic(string name)
			: base(name)
		{
			UseChildren(
				new Exclude("*.ps1"),
				new Text("Get-ModelSchema.ps1")
			);
		}

		public override void ModelDidMount()
		{
			base.ModelDidMount();
			using (PowerShell instance = PowerShell.Create().AddScript(SelectChild("Get-ModelSchema.ps1").FullName))
			{
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
