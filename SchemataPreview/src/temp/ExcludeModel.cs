using System.IO;

namespace SchemataPreview
{
	public class ExcludeModel : Model
	{
		public ExcludeModel(ImmutableDefinition Props)
			: base(props)
		{
		}

		public override ModelSet? Children => null;
		public override bool Exists => Directory.GetFiles(Parent ?? (string)Props["Path"], Name).Length != 0;
	}
}
