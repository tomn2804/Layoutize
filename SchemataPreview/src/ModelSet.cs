using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SchemataPreview
{
	public partial class ModelSet
	{
		public ModelSet(Model parent)
		{
			Parent = parent;
		}

		public Model? this[string name] => Models.FirstOrDefault(model => model.Equals(name));

		public Model Parent { get; init; }
		protected SortedSet<Model> Models { get; private set; } = new(new ModelComparer());

		public void Mount()
		{
			Models.Clear();
			if (Parent.Schema["Children"] is Schema[] schemata)
			{
				Add(schemata);
			}
		}

		public void Add(params Schema[] schemata)
		{
			switch (Parent.Schema["Traversal"])
			{
				case "ReversePostOrder":
					foreach (Schema schema in schemata)
					{
						Model child = schema.NewModel();
						child.Parent = Parent;
						Models.Add(child);
						child.Build();
					}
					foreach (Model child in Models)
					{
						if (child.Children != null)
						{
							ModelBuilder.HandleMount(child.Children);
						}
					}
					break;

				case "ReversePreOrder":
				default:
					foreach (Schema schema in schemata)
					{
						Model child = schema.NewModel();
						child.Parent = Parent;
						Models.Add(child);
						ModelBuilder.HandleMount(child);
					}
					break;
			}
		}

		public bool Contains(string name)
		{
			return Models.Any(model => model.Equals(name));
		}

		public bool Remove(string name)
		{
			Model model = Models.First(model => model.Equals(name));
			ModelBuilder.HandleDelete(model);
			return Models.Remove(model);
		}
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
