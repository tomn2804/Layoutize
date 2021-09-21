using System;
using System.IO;

namespace SchemataPreview
{
	public abstract partial class Model
	{
		public Model(ReadOnlySchema schema)
		{
			Schema = schema;
		}

		public abstract ModelSet? Children { get; }
		public abstract bool Exists { get; }

		public Model? Parent { get; private set; }
		public PipeAssembly PipeAssembly { get; } = new();
		public PipelineTraversalOption? TraversalOption => Schema["Traversal"];

		public string FullName => Path.Combine(Parent?.FullName ?? Schema.Path, Name);
		public string RelativeName => Path.Combine(Parent?.RelativeName ?? string.Empty, Name);
		public string Name => Schema.Name;

		public static implicit operator string(Model rhs)
		{
			return rhs.FullName;
		}

		protected dynamic Schema { get; init; }
	}

	public abstract partial class Model : IComparable<Model>
	{
		public int CompareTo(Model? other)
		{
			if (other != null)
			{
				return Schema["Priority"]?.CompareTo(other.Schema["Priority"]) ?? Name.CompareTo(other.Name);
			}
			return 1;
		}
	}
}
