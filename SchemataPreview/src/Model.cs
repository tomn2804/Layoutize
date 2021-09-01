using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management.Automation;
using System.Reflection;

namespace SchemataPreview
{
	public abstract partial class Model
	{
		private ReadOnlySchema? _schema;

		public virtual dynamic Schema
		{
			get
			{
				Debug.Assert(_schema != null);
				return _schema;
			}
			internal set => _schema = value;
		}

		public abstract bool Exists { get; }

		public virtual bool InvokeMethod(string name)
		{
			MethodInfo? method = GetType().GetMethod(name);
			return method?.Invoke(this, null) != null;
		}

		public bool InvokeEvent(string name)
		{
			return Schema[name]?.InvokeWithContext(null, new List<PSVariable>() { new PSVariable("this", Schema), new PSVariable("_", this) }) != null;
		}
	}

	public abstract partial class Model
	{
		public string Name => Schema.Name;

		public string RelativeName => Path.Combine(Parent?.RelativeName ?? string.Empty, Name);

		public string FullName => Path.Combine(Schema["Path"] ?? string.Empty, RelativeName);

		public static implicit operator string(Model rhs)
		{
			return rhs.FullName;
		}
	}

	public abstract partial class Model
	{
		public virtual Model? Parent { get; internal set; }
		public abstract ModelSet? Children { get; }

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

		private void Validate()
		{
			Debug.Assert(Schema is ReadOnlySchema);
			Debug.Assert(!string.IsNullOrWhiteSpace(FullName));
			if (!Path.IsPathFullyQualified(FullName))
			{
				throw new InvalidOperationException();
			}
			if (FullName.IndexOfAny(Path.GetInvalidPathChars()) != -1)
			{
				throw new InvalidOperationException();
			}
		}
	}

	public abstract partial class Model : IEquatable<string>
	{
		public bool Equals(string? other)
		{
			return Name == other;
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
