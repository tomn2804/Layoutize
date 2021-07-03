using System;
using System.Linq;
using System.Collections.Generic;

namespace SchemataPreview.Models
{
	public abstract class Model
	{
		public string Name { get; internal set; }
		public string FullName { get; internal set; }
		public List<Model> Schema { get; internal set; }

		public Model Parent { get; internal set; }
		public bool IsMounted { get; internal set; }
		public bool ShouldHardMount { get; internal set; }

		public Model(string name)
		{
			Name = name;
			Schema = new List<Model>();
		}

		public Model Use(params Model[] models)
		{
			foreach (Model model in models)
			{
				// TODO: Dismount model
				Schema.RemoveAll(m => m.Name == model.Name);
				Schema.Add(model);
			}
			return this;
		}

		public Model SelectFromSchema(string name)
		{
			return Schema.Find(model => model.Name == name);
		}

		public Model[] SelectFromSchema(params string[] names)
		{
			return Schema.FindAll(model => names.Contains(model.Name)).ToArray();
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
