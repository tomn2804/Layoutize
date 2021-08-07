using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

#nullable enable

namespace SchemataPreview
{
	public abstract partial class Model
	{
		private dynamic? _schema;

		public dynamic Schema
		{
			get => _schema != null ? _schema : throw new InvalidOperationException();
			internal set => _schema = value;
		}

		public Model? Parent { get; internal set; }

		public abstract bool Exists { get; }
		public abstract List<Model>? Children { get; protected internal set; }
	}

	public abstract partial class Model
	{
		public string Name => Schema.Name;

		public string RelativePath => Parent != null ? Path.Combine(Parent.RelativePath, Name) : Name;

		public string AbsolutePath
		{
			get
			{
				string result = Schema["Path"] != null ? Path.Combine(Schema.Path, RelativePath) : RelativePath;
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
		public bool HasChild(string name)
		{
			return Children?.Find(child => child.Name == name) != null;
		}

		public virtual void Build()
		{
			Debug.Assert(Schema is ReadOnlySchema);
			Debug.Assert(!string.IsNullOrWhiteSpace(AbsolutePath));
			Debug.Assert(Path.IsPathFullyQualified(AbsolutePath));
			Debug.Assert(AbsolutePath.IndexOfAny(Path.GetInvalidPathChars()) == -1);
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

		public void InvokeEvent(string methodName, string handlerName)
		{
			if (GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic) is var method && method != null)
			{
				method?.Invoke(this, null);
			}
			if (Children != null)
			{
				foreach (Model child in Children)
				{
					child.InvokeEvent(methodName, handlerName);
				}
			}
			Schema[handlerName]?.Invoke(this, null);
		}
	}

	public class Model<T> : Model where T : Model, new()
	{
		public override bool Exists => BaseModel.Exists;
		public override List<Model>? Children { get => BaseModel.Children; protected internal set => BaseModel.Children = value; }

		protected T BaseModel { get; } = new();

		public override void Build()
		{
			base.Build();
			BaseModel.Schema = Schema;
			BaseModel.Parent = Parent;
			BaseModel.Children = Children;
			BaseModel.Build();
		}

		public override void Create()
		{
			BaseModel.Create();
			base.Create();
		}

		public override void Delete()
		{
			BaseModel.Delete();
			base.Delete();
		}

		public override void Format()
		{
			BaseModel.Format();
			base.Format();
		}
	}
}

#nullable disable
