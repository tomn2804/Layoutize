using System;
using System.IO;

namespace SchemataPreview
{
	public abstract class FileSystemModel : Model
	{
		protected override void Validate()
		{
			base.Validate();
			if (Name.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
			{
				throw new InvalidOperationException($"Property 'Name' contains invalid characters. Recieved value: '{Name}'");
			}
		}
	}
}
