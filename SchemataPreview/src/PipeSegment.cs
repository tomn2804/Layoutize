namespace SchemataPreview
{
	public class PipeSegment
	{
		public PipeSegment(Model model)
		{
			OnProcessed = new(model);
			OnProcessing = new(model);
		}

		public PipeEventHandler OnProcessed { get; }
		public PipeEventHandler OnProcessing { get; }
	}
}
