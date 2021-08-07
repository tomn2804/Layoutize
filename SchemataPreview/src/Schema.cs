using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;

namespace SchemataPreview
{
	public abstract class Schema : DynamicHashtable
	{
		public Schema()
			: base()
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

		public abstract Model Mount();

		public abstract Model Mount(string path);

		public abstract Model Mount(Model parent);
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

		public override T Mount()
		{
			return Mount(null);
		}

		public override T Mount(string path)
		{
			this["Path"] = path;
			return Mount(null);
		}

		public override T Mount(Model? parent)
		{
			T model = new();
			model.Schema = this;
			model.Parent = parent;
			model.Build();
			if (model.Exists)
			{
				if (this["UseHardMount"] is bool useHardMount && useHardMount)
				{
					model.InvokeEvent("Delete", "OnDeleted");
					model.InvokeEvent("Create", "OnCreated");
				}
			}
			else
			{
				model.InvokeEvent("Create", "OnCreated");
			}
			if (this["Children"] != null && model.Children != null)
			{
				foreach (Schema child in (List<Schema>)this["Children"])
				{
					model.Children.Add(child.Mount(model));
				}
			}
			((ScriptBlock)this["OnMounted"])?.InvokeWithContext(null, new List<PSVariable>() { new PSVariable("this", this), new PSVariable("_", model) });
			return model;
		}
	}
}
