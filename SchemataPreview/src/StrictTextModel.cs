namespace SchemataPreview
{
	public class StrictTextModel : TextModel
	{
		public StrictTextModel(ImmutableSchema schema)
			: base(schema)
		{
			PipeAssembly.Register(PipelineOption.Format).OnProcessing += () =>
			{
				Contents = TextEditor.Format(Contents);
			};
		}
	}
}
