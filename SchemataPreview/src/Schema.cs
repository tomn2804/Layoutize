using System;
using System.Collections;
using System.Diagnostics;
using System.Management.Automation;

namespace SchemataPreview
{
	public abstract class Schema : DynamicDictionary
	{
		public ReadOnlySchema AsReadOnly()
		{
			return new(this);
		}

		public abstract Model Build();

		public abstract Model Build(string path);

		public abstract Model GetNewModel();

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
			(new Pipeline(model)).Invoke(PipelineOption.Mount);
			return model;
		}

		public override T Build(string path)
		{
			Add("Path", path);
			return Build();
		}

		public override T GetNewModel()
		{
			T? model = (T?)Activator.CreateInstance(typeof(T), AsReadOnly());
			Debug.Assert(model != null);
			return model;
		}
	}
}
