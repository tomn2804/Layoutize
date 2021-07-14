using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Linq;

namespace SchemataPreview
{
	public abstract partial class Model
	{
		public Model()
		{
			ConfigurationHandler = new(this);
			EventHandler = new(this);
			HierarchyHandler = new(this);
		}

		public string FullName { get; internal set; }
		public string Name { get; private set; }

		public bool IsMounted { get; internal set; }
		public bool ShouldHardMount { get; private set; }
		public List<Model> Children { get => HierarchyHandler.Children; }

		private ConfigurationHandler ConfigurationHandler { get; set; }
		private EventHandler EventHandler { get; set; }
		private HierarchyHandler HierarchyHandler { get; set; }

		public abstract bool Exists { get; }

		public virtual void PresetConfiguration()
		{
		}

		public void InvokeEvent(string type)
		{
			EventHandler.Invoke(type);
		}

		public Model AddChildren(params Model[] models)
		{
			HierarchyHandler.AddChildren(models);
			return this;
		}

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

#nullable enable

	public abstract partial class Model
	{
		public Model? Parent { get => HierarchyHandler.Parent; internal set => HierarchyHandler.Parent = value; }

		public Model? SelectChild(string name)
		{
			return HierarchyHandler.SelectChild(name);
		}
	}

#nullable disable

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
