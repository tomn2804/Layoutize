using System.Collections.Generic;

namespace SchemataPreview
{
	public class PipeAssembly : Dictionary<object, PipeSegment>
	{
		public PipeAssembly(Model model)
		{
			Model = model;
		}

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

		internal bool Build(object key, in Pipe pipe)
		{
			if (this.TryGetValue(key) is PipeSegment segment)
			{
				pipe.Extend(segment);
				return Model.Children != null;
			}
			return Model.PassThru && (Model.Children != null);
		}

		protected Model Model { get; }
	}
}
