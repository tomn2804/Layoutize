#nullable enable

namespace SchemataPreview
{
	public class ModelList : UniqueList<Model>
	{
		public Model? this[string name] => Find(model => model.Name == name);

		public void Add(params Model[] models)
		{
			AddRange(models);
		}

		public bool Contains(string name)
		{
			return this[name] != null;
		}
	}
}

#nullable disable
