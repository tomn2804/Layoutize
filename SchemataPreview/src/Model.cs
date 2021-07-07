using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;

namespace SchemataPreview
{
	public abstract partial class Model
	{
		public Model(string name)
		{
			Name = name;
			Children = new();
		}

		public string Name { get; private set; }
		public string FullName { get; private set; }

		public Model Parent { get; private set; }
		public List<Model> Children { get; private set; }

		public bool IsMounted { get; internal set; }
		public abstract bool Exists { get; }

		public virtual void Configure()
		{
		}

		public virtual void Configure(string path)
		{
			FullName = Path.Combine(path, Name);
			foreach (Model child in Children)
			{
				child.Parent = this;
				Configure(FullName);
			}
		}

		public Model UseChildren(params Model[] models)
		{
			foreach (Model model in models)
			{
				if (IsMounted)
				{
					Controller.Mount(this, model);
				}
				else
				{
					Controller.Dismount(model);
					Children.RemoveAll(child => child.Name == model.Name);
					Children.Add(model);
				}
			}
			return this;
		}

#nullable enable

		public Model? SelectChild(string name)
		{
			return Children.Find(child => child.Name == name);
		}

#nullable disable
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
		internal List<Action> CreateActions { get; private set; }

		public Model OnCreate(Action action)
		{
			CreateActions.Add(action);
			return this;
		}

		public Model OnCreate(ScriptBlock command)
		{
			CreateActions.Add(() => command.Invoke());
			return this;
		}
	}

	public abstract partial class Model
	{
		internal List<Action> DeleteActions { get; private set; }

		public Model OnDelete(Action action)
		{
			DeleteActions.Add(action);
			return this;
		}

		public Model OnDelete(ScriptBlock command)
		{
			DeleteActions.Add(() => command.Invoke());
			return this;
		}
	}

	public abstract partial class Model
	{
		internal List<Action> UpdateActions { get; private set; }

		public Model OnUpdate(Action action)
		{
			UpdateActions.Add(action);
			return this;
		}

		public Model OnUpdate(ScriptBlock command)
		{
			UpdateActions.Add(() => command.Invoke());
			return this;
		}
	}

	public abstract partial class Model
	{
		internal List<Action> CleanupActions { get; private set; }

		public Model OnCleanup(Action action)
		{
			CleanupActions.Add(action);
			return this;
		}

		public Model OnCleanup(ScriptBlock command)
		{
			CleanupActions.Add(() => command.Invoke());
			return this;
		}
	}

	public abstract partial class Model
	{
		internal List<Action> MountActions { get; private set; }

		public Model OnMount(Action action)
		{
			MountActions.Add(action);
			return this;
		}

		public Model OnMount(ScriptBlock command)
		{
			MountActions.Add(() => command.Invoke());
			return this;
		}
	}
}
