using System.Management.Automation;

namespace SchemataPreview
{
	public class BranchDirectoryModel : Model<StrictDirectoryModel>
	{
		protected void OnBuild()
		{
			Schema.AddChildren(
				new Schema<ExcludeModel> { { "Name", "*.ps1" } },
				new Schema<StrictTextModel> { { "Name", "Get-CurrentDirectorySchema.ps1" } }
			);
			if (Schema.GetChild("Get-CurrentDirectorySchema.ps1").Build() is var schema && schema.Exists)
			{
				using PowerShell instance = PowerShell.Create().AddScript(schema);
				foreach (PSObject obj in instance.Invoke())
				{
					if (obj.BaseObject is Schema child)
					{
						Schema.AddChildren(child);
					}
				}
			}
		}
	}
}
