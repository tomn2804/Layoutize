using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace SchemataPreview
{
	public abstract class ModelTree
	{
		public Model? Parent { get; private set; }
		public List<Model> Children { get; private set; }

		public virtual void InternalConfigure(ReadOnlySchema schema)
		{
			Parent = (Model)schema["Parent"].Model;
			Children = ((List<Schema>)schema["Children"]).Select(s => (Model)s["Model"]).ToList();
		}

		public virtual void Configure(ReadOnlySchema schema)
		{
		}

		public void AddChildren(params Schema[] schemata)
		{
			foreach (Schema schema in schemata)
			{
				Model model = schema;
				int existingChild = Children.FindIndex(child => child.Name == schema.Name);
				if (existingChild != -1)
				{
					if (force)
					{
						Children.RemoveAt(existingChild);
					}
					else
					{
						throw;
					}
				}
				schema.Cache.ClearAll();
				schema._parent.Value = this;
				Children.Add(schema);
			}
		}

		public Model GetChild(string name)
		{
			Model result;
			if (!TryGetChild(name, out result))
			{
				throw new ObjectNotFoundException();
			}
			return result;
		}

		public bool TryGetChild(string name, out Model result)
		{
			result = Children.Find(child => child.Name == name);
			return result != null;
		}

		public bool HasChild(string name)
		{
			return Children.Any(child => child.Name == name);
		}

		public string Name { get; private set; }
		public string FullName => Parent != null ? Path.Combine(Parent.FullName, Name) : Name;

		public override void InternalConfigure(ReadOnlySchema schema)
		{
			base.InternalConfigure(schema);
			Name = (string)schema["Name"];
		}

		public void AddEventListener(object key, Action callback)
		{
		}

		public abstract bool Exists { get; }

		public static implicit operator string(Model rhs) => rhs.FullName;
	}

	public abstract class Schema : Dictionary<object, object>
	{
		protected internal Schema()
			: base()
		{
		}

		protected internal Schema(Hashtable schema)
			: base(schema.Cast<DictionaryEntry>().ToDictionary(entry => entry.Key, entry => entry.Value))
		{
		}

		public abstract void Build(string path);
	}

	public class Schema<T> : Schema where T : Model, new()
	{
		internal Schema()
			: base()
		{
		}

		public Schema(Hashtable schema)
			: base(schema)
		{
		}

		public override void Mount(string path)
		{
			if (!(typeof(T) is AnchorDirectoryModel))
			{
				throw new InvalidOperationException();
			}
			this["FullName"] = Path.Combine(path, (string)this["Name"]);
			Mount(model);
		}

		private void Mount(Schema currentSchema)
		{
			ReadOnlySchema schema = new(currentSchema);
			T model = new();
			model.InternalConfigure(schema);
			model.Configure(schema);
			if (model.Exists)
			{
				if (model.UseHardMount)
				{
					model.InvokeEvent(EventOption.Delete);
					model.InvokeEvent(EventOption.Create);
				}
			}
			else
			{
				model.InvokeEvent(EventOption.Create);
			}
			foreach (Schema child in currentSchema["Children"])
			{
				child["FullName"] = Path.Combine((string)currentSchema["FullName"], (string)child["Name"]);
				Mount(child);
			}
			if (!model.IsMounted)
			{
				model.IsMounted = true;
				model.InvokeEvent(EventOption.PostMount);
			}
		}
	}
}
