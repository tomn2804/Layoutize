using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;

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
				Model child = schema.GetNewModel();
				Models.Add(child);
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
					Pipeline.TraverseReversePostOrder(PipelineOption.Mount, models);
					break;

				case PipelineTraversalOption.PreOrder:
				default:
					Pipeline.TraverseReversePreOrder(PipelineOption.Mount, models);
					break;
			}
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

		//public bool MatchName(string pattern)
		//{
		//	return Models.Any(model => model.Pattern.IsMatch(pattern));
		//}

		//public bool Remove(Model model)
		//{
		//	ModelBuilder.HandleDelete(model);
		//	return Models.Remove(model);
		//}

		//public bool RemoveByName(string name)
		//{
		//	return Remove(Models.First(model => model.Equals(name)));
		//}

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
