using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemataPreview
{
	internal class HierarchyHandler : Handler
	{
		public HierarchyHandler(Model model)
			: base(model)
		{
		}

		public Model? Parent { get; set; }
		public List<Model> Children { get; } = new();

		public void AddChildren(params Model[] models)
		{
			foreach (Model model in models)
			{
				model.Parent = Model;
				Children.RemoveAll(child => child.Name == model.Name);
				Children.Add(model);
				if (Model.IsMounted)
				{
					model.Mount(Model.FullName);
				}
			}
		}

		public Model? SelectChild(string name)
		{
			return Children.Find(child => child.Name == name);
		}
	}
}
