using System;
using System.IO;
using System.Management.Automation;

namespace SchemataPreview
{
	public abstract partial class Model
	{
		public Model(ImmutableSchema schema)
		{
			Schema = schema;
		}

		public abstract ModelSet? Children { get; }

		public abstract bool Exists { get; }

		public Model? Parent { get; private set; }

		public bool PassThru { get; }

		public PipeAssembly PipeAssembly { get; } = new();

		public PipelineTraversalOption? Traversal => Schema["Traversal"];

		public string FullName => Path.Combine(Parent?.FullName ?? Schema["Path"], Name);

		public string Name => Schema.Name;

		public string RelativeName => Path.Combine(Parent?.RelativeName ?? string.Empty, Name);

		public static implicit operator string(Model rhs)
		{
			return rhs.FullName;
		}

		public Action CaptureContext(ScriptBlock script, params object[] args)
		{
			script = script.GetNewClosure();
			return () => script.InvokeWithContext(null, new()
			{
				{ new PSVariable("this", this) },
				{ new PSVariable("_", Schema) }
			}, args);
		}

		protected dynamic Schema { get; }
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
