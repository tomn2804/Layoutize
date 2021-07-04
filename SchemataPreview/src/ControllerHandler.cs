using SchemataPreview.Models;
using System.IO;

namespace SchemataPreview
{
	public class ControllerHandler
	{
		public interface IMount
		{
			public string Name { get; internal set; }
			public string FullName { get; internal set; }

			public bool IsMounted { get; internal set; }

			public Model Parent { get; internal set; }
			public List<Model> Children { get; internal set; }

			bool Exists();
		}

		public interface ICreate : IMount
		{
			void Create();
		}

		public interface IDelete : IMount
		{
			void Delete();
		}

		public static void Mount(Model model)
		{
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

		public static void Create(ICreate model)
		{
			if (!model.IsMounted)
			{
				throw new ModelNotMountedException(model);
			}
			if (!model.Exists())
			{
				model.Create();
			}
			foreach (ICreate child in model.Children)
			{
				Create(child);
			}
		}

		public static void Delete(IDelete model)
		{
			if (!model.IsMounted)
			{
				throw new ModelNotMountedException(model);
			}
			if (model.Exists())
			{
				model.Delete();
			}
			foreach (IDelete child in model.Children)
			{
				Delete(child);
			}
		}
	}
}
