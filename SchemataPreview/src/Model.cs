using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management.Automation;

#nullable enable

namespace SchemataPreview
{
	public abstract partial class Model : IEquatable<Model>
	{
		public bool Equals(Model? other)
		{
			return Name == other?.Name;
		}
	}

	public abstract partial class Model : IComparable<Model>
	{
		public int CompareTo(Model? other)
		{
			return Name.CompareTo(other?.Name);
		}
	}

	public abstract partial class Model
	{
		public static operator Model(string name)
		{
			Model result = new();
			result.Name = name;
			return result;
		}

		internal Model()
		{
		}

		private dynamic? _schema;

		public virtual dynamic Schema
		{
			get => _schema ?? throw new InvalidOperationException();
			internal set => _schema = value;
		}

		public abstract bool Exists { get; }
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
					InvokeHandlers((Action)Delete, Schema["OnDeleted"]);
					InvokeHandlers((Action)Create, Schema["OnCreated"]);
				}
			}
			else
			{
				InvokeHandlers((Action)Create, Schema["OnCreated"]);
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
		public virtual Model? Parent { get; internal set; }
		public abstract ModelSet? Children { get; }

		public bool HasChild(string name)
		{
			return Children?.Find(child => child.Name == name) != null;
		}
	}

	public class Model<T> : Model where T : Model, new()
	{
		public override bool Exists => BaseModel.Exists;
		public override dynamic Schema { get => BaseModel.Schema; internal set => BaseModel.Schema = value; }

		public override List<Model>? Children => BaseModel.Children;
		public override Model? Parent { get => BaseModel.Parent; internal set => BaseModel.Parent = value; }

		protected T BaseModel { get; } = new();

		public override void InternalMount()
		{
			BaseModel.Mount();
			Mount();
		}

		public override void InternalCreate()
		{
			BaseModel.Create();
			Create();
		}

		public override void InternalDelete()
		{
			BaseModel.Delete();
			Delete();
		}

		public override void InternalFormat()
		{
			BaseModel.Format();
			Format();
		}

		public override void InternalUpdate()
		{
			BaseModel.Update();
			Update();
		}
	}
}

#nullable disable
