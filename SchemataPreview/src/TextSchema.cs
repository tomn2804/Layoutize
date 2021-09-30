using System;
using System.Collections;

namespace SchemataPreview
{
	public class TextSchema : Schema<TextModel>
	{
		public TextSchema(Hashtable props)
			: base(props)
		{
		}

		protected override Schema Build()
		{
			Hashtable props = new();
			switch (props["Contents"])
			{
				case string line:
					props["OnCreated"] = (Action<TextModel>)((model) =>
					{
						model.Contents = new string[] { line };
						((Action<Model>?)Props["OnCreated"])?.Invoke(model);
					});
					break;

				case string[] lines:
					props["OnCreated"] = (Action<TextModel>)((model) =>
					{
						model.Contents = lines;
						((Action<Model>?)Props["OnCreated"])?.Invoke(model);
					});
					break;
			}
			return new FileSchema(props);
		}
	}
}
