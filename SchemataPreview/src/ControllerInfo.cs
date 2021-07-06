using SchemataPreview.Models;

namespace SchemataPreview
{
	public class ControllerInfo
	{
		public Model Model { get; private set; }

		public ControllerInfo(Model model)
		{
			Model = model;
		}

		public ControllerInfo Mount(string path)
		{
			Model.FullName = path;
			ModelController.Mount(Model);
			return this;
		}

		public ControllerInfo Dismount()
		{
			ModelController.Dismount(Model);
			return this;
		}

		public ControllerInfo OnCreate()
		{
			ModelController.OnCreate(Model);
			return this;
		}

		public ControllerInfo OnDelete()
		{
			ModelController.OnDelete(Model);
			return this;
		}
	}
}
