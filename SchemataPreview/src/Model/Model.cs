using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Linq;
using System.Diagnostics;
using System.IO;

namespace SchemataPreview
{
	public abstract partial class Model
	{
		private string _name;

		public string Name
		{
			get => _name;
			set
			{
				if (IsMounted)
				{
					throw;
				}
				if (string.IsNullOrWhiteSpace(value) || value.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
				{
					throw;
				}
				_name = value;
			}
		}

		private bool _shouldHardMount;

		public bool ShouldHardMount
		{
			get => _shouldHardMount;
			set
			{
				if (IsMounted)
				{
					throw;
				}
				_shouldHardMount = value;
			}
		}

		private List<Model> _children = new();

		public List<Model> Children
		{
			get => _children;
			set
			{
				if (IsMounted)
				{
					throw;
				}
				_children = value;
			}
		}
	}

	public abstract partial class Model
	{
		private string _fullName;

		public string FullName
		{
			get => _fullName;
			private set
			{
				Debug.Assert(!IsMounted);
				_fullName = value;
			}
		}

		public bool IsMounted { get; private set; }

		public abstract bool Exists { get; }

		protected abstract void Build(Builder builder);
	}

	public abstract partial class Model
	{
		private Dictionary<string, List<Action>> EventToCallbacks { get; set; } = new();

		public void InvokeEvent(string type)
		{
			foreach (Action callback in EventToCallbacks[type])
			{
				callback();
			}
		}
	}

	public abstract partial class Model : ICloneable
	{
		public object Clone()
		{
			Model other = (Model)MemberwiseClone();
			other.Parent = (Model)Parent?.Clone();
			other.Children = Children.Select(child => (Model)child.Clone()).ToList();
			return other;
		}
	}

#nullable enable

	public abstract partial class Model
	{
		private ScriptBlock? _psBuild;

		public ScriptBlock? PSBuild
		{
			get => _psBuild;
			set
			{
				if (IsMounted)
				{
					throw;
				}
				_psBuild = value;
			}
		}

		private Model? _parent;

		public Model? Parent
		{
			get => _parent;
			private set
			{
				Debug.Assert(!IsMounted);
				_parent = value;
			}
		}

		public Model? SelectChild(string name)
		{
			return Children.Find(child => child.Name == name);
		}
	}

#nullable disable
}
