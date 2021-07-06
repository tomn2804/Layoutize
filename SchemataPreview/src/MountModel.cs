using SchemataPreview.Models;
using System.Management.Automation;

namespace SchemataPreview
{
	[Cmdlet(VerbsData.Mount, "Model")]
	[OutputType(typeof(Model))]
	public class MountModel : Cmdlet
	{
		[Parameter(
			Position = 0,
			ValueFromPipeline = true,
			ValueFromPipelineByPropertyName = true
		)]
		public Model[] Model { get; set; }
	}
}
