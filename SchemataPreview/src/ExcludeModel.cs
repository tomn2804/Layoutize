using System.Collections.Generic;
using System.IO;

#nullable enable

namespace SchemataPreview
{
	public class ExcludeModel : Model
	{
		public override bool Exists => Directory.GetFiles(Parent ?? Schema.Path, Name).Length != 0;
		public override List<Model>? Children => null;
	}
}

#nullable disable
