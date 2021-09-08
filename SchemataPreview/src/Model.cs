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
		public string FullName => Path.Combine(Parent?.FullName ?? Schema.Path, Name);
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
		public Model? Parent { get; private set; }

		public dynamic Schema
		{
			get
			{
				Debug.Assert(_schema is ReadOnlySchema);
				return _schema;
			}
		}

		internal void Init(ReadOnlySchema schema)
		{
			_schema = schema;
		}

		private ReadOnlySchema? _schema;
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

		protected virtual void Validate()
		{
			Debug.Assert(Schema is ReadOnlySchema);
			Debug.Assert(!string.IsNullOrWhiteSpace(FullName));
			if (!Path.IsPathFullyQualified(FullName))
			{
				throw new InvalidOperationException($"Cannot resolve property 'FullName' to an absolute path. Recieved value: '{FullName}'");
			}
			if (FullName.IndexOfAny(Path.GetInvalidPathChars()) != -1)
			{
				throw new InvalidOperationException($"Property 'FullName' contains invalid characters. Recieved value: '{FullName}'");
			}
		}
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
			switch (Schema[name])
			{
				case ScriptBlock script:
					script.GetNewClosure().InvokeWithContext(null, new List<PSVariable>() { new PSVariable("this", Schema), new PSVariable("_", this) });
					break;

				case Action action:
					action.Invoke();
					break;

				default:
					return false;
			}
			return true;
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
			if (method == null)
			{
				return false;
			}
			method.Invoke(this, null);
			return true;
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
