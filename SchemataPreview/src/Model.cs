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

		public string Name { get; }

#nullable enable

		public string? FullName
		{
			get => FullName;
			set
			{
				if (IsMounted)
				{
					Controller.Dismount(this);
				}
				FullName = value;
				Children.ForEach(child => child.FullName = string.IsNullOrWhiteSpace(FullName) ? null : Path.Combine(FullName, child.Name));
			}
		}

		public Model? Parent
		{
			get => Parent;
			private set
			{
				if (IsMounted)
				{
					Controller.Dismount(this);
				}
				Parent = value;
				if (Parent != null && Parent.FullName != null)
				{
					FullName = Path.Combine(Parent.FullName, Name);
				}
			}
		}

#nullable disable

		public List<Model> Children { get; }

		public bool IsMounted { get; internal set; }
		public abstract bool Exists { get; }

		public virtual void Configure()
		{
			OnMount(() =>
			{
				IsMounted = true;
			});
			OnDismount(() =>
			{
				FullName = null;
				Parent = null;
				IsMounted = false;
			});
		}

		public Model UseChildren(params Model[] models)
		{
			foreach (Model model in models)
			{
				model.Parent = this;
				Children.RemoveAll(child => child.Name == model.Name);
				Children.Add(model);
				if (IsMounted)
				{
					Controller.Mount(model);
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

		public void RemoveChild(string name)
		{
			Children.RemoveAll(child => child.Name == name);
		}
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

	public abstract partial class Model
	{
		internal List<Action> DismountActions { get; private set; }

		public Model OnDismount(Action action)
		{
			DismountActions.Add(action);
			return this;
		}

		public Model OnDismount(ScriptBlock command)
		{
			DismountActions.Add(() => command.Invoke());
			return this;
		}
	}
}
