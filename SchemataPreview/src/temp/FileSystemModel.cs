using System;
using System.Diagnostics;
using System.IO;

namespace SchemataPreview
{
	public abstract class FileSystemModel : Model
	{
		public FileSystemModel(ImmutableDefinition Props)
			: base(props)
		{
			PipeAssembly.Register(PipeOption.Create);
			PipeAssembly.Register(PipeOption.Delete);
			PipeAssembly.Register(PipeOption.Mount).OnProcessing += (pipe, _) =>
			{
				Validate();
				if (Exists)
				{
					if (Props.TryGetValue("UseHardMount", out object? useHardMount) && (bool)useHardMount)
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
				throw new ArgumentException($"Property 'Name' contains invalid characters. Recieved value: '{Name}'");
			}
			if (!Path.IsPathFullyQualified(FullName))
			{
				throw new ArgumentException($"Cannot resolve property 'FullName' to an absolute path. Recieved value: '{FullName}'");
			}
			if (FullName.IndexOfAny(Path.GetInvalidPathChars()) != -1)
			{
				throw new ArgumentException($"Property 'FullName' contains invalid characters. Recieved value: '{FullName}'");
			}
		}
	}
}
