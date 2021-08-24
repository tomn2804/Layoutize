using System.IO;

namespace SchemataPreview
{
	public class ExcludeModel : Model
	{
		public override bool Exists => Directory.GetFiles(Parent ?? Schema.Path, Name).Length != 0;
		public override ModelSet? Children => null;
	}
}
