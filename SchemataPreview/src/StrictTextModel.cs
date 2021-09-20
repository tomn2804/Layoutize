namespace SchemataPreview
{
	public class StrictTextModel : TextModel
	{
		public StrictTextModel(ReadOnlySchema schema)
			: base(schema)
		{
			PipeAssembly.Register(PipelineOption.Format).OnProcessing += () =>
			{
				Contents = TextEditor.Format(Contents);
			});
		}
	}
}
