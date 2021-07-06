using System.IO;

namespace SchemataPreview
{
	public static class Controller
	{
		public static void Mount(Model model)
		{
			// TODO: check for init
			if (model.Exists())
			{
				if (model.ShouldHardMount)
				{
					model.Delete();
					model.Create();
				}
			}
			else
			{
				model.Create();
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
				model.ModelDidMount();
			}
		}

		public static void Mount(Model model, params Model[] children)
		{
			model.UseChildren(children);
			try
			{
				System.IO.Directory.CreateDirectory(FullName);
			}
			catch (Exception e)
			{
				Console.WriteLine($"Error: {e}");
			}

			// TODO: check for init
			if (model.Exists())
			{
				if (model.ShouldHardMount)
				{
					model.Delete();
					model.Create();
				}
			}
			else
			{
				model.Create();
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
				model.ModelDidMount();
			}
		}

		public static void Dismount(Model model)
		{
			if (model.IsMounted)
			{
				model.ModelWillDismount();
				model.FullName = null;
				model.Parent = null;
				model.IsMounted = false;
			}
			foreach (Model child in model.Children)
			{
				Dismount(child);
			}
		}

		public static void OnCreate(ICreate model)
		{
			if (!model.IsMounted)
			{
				throw new ModelNotMountedException(model);
			}
			if (!model.Exists())
			{
				model.OnCreate();
			}
			foreach (ICreate child in model.Children)
			{
				OnCreate(child);
			}
		}

		public static void OnDelete(IDelete model)
		{
			if (!model.IsMounted)
			{
				throw new ModelNotMountedException(model);
			}
			if (model.Exists())
			{
				model.OnDelete();
			}
			foreach (IDelete child in model.Children)
			{
				OnDelete(child);
			}
		}
	}
}
