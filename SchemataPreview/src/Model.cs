using System;
using System.Linq;
using System.Collections.Generic;

namespace SchemataPreview.Models
{
	public abstract class Model
	{
		public string Name { get; internal set; }
		public string FullName { get; internal set; }

		public Model Parent { get; internal set; }
		public List<Model> Children { get; internal set; }

		public bool IsMounted { get; internal set; }
		public bool ShouldHardMount { get; internal set; }

		public Model(string name) => Name = name;

		public Model UseChildren(params Model[] models)
		{
			foreach (Model model in models)
			{
				// TODO: Dismount model
				Children.RemoveAll(child => child.Name == model.Name);
				Children.Add(model);
			}
			return this;
		}

		public Model SelectFromSchema(string name)
		{
			return Children.Find(child => child.Name == name);
		}

		public Model[] SelectFromSchema(params string[] names)
		{
			return Children.FindAll(child => names.Contains(child.Name)).ToArray();
		}

		public abstract void Create();

		public abstract void Delete();

		public abstract bool Exists();

		public void ModelDidMount()
		{
		}

		public void ModelWillDismount()
		{
		}
	}
}
