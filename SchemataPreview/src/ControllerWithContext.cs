namespace SchemataPreview
{
	public class ControllerWithContext
	{
		public Model Model { get; private set; }

		public ControllerWithContext(Model model)
		{
			Model = model;
		}

		public ControllerWithContext Mount(string path)
		{
			Model.FullName = path;
			Controller.Mount(Model);
			return this;
		}

		public ControllerWithContext Dismount()
		{
			Controller.Dismount(Model);
			return this;
		}

		public ControllerWithContext Create()
		{
			Controller.Create(Model);
			return this;
		}

		public ControllerWithContext OnDelete()
		{
			Controller.OnDelete(Model);
			return this;
		}
	}
}
