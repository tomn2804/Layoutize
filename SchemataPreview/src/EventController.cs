using System.IO;

namespace SchemataPreview
{
	public static class EventController
	{
		public static void Mount(string path, Model model)
		{
			model.FullName = Path.Combine(path, model.Name);
			Mount(model);
		}

		internal static void Mount(Model model)
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
			model.Children.ForEach(child =>
			{
				child.FullName = Path.Combine(model.FullName, child.Name);
				child.Parent = model;
				Mount(child);
			});
			if (!model.IsMounted)
			{
				model.InvokeEvent(EventOption.Mount);
			}
		}

		public static void Dismount(Model model)
		{
			if (model.IsMounted)
			{
				model.InvokeEvent(EventOption.Dismount);
			}
			model.Children.ForEach(child => Dismount(child));
		}

		public static void Create(Model model)
		{
			if (!model.IsMounted)
			{
				throw new ModelNotMountedException(model);
			}
			if (!model.Exists)
			{
				model.InvokeEvent(EventOption.Create);
			}
			model.Children.ForEach(child => Create(child));
		}

		public static void Delete(Model model)
		{
			if (!model.IsMounted)
			{
				throw new ModelNotMountedException(model);
			}
			if (model.Exists)
			{
				model.InvokeEvent(EventOption.Delete);
			}
			model.Children.ForEach(child => Delete(child));
		}

		public static void Update(Model model)
		{
			if (!model.IsMounted)
			{
				throw new ModelNotMountedException(model);
			}
			if (model.Exists)
			{
				model.InvokeEvent(EventOption.Update);
			}
			model.Children.ForEach(child => Update(child));
		}

		public static void Cleanup(Model model)
		{
			if (!model.IsMounted)
			{
				throw new ModelNotMountedException(model);
			}
			if (model.Exists)
			{
				model.InvokeEvent(EventOption.Cleanup);
			}
			model.Children.ForEach(child => Cleanup(child));
		}
	}
}
