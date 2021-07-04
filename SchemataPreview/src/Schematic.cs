using System.Management.Automation;

namespace SchemataPreview.Models
{
	public interface IMountEvent
	{
		void ModelDidMount();
	}

	public interface IDismountEvent
	{
		void ModelWillDismount();
	}

	public class Schematic : Directory, IMountEvent
	{
		public Schematic(string name)
			: base(name)
		{
			UseChildren(
				new Exclude("*.ps1"),
				new StaticText("Get-ModelSchema.ps1")
			);
		}

		public void ModelDidMount()
		{
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
			ControllerHandler.Mount(this);
		}
	}
}
