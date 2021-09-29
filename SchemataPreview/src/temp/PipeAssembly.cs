using System.Collections.Generic;

namespace SchemataPreview
{
	public class PipeAssembly : Dictionary<object, PipeSegment>
	{
		public PipeAssembly(Model model)
		{
			Model = model;
		}

		public Model Model { get; }

		public PipeSegment Register(object key)
		{
			PipeSegment value = new(Model);
			Add(key, value);
			return value;
		}

		public void Unregister(object key)
		{
			Remove(key);
		}
	}
}
