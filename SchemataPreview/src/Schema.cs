using System;
using System.Collections;
using System.IO;

namespace SchemataPreview
{
	public abstract class Schema : DynamicHashtable
	{
		protected Schema()
		{
		}

		protected Schema(Hashtable hashtable)
			: base(hashtable)
		{
		}

		public ReadOnlySchema AsReadOnly()
		{
			return new(this);
		}

		public abstract Model Build();

		public abstract Model Build(string path);

		public abstract Model NewModel();
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
			T model = NewModel();
			ModelBuilder.HandleMount(model);
			return model;
		}

		public override T Build(string path)
		{
			this["Path"] = path;
			return Build();
		}

		public override T NewModel()
		{
			Validate();
			T model = new();
			model.Schema = AsReadOnly();
			return model;
		}

		private void Validate()
		{
			if ((this["Name"] is not string name) || (name.IndexOfAny(Path.GetInvalidFileNameChars()) != -1))
			{
				throw new InvalidOperationException();
			}
		}
	}
}
