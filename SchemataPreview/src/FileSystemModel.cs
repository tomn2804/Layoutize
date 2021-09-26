using System;
using System.Diagnostics;
using System.IO;

namespace SchemataPreview
{
	public abstract class FileSystemModel : Model
	{
		public FileSystemModel(ImmutableSchema schema)
			: base(schema)
		{
			Validate();
			PipeAssembly.Register(PipeOption.Create);
			PipeAssembly.Register(PipeOption.Delete);
			PipeAssembly.Register(PipeOption.Mount).OnProcessing += (pipe, _) =>
			{
				if (Exists)
				{
					if (schema.TryGetValue("UseHardMount", out object? useHardMount) && (bool)useHardMount)
					{
						pipe.Extend(PipeAssembly[PipeOption.Delete]);
						pipe.Extend(PipeAssembly[PipeOption.Create]);
					}
				}
				else
				{
					pipe.Extend(PipeAssembly[PipeOption.Create]);
				}
			};
		}

		protected virtual void Validate()
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(Name));
			Debug.Assert(!string.IsNullOrWhiteSpace(FullName));
			if (Name.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
			{
				throw new InvalidOperationException($"Property 'Name' contains invalid characters. Recieved value: '{Name}'");
			}
			if (!Path.IsPathFullyQualified(FullName))
			{
				throw new InvalidOperationException($"Unable to resolve property 'FullName' to an absolute path. Recieved value: '{FullName}'");
			}
			if (FullName.IndexOfAny(Path.GetInvalidPathChars()) != -1)
			{
				throw new InvalidOperationException($"Property 'FullName' contains invalid characters. Recieved value: '{FullName}'");
			}
		}
	}
}
