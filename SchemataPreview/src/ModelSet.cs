using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SchemataPreview
{
	public partial class ModelSet
	{
		public ModelSet(Model parent)
		{
			Parent = parent;
		}

		public ModelSet(Model parent, Schema[] schemata)
			: this(parent)
		{
			foreach (Schema schema in schemata)
			{
				schema["Parent"] = parent;
				Models.Add(schema.GetNewModel());
			}
		}

		public Model Parent { get; }
		public int Count => Models.Count;
		public Model this[string name] => Models.First(model => model.Name == name);

		public void Add(params Schema[] schemata)
		{
			SortedSet<Model> models = new(new ModelComparer());
			foreach (Schema schema in schemata)
			{
				schema["Parent"] = Parent;
				Model child = schema.GetNewModel();
				if (Models.Add(child))
				{
					models.Add(child);
				}
			}
			switch (Parent.Traversal)
			{
				case PipelineTraversalOption.PostOrder:
					PipelineSequential.TraverseReversePostOrder(PipeOption.Mount, models);
					break;

				case PipelineTraversalOption.PreOrder:
				default:
					PipelineSequential.TraverseReversePreOrder(PipeOption.Mount, models);
					break;
			}
		}

		public void Add<T>(params string[] patterns) where T : Model
		{
			Add<T, T>(patterns);
		}

		public void Add<TDirectory, TFile>(params string[] patterns) where TDirectory : Model where TFile : Model
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

		public void Remove(Model model)
		{
			(new Pipeline(model)).Invoke(PipeOption.Delete);
			Models.Remove(model);
		}

		public void RemoveByName(string name)
		{
			Remove(Models.First(model => model.Equals(name)));
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
