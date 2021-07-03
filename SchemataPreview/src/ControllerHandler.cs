using SchemataPreview.Models;
using System.IO;

namespace SchemataPreview
{
	public class ControllerHandler
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

		public static void Create(Model model)
		{
			if (!model.Exists())
			{
				model.Create();
			}
			foreach (Model child in model.Children)
			{
				Create(child);
			}
		}

		public static void Delete(Model model)
		{
			if (model.Exists())
			{
				model.Delete();
			}
			foreach (Model child in model.Children)
			{
				Delete(child);
			}
		}

		//public static void Clear(Model model)
		//{
		//	if (model.Exists())
		//	{
		//		model.Clear();
		//	}
		//	foreach (Model child in model.Children)
		//	{
		//		Clear(child);
		//	}
		//}

		//public static void Format(Model model)
		//{
		//	if (model.Exists())
		//	{
		//		model.Format();
		//	}
		//	foreach (Model child in model.Children)
		//	{
		//		Format(child);
		//	}
		//}
	}
}
