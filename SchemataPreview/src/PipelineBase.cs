namespace SchemataPreview
{
	public abstract class PipelineBase
	{
		protected PipelineBase(Model model)
		{
			Model = model;
		}

		protected Model Model { get; }
	}
}
