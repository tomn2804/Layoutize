using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace SchemataPreview
{
	public abstract class DefaultProperty<T> : Property<T> where T : notnull
	{
		[AllowNull]
		[DisallowNull]
		[MemberNotNull(nameof(_value))]
		public override T Value
		{
			get => _value.AssertNotNull();
			init => _value = value ?? _getDefaultValue();
		}

		protected DefaultProperty(ImmutableSchema schema, Func<T> getDefaultValue) : base(schema)
		{
			Value = TryGetValue(out T? value) ? value.AssertNotNull() : getDefaultValue();
			_getDefaultValue = getDefaultValue;
		}

		protected DefaultProperty(DefaultProperty<T> other)
			: base(other)
		{
			Value = other.Value;
			_getDefaultValue = other._getDefaultValue;
		}

		private Func<T> _getDefaultValue;
		private T _value;
	}

	public abstract class NullableProperty<T> : Property<T?>
	{
		public override T? Value { get; init; }

		protected NullableProperty(ImmutableSchema schema) : base(schema)
		{
			Value = TryGetValue(out T? result) ? result.AssertNotNull() : default;
		}

		protected NullableProperty(NullableProperty<T> other)
			: base(other)
		{
			Value = other.Value;
		}
	}

	public abstract class Property<T>
	{
		public abstract string Key { get; }
		public abstract T Value { get; init; }

		public static implicit operator T(Property<T> @this)
		{
			return @this.Value;
		}

		protected Property(ImmutableSchema schema)
		{
			Schema = schema;
		}

		protected Property(Property<T> other)
		{
			Schema = other.Schema;
		}

		protected ImmutableSchema Schema { get; }

		protected virtual bool TryGetValue(out T? value)
		{
			if (Schema.TryGetValue(Key, out object? result))
			{
				value = result is T t ? t : throw new ArgumentException($"Schema property value at key {Key} must be of type '{typeof(T)}'. Recieved type: '{result.GetType()}'.", Key);
				return true;
			}
			value = default;
			return false;
		}
	}

	public abstract class RequiredProperty<T> : Property<T> where T : notnull
	{
		[AllowNull]
		[MemberNotNull(nameof(_value))]
		public override T Value
		{
			get => _value;
			init => _value = value ?? throw new ArgumentNullException(Key);
		}

		protected RequiredProperty(ImmutableSchema schema)
			: base(schema)
		{
			TryGetValue(out T? result);
			Value = result;
		}

		protected RequiredProperty(RequiredProperty<T> other)
			: base(other)
		{
			Value = other.Value;
		}

		private T _value;
	}
}

namespace SchemataPreview
{
	public class FileNameProperty : NameProperty
	{
		public FileNameProperty(ImmutableSchema schema)
			: base(schema)
		{
			Validate();
		}

		public FileNameProperty(FileNameProperty other)
			: base(other)
		{
			Validate();
		}

		protected void Validate()
		{
			if (Value.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
			{
				throw new ArgumentException($"Schema '{Key}' property value cannot contains invalid characters. Recieved value: '{Value}'", Key);
			}
		}
	}

	public class NameProperty : RequiredProperty<string>
	{
		public NameProperty(ImmutableSchema schema)
			: base(schema)
		{
		}

		public NameProperty(NameProperty other)
			: base(other)
		{
		}

		public override string Key => "Name";

		protected override bool TryGetValue(out string? value)
		{
			if (Schema.TryGetValue(Key, out object? result) && !string.IsNullOrWhiteSpace(result.ToString()))
			{
				value = result.AssertNotNull().ToString();
				return true;
			}
			value = default;
			return false;
		}
	}

	public class ParentProperty : NullableProperty<Model?>
	{
		public ParentProperty(ImmutableSchema schema)
			: base(schema)
		{
		}

		public ParentProperty(ParentProperty other)
			: base(other)
		{
		}

		public override string Key => "Parent";
	}

	public class PassThruProperty : DefaultProperty<bool>
	{
		public PassThruProperty(ImmutableSchema schema)
			: base(schema, () => false)
		{
		}

		public PassThruProperty(PassThruProperty other)
			: base(other)
		{
		}

		public override string Key => "PassThru";
	}

	public class PathProperty : NullableProperty<string>
	{
		public PathProperty(ImmutableSchema schema)
			: base(schema)
		{
		}

		public PathProperty(PathProperty other)
			: base(other)
		{
		}

		public override string Key => "Path";
	}

	public class PriorityProperty : NullableProperty<int>
	{
		public PriorityProperty(ImmutableSchema schema)
			: base(schema)
		{
		}

		public PriorityProperty(PriorityProperty other)
			: base(other)
		{
		}

		public override string Key => "Priority";
	}

	public class TraversalProperty : DefaultProperty<PipelineTraversalOption>
	{
		public TraversalProperty(ImmutableSchema schema)
			: base(schema, () => PipelineTraversalOption.PreOrder)
		{
		}

		public TraversalProperty(TraversalProperty other)
			: base(other)
		{
		}

		public override string Key => "Traversal";
	}
}
