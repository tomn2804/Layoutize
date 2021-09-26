using System;
using System.Diagnostics;
using System.IO;
using System.Management.Automation;

namespace SchemataPreview
{
	public abstract partial class Model
	{
		public abstract ModelSet? Children { get; }
		public abstract bool Exists { get; }

		public string Name => _name;
		public Model? Parent => _parent;
		public bool PassThru => _passThru;
		public PipeAssembly PipeAssembly { get; }
		public PipelineTraversalOption Traversal => _traversal;
		public string FullName => Path.Combine(Parent?.FullName ?? (string)Schema["Path"], Name);
		public string RelativeName => Path.Combine(Parent?.RelativeName ?? string.Empty, Name);

		public static implicit operator string(Model value)
		{
			return value.FullName;
		}

		public Action CopyClosureTo(ScriptBlock script, params object[] args)
		{
			script = script.GetNewClosure();
			return () => script.InvokeWithContext(null, new()
			{
				{ new PSVariable("this", this) },
				{ new PSVariable("_", Schema) }
			}, args);
		}

		public override string ToString()
		{
			return FullName;
		}

		protected Model(ImmutableSchema schema)
		{
			Schema = schema;
			PipeAssembly = new(this);
			_name = new(this);
			_parent = new(this);
			_passThru = new(this);
			_traversal = new(this);
		}

		protected ImmutableSchema Schema { get; }
		private readonly NameProperty _name;
		private readonly ParentProperty _parent;
		private readonly PassThruProperty _passThru;
		private readonly TraversalProperty _traversal;
	}

	public abstract partial class Model
	{
		public abstract class DefaultProperty<T> : Property<T> where T : notnull
		{
			public override T Value { get; }

			public static implicit operator T(DefaultProperty<T> property)
			{
				return property.Value;
			}

			protected DefaultProperty(Model model, string key, Func<T> defaultValue)
							: base(model, key)
			{
				if (TryGetValue(out T? value))
				{
					Debug.Assert(value is not null and not PSObject);
					Value = value;
				}
				else
				{
					Value = defaultValue.Invoke();
				}
			}
		}

		public class NameProperty : RequiredProperty<string>
		{
			public NameProperty(Model model)
				: base(model, "Name")
			{
			}

			protected override bool TryGetValue(out string? result)
			{
				if (Schema.TryGetValue(Key, out object? @object) && !string.IsNullOrWhiteSpace(@object.ToString()))
				{
					Debug.Assert(@object is not null and not PSObject);
					result = @object.ToString();
					return true;
				}
				result = default;
				return false;
			}
		}

		public abstract class NullableProperty<T> : Property<T?>
		{
			public override T? Value { get; }

			protected NullableProperty(Model model, string key)
				: base(model, key)
			{
				if (TryGetValue(out T? value))
				{
					Debug.Assert(value is not null and not PSObject);
					Value = value;
				}
			}
		}

		public class ParentProperty : NullableProperty<Model?>
		{
			public ParentProperty(Model model)
				: base(model, "Parent")
			{
			}
		}

		public class PassThruProperty : DefaultProperty<bool>
		{
			public PassThruProperty(Model model)
				: base(model, "PassThru", () => false)
			{
			}
		}

		public abstract class Property<T>
		{
			public string Key { get; }
			public abstract T? Value { get; }

			public static implicit operator T?(Property<T> property)
			{
				return property.Value;
			}

			protected Property(Model model, string key)
			{
				Model = model;
				Schema = Model.Schema;
				Key = key;
			}

			protected Model Model { get; }
			protected ImmutableSchema Schema { get; }

			protected virtual bool TryGetValue(out T? result)
			{
				if (Schema.TryGetValue(Key, out object? @object))
				{
					Debug.Assert(@object is not null and not PSObject);
					result = @object is T t ? t : throw new InvalidCastException($"Unable to cast schema property '{Key}' of type '{@object.GetType()}' to type '{typeof(T)}'.");
					return true;
				}
				result = default;
				return false;
			}
		}

		public abstract class RequiredProperty<T> : Property<T> where T : notnull
		{
			public override T Value { get; }

			public static implicit operator T(RequiredProperty<T> property)
			{
				return property.Value;
			}

			protected RequiredProperty(Model model, string key)
				: base(model, key)
			{
				if (!TryGetValue(out T? value))
				{
					throw new ArgumentNullException(Key);
				}
				Debug.Assert(value is not null and not PSObject);
				Value = value;
			}
		}

		public class TraversalProperty : DefaultProperty<PipelineTraversalOption>
		{
			public TraversalProperty(Model model)
				: base(model, "Traversal", () => PipelineTraversalOption.PreOrder)
			{
			}
		}
	}

	public abstract partial class Model : IComparable<Model>
	{
		public int CompareTo(Model? other)
		{
			if (other is not null)
			{
				Schema.TryGetValue("Priority", out object? lhs_priority);
				other.Schema.TryGetValue("Priority", out object? rhs_priority);
				return ((int?)rhs_priority)?.CompareTo(lhs_priority) ?? Name.CompareTo(other.Name);
			}
			return 1;
		}
	}
}
