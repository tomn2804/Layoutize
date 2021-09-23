using System.Threading.Tasks;

namespace SchemataPreview
{
	public class PipelineAsync : PipelineBase
	{
		public PipelineAsync(Model model)
			: base(model)
		{
		}

		public async Task Invoke(object key)
		{
			Pipeline pipeline = new(Model);
			await Task.Run(() => pipeline.Invoke(key));
		}
	}
}
