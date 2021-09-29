using System.IO;

namespace SchemataPreview
{
	public class ExcludeModel : Model
	{
		public ExcludeModel(ImmutableSchema schema)
			: base(schema)
		{
		}

		public override ModelSet? Children => null;
		public override bool Exists => Directory.GetFiles(Parent ?? (string)Schema["Path"], Name).Length != 0;
	}
}
