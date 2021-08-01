using System.Collections;
using System.Collections.Generic;
using System.Data;
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
			return Schema.TryGetValue(binder.Name, out result);
		}

		public override bool TrySetMember(SetMemberBinder binder, object value)
		{
			throw new ReadOnlyException();
		}
	}

	public partial class ReadOnlySchema : IReadOnlyDictionary<object, object>
	{
		public IEnumerable<object> Keys => Schema.Keys;

		public IEnumerable<object> Values => Schema.Values;

		public int Count => Schema.Count;

		public object this[object key] => Schema[key];

		public bool ContainsKey(object key)
		{
			return Schema.ContainsKey(key);
		}

		public bool TryGetValue(object key, out object value)
		{
			return Schema.TryGetValue(key, out value);
		}

		public IEnumerator<KeyValuePair<object, object>> GetEnumerator()
		{
			return Schema.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return Schema.GetEnumerator();
		}
	}
}
