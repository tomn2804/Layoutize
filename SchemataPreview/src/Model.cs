using System.Collections.Generic;
using System.Management.Automation;

namespace SchemataPreview.Models
{
	public abstract partial class Model
	{
		public string Name { get; internal set; }
		public string FullName { get; internal set; }

		public Model Parent { get; internal set; }
		public List<Model> Children { get; internal set; }

		public bool IsMounted { get; internal set; }

		public Model(string name)
		{
			Name = name;
			Children = new List<Model>();
		}

		public Model UseChildren(params Model[] models)
		{
			foreach (Model model in models)
			{
				ControllerHandler.Dismount(model);
				Children.RemoveAll(child => child.Name == model.Name);
				Children.Add(model);
			}
			return this;
		}

		public Model? SelectChild(string name) => Children.Find(child => child.Name == name);

		public abstract bool Exists();
	}

	public abstract partial class Model
	{
		public bool ShouldHardMount { get; internal set; }

		public Model UseHardMount()
		{
			ShouldHardMount = true;
			return this;
		}
	}

	public abstract partial class Model
	{
		public virtual void ModelDidMount()
		{
		}

		public virtual void ModelWillDismount()
		{
		}
	}

	public abstract partial class Model
	{
		public ScriptBlock CreateHandler { get; internal set; }

		public Model OnCreate(ScriptBlock command)
		{
			CreateHandler = command;
			return this;
		}

		public virtual void Create()
		{
		}
	}

	public abstract partial class Model
	{
		public ScriptBlock DeleteHandler { get; internal set; }

		public Model OnDelete(ScriptBlock command)
		{
			DeleteHandler = command;
			return this;
		}

		public virtual void Delete()
		{
		}
	}

	public abstract partial class Model
	{
		public ScriptBlock UpdateHandler { get; internal set; }

		public Model OnUpdate(ScriptBlock command)
		{
			UpdateHandler = command;
			return this;
		}

		public virtual void Update()
		{
		}
	}

	public abstract partial class Model
	{
		public ScriptBlock CleanupHandler { get; internal set; }

		public Model OnCleanup(ScriptBlock command)
		{
			CleanupHandler = command;
			return this;
		}

		public virtual void Cleanup()
		{
		}
	}
}
