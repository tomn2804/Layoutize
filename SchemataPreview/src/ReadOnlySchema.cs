using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;

namespace SchemataPreview
{
	public partial class ReadOnlySchema : DynamicObject
	{
		internal ReadOnlySchema(Schema schema)
			: base()
		{
			Schema = schema;
		}

		protected internal Schema Schema { get; init; }

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			return Schema.TryGetMember(binder, out result);
		}

		public override bool TrySetMember(SetMemberBinder binder, object value)
		{
			throw new ReadOnlyException();
		}
	}

	public partial class ReadOnlySchema : IReadOnlyDictionary<object, object>
	{
		public object this[object key] => Schema[key];

		public IEnumerable<object> Keys => Schema.Keys;

		public IEnumerable<object> Values => Schema.Values;

		public int Count => throw new System.NotImplementedException();

		public bool ContainsKey(object key)
		{
			throw new System.NotImplementedException();
		}

		public IEnumerator<KeyValuePair<object, object>> GetEnumerator()
		{
			throw new System.NotImplementedException();
		}

		public bool TryGetValue(object key, [MaybeNullWhen(false)] out object value)
		{
			throw new System.NotImplementedException();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new System.NotImplementedException();
		}
	}
}
