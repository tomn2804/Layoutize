#nullable enable

using System;
using System.Collections;

namespace SchemataPreview
{
	public abstract class Schema : DynamicHashtable
	{
		public Schema()
		{
		}

		public Schema(Hashtable hashtable)
			: base(hashtable)
		{
		}

		public ReadOnlySchema AsReadOnly()
		{
			return new(this);
		}

		public abstract Model Build();

		public abstract Model Build(string path);

		public abstract Model BuildTo(Model parent);
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
			return BuildTo(null);
		}

		public override T Build(string path)
		{
			this["Path"] = path;
			return BuildTo(null);
		}

		public override T BuildTo(Model? parent)
		{
			if (parent == null && this["Path"] == null)
			{
				throw new InvalidOperationException();
			}
			T model = new();
			model.Parent = parent;
			model.Schema = AsReadOnly();
			model.InvokeHandlers(
				(Action)model.Mount,
				(Action)(() =>
				{
					if (this["Children"] != null && model.Children != null)
					{
						foreach (Schema child in (Schema[])this["Children"])
						{
							model.Children.Add(child.BuildTo(model));
						}
					}
				}),
				this["OnMounted"]
			);
			return model;
		}
	}
}

#nullable disable
