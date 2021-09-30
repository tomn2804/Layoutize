using System;
using System.IO;

namespace SchemataPreview
{
	public abstract class DefaultProperty<T> : Property<T> where T : notnull
	{
		public override T Value { get; }

		protected DefaultProperty(ImmutableSchema schema)
			: base(schema)
		{
			Value = TryGetValue(out T? value) ? value.AssertNotNull() : DefaultValue;
		}

		protected abstract T DefaultValue { get; }
	}

	public abstract class NullableProperty<T> : Property<T?>
	{
		public override T? Value { get; }

		protected NullableProperty(ImmutableSchema schema)
			: base(schema)
		{
			Value = TryGetValue(out T? result) ? result.AssertNotNull() : default;
		}
	}

	public abstract class Property<T>
	{
		public abstract string Key { get; }
		public abstract T Value { get; }

		public static implicit operator T(Property<T> @this)
		{
			return @this.Value;
		}

		protected Property(ImmutableSchema schema)
		{
			Schema = schema;
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
		public override T Value { get; }

		protected RequiredProperty(ImmutableSchema schema)
			: base(schema)
		{
			TryGetValue(out T? result);
			Value = result ?? throw new ArgumentNullException(Key);
		}
	}
}

namespace SchemataPreview
{
	public class FileNameProperty : NameProperty
	{
		public FileNameProperty(ImmutableSchema schema)
			: base(schema)
		{
		}

		protected override bool TryGetValue(out string? value)
		{
			bool hasResult = base.TryGetValue(out value);
			if (value?.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
			{
				throw new ArgumentException($"Schema '{Key}' property value cannot contains invalid characters. Recieved value: '{Value}'", Key);
			}
			return hasResult;
		}
	}

	public class NameProperty : RequiredProperty<string>
	{
		public NameProperty(ImmutableSchema schema)
			: base(schema)
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

		public override string Key => "Parent";
	}

	public class PassThruProperty : DefaultProperty<bool>
	{
		public PassThruProperty(ImmutableSchema schema)
			: base(schema)
		{
		}

		public override string Key => "PassThru";
		protected override bool DefaultValue => false;
	}

	public class PathProperty : NullableProperty<string>
	{
		public PathProperty(ImmutableSchema schema)
			: base(schema)
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

		public override string Key => "Priority";
	}

	public class TraversalProperty : DefaultProperty<PipelineTraversalOption>
	{
		public TraversalProperty(ImmutableSchema schema)
			: base(schema)
		{
		}

		public override string Key => "Traversal";
		protected override PipelineTraversalOption DefaultValue => PipelineTraversalOption.PreOrder;
	}
}
