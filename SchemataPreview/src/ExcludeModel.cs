﻿using System.IO;

namespace SchemataPreview
{
	public class ExcludeModel : Model
	{
		public ExcludeModel(ReadOnlySchema schema)
			: base(schema)
		{
		}

		public override ModelSet? Children => null;
		public override bool Exists => Directory.GetFiles(Parent ?? Schema["Path"], Name).Length != 0;
	}
}
