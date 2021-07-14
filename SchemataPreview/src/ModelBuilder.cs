using System.IO;

namespace SchemataPreview
{
	public class ModelBuilder
	{
		public ModelBuilder(Model model)
		{
			Model = model;
		}

		private Model Model { get; set; }

		public Model Mount(string path)
		{
			Model.FullName = Path.Combine(path, Model.Name);
			Mount(Model);
			return Model;
		}

		private static void Mount(Model model)
		{
			if (model.Exists)
			{
				if (model.ShouldHardMount)
				{
					model.InvokeEvent(EventOption.Delete);
					model.InvokeEvent(EventOption.Create);
				}
			}
			else
			{
				model.InvokeEvent(EventOption.Create);
			}
			foreach (Model child in model.Children)
			{
				child.FullName = Path.Combine(model.FullName, child.Name);
				child.Parent = model;
				Mount(child);
			}
			if (!model.IsMounted)
			{
				model.IsMounted = true;
				model.InvokeEvent(EventOption.PostMount);
			}
		}

		public Model Dismount()
		{
			Dismount(Model);
			return Model;
		}

		private static void Dismount(Model model)
		{
			if (model.IsMounted)
			{
				model.InvokeEvent(EventOption.PreDismount);
				model.FullName = null;
				model.Parent = null;
				model.IsMounted = false;
			}
			foreach (Model child in model.Children)
			{
				Dismount(child);
			}
		}
	}
}
