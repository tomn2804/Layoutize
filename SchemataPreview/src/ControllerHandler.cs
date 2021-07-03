using System;
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
			foreach (Model m in model.Schema)
			{
				m.FullName = Path.Combine(model.FullName, m.Name);
				m.Parent = model;
				Mount(m);
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
			foreach (Model m in model.Schema)
			{
				Dismount(m);
			}
		}

		public static void Create(Model model)
		{
			if (!model.Exists())
			{
				model.Create();
			}
			foreach (Model m in model.Schema)
			{
				Create(m);
			}
		}

		public static void Delete(Model model)
		{
			if (model.Exists())
			{
				model.Delete();
			}
			foreach (Model m in model.Schema)
			{
				Delete(m);
			}
		}

		public static void Clear(Model model)
		{
			if (model.Exists())
			{
				model.Clear();
			}
			foreach (Model m in model.Schema)
			{
				Clear(m);
			}
		}

		public static void Format(Model model)
		{
			if (model.Exists())
			{
				model.Format();
			}
			foreach (Model m in model.Schema)
			{
				Format(m);
			}
		}
	}
}
