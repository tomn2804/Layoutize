using System;
using System.IO;
using System.Management.Automation;

namespace SchemataPreview
{
	public abstract partial class Model
	{
		public abstract ModelSet? Children { get; }
		public abstract bool Exists { get; }

		public string Name { get; }
		public Model? Parent { get; }
		public bool PassThru { get; }
		public PipeAssembly PipeAssembly { get; }
		public PipelineTraversalOption Traversal { get; }
		public string FullName => Path.Combine(Parent?.FullName ?? (string)Schema["Path"], Name);
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

		protected Model(ImmutableSchema schema)
		{
			Schema = schema;
			PipeAssembly = new(this);

			schema.TryGetValue("Name", out object? name);
			Name = (string?)name ?? throw new NullReferenceException(nameof(name));

			schema.TryGetValue("Parent", out object? parent);
			Parent = (Model?)parent;

			schema.TryGetValue("PassThru", out object? passThru);
			PassThru = (bool?)passThru ?? false;

			schema.TryGetValue("Traversal", out object? traversal);
			Traversal = (PipelineTraversalOption?)traversal ?? PipelineTraversalOption.PreOrder;
		}

		protected ImmutableSchema Schema { get; }
	}

	public abstract partial class Model : IComparable<Model>
	{
		public int CompareTo(Model? other)
		{
			if (other != null)
			{
				Schema.TryGetValue("Priority", out object? lhs_priority);
				other.Schema.TryGetValue("Priority", out object? rhs_priority);
				return ((int?)lhs_priority)?.CompareTo(rhs_priority) ?? Name.CompareTo(other.Name);
			}
			return 1;
		}
	}
}
