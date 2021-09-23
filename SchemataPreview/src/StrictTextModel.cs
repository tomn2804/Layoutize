namespace SchemataPreview
{
	public class StrictTextModel : TextModel
	{
		public StrictTextModel(ImmutableSchema schema)
			: base(schema)
		{
			PipeAssembly.Register(PipeOption.Format).OnProcessing += (_, _) =>
			{
				Contents = TextEditor.Format(Contents);
			};
		}
	}
}
