using System.IO;

namespace SchemataPreview
{
	public static class Controller
	{
		public static void Mount(string path, Model model)
		{
			model.FullName = Path.Combine(path, model.Name);
			model.Configure();
			Mount(model);
		}

		internal static void Mount(Model model)
		{
			if (model.Exists)
			{
				if (model.ShouldHardMount)
				{
					model.DeleteActions.ForEach(action => action());
					model.CreateActions.ForEach(action => action());
				}
			}
			else
			{
				model.CreateActions.ForEach(action => action());
			}
			model.Children.ForEach(child => Mount(child));
			if (!model.IsMounted)
			{
				model.MountActions.ForEach(action => action());
			}
		}

		public static void Dismount(Model model)
		{
			if (model.IsMounted)
			{
				model.DismountActions.ForEach(action => action());
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
				model.CreateActions.ForEach(action => action());
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
				model.DeleteActions.ForEach(action => action());
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
				model.UpdateActions.ForEach(action => action());
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
				model.CleanupActions.ForEach(action => action());
			}
			model.Children.ForEach(child => Cleanup(child));
		}
	}
}
