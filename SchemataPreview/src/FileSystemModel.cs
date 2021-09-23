using System;
using System.Diagnostics;

namespace SchemataPreview
{
	public abstract class FileSystemModel : Model
	{
		public FileSystemModel(ImmutableSchema schema)
			: base(schema)
		{
			PipeAssembly.Register(PipelineOption.Create);
			PipeAssembly.Register(PipelineOption.Delete);
			PipeAssembly.Register(PipelineOption.Mount).OnProcessing += (segment, _) =>
			 {
				 Validate();
				 if (Exists)
				 {
					 if (schema.TryGetValue("UseHardMount") is bool useHardMount && useHardMount)
					 {
						 segment.Extend(PipeAssembly[PipelineOption.Delete]);
						 segment.Extend(PipeAssembly[PipelineOption.Create]);
					 }
				 }
				 else
				 {
					 segment.Extend(PipeAssembly[PipelineOption.Create]);
				 }
			 };
		}

		protected virtual void Validate()
		{
			//Debug.Assert(Schema is ImmutableSchema);
			//Debug.Assert(!string.IsNullOrWhiteSpace(FullName));
			//if (Name.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
			//{
			//	throw new InvalidOperationException($"Property 'Name' contains invalid characters. Recieved value: '{Name}'");
			//}
			//if (Schema["Name"] is not string)
			//{
			//	throw new InvalidOperationException("Property 'Name' is not initialized to type [string].");
			//}
			//if (!Path.IsPathFullyQualified(FullName))
			//{
			//	throw new InvalidOperationException($"Cannot resolve property 'FullName' to an absolute path. Recieved value: '{FullName}'");
			//}
			//if (FullName.IndexOfAny(Path.GetInvalidPathChars()) != -1)
			//{
			//	throw new InvalidOperationException($"Property 'FullName' contains invalid characters. Recieved value: '{FullName}'");
			//}
		}
	}
}
