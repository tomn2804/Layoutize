using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management.Automation;

namespace SchemataPreview
{
	public abstract partial class Model
	{
		public abstract ModelSet? Children { get; }
		public abstract bool Exists { get; }
		public Model? Parent { get; private set; }

		public WildcardPattern Pattern
		{
			get
			{
				Debug.Assert(_pattern != null);
				return _pattern;
			}
		}

		public dynamic Schema
		{
			get
			{
				Debug.Assert(_schema is ReadOnlySchema);
				return _schema;
			}
		}

		public string FullName => Path.Combine(Parent?.FullName ?? Schema.Path, Name);
		public string RelativeName => Path.Combine(Parent?.RelativeName ?? string.Empty, Name);

		public static implicit operator string(Model rhs)
		{
			return rhs.FullName;
		}

		public virtual void Mount()
		{
			Build();
			Children?.Mount();
		}

		internal void Build()
		{
			Validate();
			if (Exists)
			{
				if (Schema["UseHardMount"] is bool useHardMount && useHardMount)
				{
					ModelBuilder.HandleDelete(this);
					ModelBuilder.HandleCreate(this);
				}
			}
			else
			{
				ModelBuilder.HandleCreate(this);
			}
		}

		internal void Init(ReadOnlySchema schema)
		{
			_schema = schema;
			_pattern = new(Schema.Name);
		}

		protected virtual void Validate()
		{
			Debug.Assert(Schema is ReadOnlySchema);
			Debug.Assert(!string.IsNullOrWhiteSpace(FullName));
			if (Schema["Name"] == null)
			{
				throw new InvalidOperationException("Property 'Name' is uninitialized.");
			}
			if (!Path.IsPathFullyQualified(FullName))
			{
				throw new InvalidOperationException($"Cannot resolve property 'FullName' to an absolute path. Recieved value: '{FullName}'");
			}
			if (FullName.IndexOfAny(Path.GetInvalidPathChars()) != -1)
			{
				throw new InvalidOperationException($"Property 'FullName' contains invalid characters. Recieved value: '{FullName}'");
			}
		}

		private WildcardPattern? _pattern;
		private ReadOnlySchema? _schema;
	}

	public abstract partial class Model : DynamicHandler
	{
		public override bool InvokeCallback(string name)
		{
			switch (Schema[name])
			{
				case ScriptBlock callback:
					callback.GetNewClosure().InvokeWithContext(null, new List<PSVariable>() { new PSVariable("this", Schema), new PSVariable("_", this) });
					break;

				case Action callback:
					callback.Invoke();
					break;

				default:
					return false;
			}
			return true;
		}
	}

	public partial class Model : IEquatable<string>
	{
		public virtual string Name => Schema.Name;

		public bool Equals(string? name)
		{
			return Name == name;
		}
	}

	public abstract partial class Model : IEquatable<Model>
	{
		public bool Equals(Model? other)
		{
			return RelativeName == other?.RelativeName;
		}
	}
}
