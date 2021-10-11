namespace SchemataPreview
{
	public class StrictTextModel : TextModel
	{
		public StrictTextModel(ImmutableDefinition Props)
			: base(props)
		{
			PipeAssembly.Register(PipeOption.Format).OnProcessing += (_, _) =>
			{
				Contents = TextEditor.Format(Contents);
			};
		}
	}
}
