using System.Management.Automation;

namespace SchemataPreview
{
	public class SchematicDirectoryModel : StrictDirectoryModel
	{
		public override void Mount()
		{
			base.Mount();
			StrictTextModel schematic = (new Schema<StrictTextModel> { { "Name", "Get-CurrentDirectorySchema.ps1" } }).BuildTo(this);
			Children.AddOrReplace(
				(new Schema<ExcludeModel> { { "Name", "*.ps1" } }).BuildTo(this),
				schematic
			);
			if (schematic.Exists)
			{
				using PowerShell instance = PowerShell.Create().AddScript(schematic);
				foreach (PSObject obj in instance.Invoke())
				{
					if (obj.BaseObject is Schema child)
					{
						Children.Add(child.BuildTo(this));
					}
				}
			}
		}
	}
}
