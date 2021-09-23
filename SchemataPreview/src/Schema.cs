using System;
using System.Collections;
using System.Collections.Immutable;
using System.Management.Automation;

namespace SchemataPreview
{
	public abstract class Schema : DynamicDictionary<ImmutableDictionary<string, object>.Builder, object>
	{
		public abstract Model Build();

		public abstract Model Build(string path);

		public abstract Model GetNewModel();

		public ImmutableSchema ToImmutable()
		{
			return new(Dictionary.ToImmutable());
		}

		protected Schema()
			: base(ImmutableDictionary.CreateBuilder<string, object>())
		{
		}

		protected Schema(Hashtable hashtable)
			: base(ImmutableDictionary.CreateBuilder<string, object>())
		{
			foreach (DictionaryEntry entry in hashtable)
			{
				if (entry.Value != null)
				{
					Add((string)entry.Key, entry.Value is PSObject obj ? obj.BaseObject : entry.Value);
				}
			}
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
			T model = GetNewModel();
			new Pipeline(model).Invoke(PipeOption.Mount);
			return model;
		}

		public override T Build(string path)
		{
			this["Path"] = path;
			return Build();
		}

		public override T GetNewModel()
		{
			return (T)(Activator.CreateInstance(typeof(T), ToImmutable()) ?? throw new MissingMethodException());
		}
	}
}
