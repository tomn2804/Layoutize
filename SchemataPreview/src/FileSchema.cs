using System;
using System.Collections;

namespace SchemataPreview
{
	public class FileSchema : Schema<FileModel>
	{
		public FileSchema(Hashtable props)
			: base(props)
		{
		}

		protected FileSchema(Hashtable props, Schema state)
			: base(props, state)
		{
		}

		protected override Schema Build()
		{
			Hashtable props = new();
			props["OnCreating"] = (Action<FileModel>)((model) =>
			{
				model.Create();
				((Action<Model>?)Props["OnCreating"])?.Invoke(model);
			});
			props["OnDeleting"] = (Action<FileModel>)((model) =>
			{
				model.Create();
				((Action<Model>?)Props["OnDeleting"])?.Invoke(model);
			});
			return new FileSchema(props, this);
		}
	}
}
