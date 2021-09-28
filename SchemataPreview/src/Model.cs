using System;
using System.Diagnostics;
using System.IO;
using System.Management.Automation;

namespace SchemataPreview
{
	public abstract partial class Model<T>
	{
		protected Model(ImmutableSchema<T> schema)
		{
		}
	}

	//public abstract partial class Model
	//{
	//	public abstract ModelSet? Children { get; }
	//	public abstract bool Exists { get; }

	// public virtual string Name => _name; public virtual Model? Parent => _parent; public virtual
	// bool PassThru => _passThru; public virtual string Path => _path; public virtual PipeAssembly
	// PipeAssembly { get; } public virtual PipelineTraversalOption Traversal => _traversal; public
	// virtual string FullName => Path.Combine(Parent?.FullName ?? Path, Name); public virtual
	// string RelativeName => Path.Combine(Parent?.RelativeName ?? string.Empty, Name);

	// public static implicit operator string(Model value) { return value.FullName; }

	// public virtual Action CopyClosureTo(ScriptBlock script, params object[] args) { script =
	// script.GetNewClosure(); return () => script.InvokeWithContext(null, new() { { new
	// PSVariable("this", this) }, { new PSVariable("_", Schema) } }, args); }

	// public override string ToString() { return FullName; }

	// protected Model(ImmutableSchema schema) { Schema = schema; PipeAssembly = new(this);

	// schema.TryGetValue("Name", out object? name); Name = name is NameProperty ? name : new NameProperty(name);

	// _name = new(this); _parent = new(this); _passThru = new(this); _traversal = new(this); }

	//	protected ImmutableSchema Schema { get; }
	//	private readonly NameProperty _name;
	//	private readonly ParentProperty _parent;
	//	private readonly PassThruProperty _passThru;
	//	private readonly PathProperty _path;
	//	private readonly TraversalProperty _traversal;
	//}

	//public abstract partial class Model : IComparable<Model>
	//{
	//	public int CompareTo(Model? other)
	//	{
	//		if (other is not null)
	//		{
	//			Schema.TryGetValue("Priority", out object? lhs_priority);
	//			other.Schema.TryGetValue("Priority", out object? rhs_priority);
	//			return ((int?)rhs_priority)?.CompareTo(lhs_priority) ?? Name.CompareTo(other.Name);
	//		}
	//		return 1;
	//	}
	//}

	//public abstract partial class Model
	//{
	//	public abstract ModelSet? Children { get; }
	//	public abstract bool Exists { get; }

	// public virtual string Name => _name; public virtual Model? Parent => _parent; public virtual
	// bool PassThru => _passThru; public virtual string Path => _path; public virtual PipeAssembly
	// PipeAssembly { get; } public virtual PipelineTraversalOption Traversal => _traversal; public
	// virtual string FullName => Path.Combine(Parent?.FullName ?? Path, Name); public virtual
	// string RelativeName => Path.Combine(Parent?.RelativeName ?? string.Empty, Name);

	// public static implicit operator string(Model value) { return value.FullName; }

	// public virtual Action CopyClosureTo(ScriptBlock script, params object[] args) { script =
	// script.GetNewClosure(); return () => script.InvokeWithContext(null, new() { { new
	// PSVariable("this", this) }, { new PSVariable("_", Schema) } }, args); }

	// public override string ToString() { return FullName; }

	// protected Model(ImmutableSchema schema) { Schema = schema; PipeAssembly = new(this);

	// schema.TryGetValue("Name", out object? name); Name = name is NameProperty ? name : new NameProperty(name);

	// _name = new(this); _parent = new(this); _passThru = new(this); _traversal = new(this); }

	//	protected ImmutableSchema Schema { get; }
	//	private readonly NameProperty _name;
	//	private readonly ParentProperty _parent;
	//	private readonly PassThruProperty _passThru;
	//	private readonly PathProperty _path;
	//	private readonly TraversalProperty _traversal;
	//}
}
