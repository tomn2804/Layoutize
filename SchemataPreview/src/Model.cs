#nullable enable

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
			get => _schema ?? throw new InvalidOperationException();
			internal set => _schema = value ?? throw new InvalidOperationException();
		}

		public abstract bool Exists { get; }

		public virtual void InvokeMethod(string name)
		{
			GetType().GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic)?.Invoke(this, null);
		}

		public virtual void Create()
		{
		}

		public virtual void Delete()
		{
		}

		public virtual void Format()
		{
		}

		public virtual void Update()
		{
		}
	}

	public abstract partial class Model
	{
		public string Name => Schema.Name;

		public string RelativePath => Path.Combine(Parent?.RelativePath ?? string.Empty, Name);

		public string AbsolutePath
		{
			get
			{
				string result = Path.Combine(Schema["Path"] ?? string.Empty, RelativePath);
				return Path.IsPathFullyQualified(result) ? result : throw new InvalidOperationException();
			}
		}

		public static implicit operator string(Model rhs)
		{
			return rhs.AbsolutePath;
		}
	}

	public abstract partial class Model
	{
		public virtual Model? Parent { get; internal set; }
		public abstract ModelSet? Children { get; }

		public virtual void Mount()
		{
			Debug.Assert(Schema is ReadOnlySchema);
			Debug.Assert(!string.IsNullOrWhiteSpace(AbsolutePath));
			Debug.Assert(Path.IsPathFullyQualified(AbsolutePath));
			Debug.Assert(AbsolutePath.IndexOfAny(Path.GetInvalidPathChars()) == -1);
			if (Exists)
			{
				if (Schema["UseHardMount"] is bool useHardMount && useHardMount)
				{
					InvokeHandlers((Action)(() => InvokeMethod("Delete")), Schema["OnDeleted"]);
					InvokeHandlers((Action)(() => InvokeMethod("Create")), Schema["OnCreated"]);
				}
			}
			else
			{
				InvokeHandlers((Action)(() => InvokeMethod("Create")), Schema["OnCreated"]);
			}
		}

		public void InvokeHandlers(params object?[] callbacks)
		{
			foreach (object? callback in callbacks)
			{
				switch (callback)
				{
					case ScriptBlock scriptBlock:
						scriptBlock.InvokeWithContext(null, new List<PSVariable>() { new PSVariable("this", Schema), new PSVariable("_", this) });
						break;

					case null:
						break;

					default:
						((Action)callback).Invoke();
						break;
				}
			}
		}
	}

	public abstract class Model<T> : Model where T : Model, new()
	{
		public override bool Exists => BaseModel.Exists;
		public override dynamic Schema { get => BaseModel.Schema; internal set => BaseModel.Schema = value; }

		public override ModelSet? Children => BaseModel.Children;
		public override Model? Parent { get => BaseModel.Parent; internal set => BaseModel.Parent = value; }

		protected T BaseModel { get; } = new();

		public override void InvokeMethod(string name)
		{
			BaseModel.InvokeMethod(name);
			base.InvokeMethod(name);
		}
	}
}

#nullable disable
