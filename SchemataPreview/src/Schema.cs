using System;
using System.Collections;
using System.Dynamic;
using System.Management.Automation;

namespace SchemataPreview
{
	public abstract class Schema : DynamicHashtable
	{
		public ReadOnlySchema AsReadOnly()
		{
			return new(this);
		}

		public abstract Model Build();

		public abstract Model Build(string path);

		public abstract Model GetNewModel();

		public override bool TryGetMember(GetMemberBinder binder, out object? result)
		{
			bool hasResult = base.TryGetMember(binder, out result);
			if (!hasResult)
			{
				CurrentRunspace.WriteWarning($"Property '{binder.Name}' is uninitialized.");
			}
			return hasResult;
		}

		public override bool TrySetMember(SetMemberBinder binder, object? value)
		{
			bool hasResult = base.TrySetMember(binder, value);
			if (!hasResult)
			{
				CurrentRunspace.WriteWarning($"Property '{binder.Name}' is uninitialized.");
			}
			return hasResult;
		}

		protected Schema()
		{
		}

		protected Schema(Hashtable hashtable)
		{
			foreach (DictionaryEntry entry in hashtable)
			{
				Add((string)entry.Key, entry.Value is PSObject obj ? obj.BaseObject : entry.Value);
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
			T model = GetNewModel();
			(new Pipeline(model)).Invoke(PipelineOption.Mount);
			return model;
		}

		public override T Build(string path)
		{
			this["Path"] = path;
			return Build();
		}

		public override T GetNewModel()
		{
			T? model = (T?)Activator.CreateInstance(typeof(T), AsReadOnly());
			if (model == null)
			{
				throw new MissingMethodException();
			}
			return model;
		}
	}
}
