using System.IO;

namespace SchemataPreview
{
	public abstract class Model
	{
		public abstract bool Exists { get; }

		public string Name => (string)Schema["Name"];
		public Model? Parent => (Model?)Schema["Parent"];
		public string FullName => Path.Combine(Parent?.FullName ?? (string)Schema["Path"], Name);
		public string RelativeName => Path.Combine(Parent?.RelativeName ?? string.Empty, Name);

		public static implicit operator string(Model @this)
		{
			return @this.FullName;
		}

		public abstract void Create();

		public abstract void Delete();

		public override string ToString()
		{
			return FullName;
		}

		protected Model(ImmutableSchema schema)
		{
			Schema = schema;
		}

		protected ImmutableSchema Schema { get; }
	}
}
