namespace SchemataPreview
{
	internal abstract class Handler
	{
		protected Handler(Model model)
		{
			Model = model;
		}

		protected Model Model { get; private set; }
	}
}
