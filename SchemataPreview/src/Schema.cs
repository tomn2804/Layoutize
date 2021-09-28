using System;
using System.Collections;

namespace SchemataPreview
{
	public abstract partial class Schema : Hashtable
	{
		public abstract Model Build();

		public abstract Model Build(string path);

		public abstract Model GetNewModel();

		public ImmutableSchema ToImmutable()
		{
			return new(this);
		}

		protected Schema()
		{
		}

		protected Schema(Hashtable hashtable)
			: base(hashtable)
		{
		}
	}

	public class Schema<T> : Schema where T : Model
	{
		public Schema()
		{
		}

		public Schema(Hashtable hashtable)
			: base(hashtable)
		{
		}

		public override T Build()
		{
			T result = GetNewModel();
			new Pipeline(result).Invoke(PipeOption.Mount);
			return result;
		}

		public override T Build(string path)
		{
			this["Path"] = path;
			return Build();
		}

		public override T GetNewModel()
		{
			return (T)Activator.CreateInstance(typeof(T), ToImmutable()).AssertNotNull();
		}
	}
}
