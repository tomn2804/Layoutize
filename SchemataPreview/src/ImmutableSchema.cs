﻿using System.Collections.Immutable;

namespace SchemataPreview
{
	public class ImmutableSchema : DynamicImmutableDictionary<ImmutableDictionary<string, object>>
	{
		public ImmutableSchema(ImmutableDictionary<string, object> dictionary)
			: base(dictionary)
		{
		}
	}
}