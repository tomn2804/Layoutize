using System.Management.Automation;

namespace SchemataPreview
{
	public class BranchDirectoryModel : StrictDirectoryModel
	{
		public override void Build()
		{
			base.Build();
			StrictTextModel schema = (new Schema<StrictTextModel> { { "Name", "Get-CurrentDirectorySchema.ps1" } }).BuildTo(this);
			Children.Add(
				(new Schema<ExcludeModel> { { "Name", "*.ps1" } }).BuildTo(this),
				schema
			);
			if (schema.Exists)
			{
				using PowerShell instance = PowerShell.Create().AddScript(schema);
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
