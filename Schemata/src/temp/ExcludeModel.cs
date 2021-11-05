using System.IO;

namespace Schemata
{
	public class ExcludeModel : Model
	{
		public ExcludeModel(ImmutableDefinition Outline)
			: base(outline)
		{
		}

		public override ModelSet? Children => null;
		public override bool Exists => Directory.GetFiles(Parent ?? (string)Outline["Path"], Name).Length != 0;
	}
}
