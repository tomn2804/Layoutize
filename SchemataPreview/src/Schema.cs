using System;
using System.Collections;
using System.Management.Automation;

namespace SchemataPreview
{
	public abstract class Schema : DynamicDictionary
	{
		public abstract Model Build();

		public abstract Model Build(string path);

		public abstract Model GetNewModel();

		public ImmutableSchema ToImmutable()
		{
			return new(Dictionary.ToImmutable());
		}

		protected Schema()
		{
		}

		protected Schema(Hashtable hashtable)
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

	public class Schema<T> : Schema where T : Model, new()
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
			(new Pipeline(result)).Invoke(PipelineOption.Mount);
			return result;
		}

		public override T Build(string path)
		{
			Add("Path", path);
			return Build();
		}

		public override T GetNewModel()
		{
			return (T)(Activator.CreateInstance(typeof(T), ToImmutable()) ?? throw new MissingMethodException());
		}
	}
}
