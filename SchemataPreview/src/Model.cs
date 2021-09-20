using System;
using System.Diagnostics;
using System.IO;

namespace SchemataPreview
{
	public abstract partial class Model
	{
		public Model(ReadOnlySchema schema)
		{
			_schema = schema;
			Parent = Schema["Parent"];
			PipeAssembly[PipelineOption.Mount] += (stack) =>
			{
				if (Exists)
				{
					if (Schema["UseHardMount"] is bool useHardMount && useHardMount)
					{
						stack.Push(PipeAssembly[PipelineOption.Delete].Handler);
						stack.Push(PipeAssembly[PipelineOption.Create].Handler);
					}
				}
				else
				{
					stack.Push(PipeAssembly[PipelineOption.Create].Handler);
				}
			});
		}

		public abstract ModelSet? Children { get; }
		public abstract bool Exists { get; }
		public Model? Parent { get; init; }
		public PipeAssembly PipeAssembly { get; } = new();
		public dynamic Schema => _schema;
		public string FullName => Path.Combine(Parent?.FullName ?? Schema.Path, Name);
		public string RelativeName => Path.Combine(Parent?.RelativeName ?? string.Empty, Name);

		public static implicit operator string(Model rhs)
		{
			return rhs.FullName;
		}

		private ReadOnlySchema _schema;
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
					EventHandler[PipelineOption.Delete].Invoke();
					EventHandler[PipelineOption.Create].Invoke();
				}
			}
			else
			{
				EventHandler[PipelineOption.Create].Invoke();
			}
		}

		protected virtual void Validate()
		{
			Debug.Assert(Schema is ReadOnlySchema);
			Debug.Assert(!string.IsNullOrWhiteSpace(FullName));
			if (Name.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
			{
				throw new InvalidOperationException($"Property 'Name' contains invalid characters. Recieved value: '{Name}'");
			}
			if (Schema["Name"] is not string)
			{
				throw new InvalidOperationException("Property 'Name' is not initialized to type [string].");
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
	}

	public partial class Model : IEquatable<string>
	{
		public string Name => Schema.Name;

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
