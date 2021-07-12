using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Linq;

namespace SchemataPreview
{
	public abstract partial class Model
	{
		public Model(string name)
		{
			Name = name;
			EventHandler = new(this);
			ConfigurationHandler = new(this);
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

		public List<Model> Children { get; } = new();
		public bool ShouldHardMount { get; private set; }

		internal EventHandler EventHandler { get; private set; }
		internal ConfigurationHandler ConfigurationHandler { get; private set; }

		public bool IsMounted { get; internal set; }
		public abstract bool Exists { get; }

		internal HierarchyHandler HierarchyHandler { get; private set; }

		public Model? Test { get => HierarchyHandler.Parent; }

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
			protected set
			{
				parent = value;
				FullName = parent?.FullName != null ? Path.Combine(parent.FullName, Name) : null;
			}
		}

#nullable disable

		public void Mount(string path)
		{
			FullName = Path.Combine(path, Name);
			EventController.Mount(this);
		}

		public void Dismount()
		{
			EventController.Dismount(this);
		}

		public Model UseHardMount()
		{
			ShouldHardMount = true;
			return this;
		}

		public Model AddChildren(params Model[] models)
		{
			HierarchyHandler.AddChildren(models);
			return this;
		}

#nullable enable

		public Model? SelectChild(string name)
		{
			return HierarchyHandler.SelectChild(name);
		}

#nullable disable

		public void Configure(ScriptBlock callback)
		{
			ConfigurationHandler.Add(callback);
		}

		public void Configure(Action callback)
		{
			ConfigurationHandler.Add(callback);
		}

		public void AddEventListener(string type, ScriptBlock callback)
		{
			EventHandler.Add(type, callback);
		}

		public void AddEventListener(string type, Action callback)
		{
			EventHandler.Add(type, callback);
		}
	}

	public abstract partial class Model : ICloneable
	{
		public object Clone()
		{
			Model other = (Model)MemberwiseClone();
			other.Children = Children.Select(child => (Model)child.Clone()).ToList();
			other.Parent = (Model)Parent.Clone();
			return other;
		}
	}
}
