namespace Schemata
{
	public class StrictTextModel : TextModel
	{
		public StrictTextModel(ImmutableDefinition Outline)
			: base(outline)
		{
			PipeAssembly.Register(PipeOption.Format).OnProcessing += (_, _) =>
			{
				Contents = TextEditor.Format(Contents);
			};
		}
	}
}
