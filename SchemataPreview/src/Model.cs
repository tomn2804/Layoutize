using System;
using System.Linq;
using System.Collections.Generic;

namespace SchemataPreview.Models
{
	public abstract class Model
	{
		public string Name { get; private set; }
		public List<Model> Schema { get; private set; }

		public Model(string name)
		{
			this.Name = name;
			this.Schema = new List<Model>();
		}

		public Model Use(params Model[] models)
		{
			foreach (Model model in models)
			{
				// TODO: Dismount model
				this.Schema.RemoveAll(m => m.Name == model.Name);
				this.Schema.Add(model);
			}
			return this;
		}

		public Model SelectFromSchema(string name)
		{
			return this.Schema.Find(model => model.Name == name);
		}

		public List<Model> SelectFromSchema(params string[] names)
		{
			return this.Schema.FindAll(model => names.Contains(model.Name));
		}
	}
}
