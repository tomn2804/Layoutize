using System.Collections.Immutable;

namespace SchemataPreview
{
	public class ImmutableSchema : DynamicImmutableDictionary
	{
		internal ImmutableSchema(IImmutableDictionary<string, object> dictionary)
			: base(dictionary)
		{
		}
	}
}
