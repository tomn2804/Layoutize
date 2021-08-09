using System;
using System.Collections;
using System.Collections.Generic;

#nullable enable

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

		public abstract Model AttachTo(Model parent);
	}

	public class Schema<T> : Schema where T : Model, new()
	{
		public Schema()
			: base()
		{
		}

		public Schema(Hashtable hashtable)
			: base(hashtable)
		{
		}

		public override T Build()
		{
			return AttachTo(null);
		}

		public override T Build(string path)
		{
			this["Path"] = path;
			return AttachTo(null);
		}

		public override T AttachTo(Model? parent)
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
						foreach (Schema child in (List<Schema>)this["Children"])
						{
							model.Children.Add(child.AttachTo(model));
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
