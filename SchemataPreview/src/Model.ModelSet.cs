using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;

namespace SchemataPreview
{
	public abstract partial class Model
	{
		public partial class ModelSet
		{
			public ModelSet(Model parent)
			{
				Parent = parent;
			}

			public Model Parent { get; init; }
			public int Count => Models.Count;
			public Model? this[string name] => Models.FirstOrDefault(model => model.Equals(name));

			public void Add(params Schema[] schemata)
			{
				ModelSet children = new(Parent);
				foreach (Schema schema in schemata)
				{
					schema["Parent"] = Parent;
					Model child = schema.NewModel();
					if (Models.Add(child))
					{
						children.Models.Add(child);
					}
				}
				children.Mount();
			}

			public void Add<T>(params string[] patterns) where T : Model, new()
			{
				Add<T, T>(patterns);
			}

			public void Add<TDirectory, TFile>(params string[] patterns) where TDirectory : Model, new() where TFile : Model, new()
			{
				List<Schema> children = new();
				foreach (string pattern in patterns)
				{
					foreach (string path in Directory.GetDirectories(Parent, pattern))
					{
						children.Add(new Schema<TDirectory> { { "Name", Path.GetFileName(path) } });
					}
					foreach (string path in Directory.GetFiles(Parent, pattern))
					{
						children.Add(new Schema<TFile> { { "Name", Path.GetFileName(path) } });
					}
				}
				Add(children.ToArray());
			}

			public bool Contains(Model model)
			{
				return Models.Contains(model);
			}

			public bool ContainsName(string name)
			{
				return Models.Any(model => model.Equals(name));
			}

			public override bool InvokeCallback(string name)
			{
				return Parent.InvokeCallback(name);
			}

			public bool MatchName(string pattern)
			{
				return Models.Any(model => model.Pattern.IsMatch(pattern));
			}

			public void Mount()
			{
				Models.Clear();
				if (Parent.Schema["Children"] is object[] schemata)
				{
					Add(Array.ConvertAll(schemata, schema => (Schema)schema));
				}
			}

			public bool Remove(Model model)
			{
				ModelBuilder.HandleDelete(model);
				return Models.Remove(model);
			}

			public bool RemoveByName(string name)
			{
				return Remove(Models.First(model => model.Equals(name)));
			}

			protected SortedSet<Model> Models { get; } = new(new ModelComparer());
		}

		public partial class ModelSet : IEnumerable<Model>
		{
			public IEnumerator<Model> GetEnumerator()
			{
				return Models.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return Models.GetEnumerator();
			}
		}
	}
}
