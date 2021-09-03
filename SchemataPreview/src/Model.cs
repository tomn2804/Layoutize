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
		public string FullName => Path.Combine(Schema["Path"] ?? string.Empty, RelativeName);
		public string Name => Schema.Name;
		public string RelativeName => Path.Combine(Parent?.RelativeName ?? string.Empty, Name);

		public static implicit operator string(Model rhs)
		{
			return rhs.FullName;
		}
	}

	public abstract partial class Model
	{
		public abstract ModelSet? Children { get; }
		public abstract bool Exists { get; }
		public virtual Model? Parent { get; internal set; }

		public virtual dynamic Schema
		{
			get
			{
				Debug.Assert(_schema != null);
				return _schema;
			}
			internal set => _schema = value;
		}

		private ReadOnlySchema? _schema;
	}

	public abstract partial class Model
	{
		public bool InvokeEvent(EventOption option)
		{
			string? name = Enum.GetName(option);
			Debug.Assert(name != null);
			return InvokeEvent(name);
		}

		public bool InvokeEvent(string name)
		{
			return Schema[name]?.InvokeWithContext(null, new List<PSVariable>() { new PSVariable("this", Schema), new PSVariable("_", this) }) != null;
		}

		public bool InvokeMethod(MethodOption option)
		{
			string? name = Enum.GetName(option);
			Debug.Assert(name != null);
			return InvokeMethod(name);
		}

		public virtual bool InvokeMethod(string name)
		{
			MethodInfo? method = GetType().GetMethod(name);
			return method?.Invoke(this, null) != null;
		}
	}

	public abstract partial class Model
	{
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
