using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Linq;

namespace SchemataPreview
{
	public abstract partial class Model : ICloneable
	{
		public object Clone()
		{
			Model other = (Model)MemberwiseClone();
			other.Children = Children.Select(child => (Model)child.Clone()).ToList();
			other.Parent = (Model)Parent.Clone();
			return other;
		}

		public Model(string name)
		{
			Name = name;
			Children = new();
			configurationHandlers = new();
			eventHandlers = new();
			Configure(() =>
			{
				AddEventListener(EventOption.Mount, () =>
				{
					IsMounted = true;
				});
				AddEventListener(EventOption.Dismount, () =>
				{
					FullName = null;
					Parent = null;
					IsMounted = false;
				});
			});
		}

		private string name;

		public string Name
		{
			get => name;
			private set
			{
				if (value != name)
				{
					name = value;
					FullName = null;
				}
			}
		}

#nullable enable

		private string? fullName;

		public string? FullName
		{
			get => fullName;
			set
			{
				if (value != fullName)
				{
					if (IsMounted)
					{
						Controller.Dismount(this);
					}
					fullName = value;
					if (Path.GetFileName(fullName) != Name)
					{
						// throw
					}
					Children.ForEach(child => child.FullName = string.IsNullOrWhiteSpace(fullName) ? null : Path.Combine(fullName, child.Name));
				}
			}
		}

		private Model? parent;

		public Model? Parent
		{
			get => parent;
			private set
			{
				parent = value;
				FullName = parent?.FullName != null ? Path.Combine(parent.FullName, Name) : null;
			}
		}

#nullable disable

		public List<Model> Children { get; private set; }
		public bool IsMounted { get; internal set; }
		public abstract bool Exists { get; }

		public Model AddChildren(params Model[] models)
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
	}

	public abstract partial class Model
	{
		private List<Action> configurationHandlers;

		public Model Configure(ScriptBlock action)
		{
			return Configure(() => action.InvokeWithContext(null, new List<PSVariable>() { new PSVariable("this", this) }));
		}

		public Model Configure(Action action)
		{
			configurationHandlers.Add(action);
			return this;
		}
	}

	public abstract partial class Model
	{
		private Dictionary<string, List<Action>> eventHandlers;

		public void AddEventListener(string type, ScriptBlock action)
		{
			AddEventListener(type, () => action.InvokeWithContext(null, new List<PSVariable>() { new PSVariable("this", this) }));
		}

		public void AddEventListener(string type, Action action)
		{
			List<Action> actions;
			if (eventHandlers.TryGetValue(type, out actions))
			{
				actions.Add(action);
			}
			else
			{
				eventHandlers.Add(type, new List<Action>() { action });
			}
		}
	}

	public abstract partial class Model
	{
		public bool ShouldHardMount { get; private set; }

		public Model UseHardMount()
		{
			ShouldHardMount = true;
			return this;
		}
	}
}
