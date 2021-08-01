using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Linq;
using System.IO;
using System.Dynamic;
using System.Collections;

namespace SchemataPreview
{
	public class Schema : Hashtable
	{
		public Schema(Hashtable hashtable)
			: base(hashtable)
		{
		}
	}

	public class Schema<T> : Schema
	{
		public Schema(Hashtable hashtable)
			: base(hashtable)
		{
		}
	}

	public class DyanmicDictionary : DynamicObject
	{
		protected Dictionary<string, object> Dictionary { get; } = new();

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			Dictionary.TryGetValue(binder.Name, out result);
			return true;
		}

		public override bool TrySetMember(SetMemberBinder binder, object value)
		{
			Dictionary[binder.Name] = value;
			return true;
		}
	}

	public class Schema3 : DyanmicDictionary, ISchema where T : Model, new()
	{
		public Schema3()
		{
			Dictionary["Model"] = new T();
		}

		public void Build(string path)
		{
			Dictionary["Model"].Build(this);
		}

		private T NewModel()
		{
			T model = new();
			model.Schema = this;
			return model;
		}

		public static implicit operator Schema<T>(Hashtable rhs)
		{
			Schema<T> lhs = new();
			foreach (DictionaryEntry entry in rhs)
			{
				lhs.Dictionary[(string)entry.Key] = entry.Value;
			}
			return lhs;
		}
	}

	public abstract partial class Schema2<T>
	{
		public abstract T Target { get; set; }

		public Schema2()
		{
			_name = new(this);
			_useHardMount = new(this);
			_schema = new(this);
			_fullName = new(this, () => Parent != null ? Path.Combine(Parent.FullName, Name) : Name);
			_cache = new(this);
		}

		private string _name;

		public virtual string Name
		{
			get => _name;
			set
			{
				FileSystem.ValidateFileNameChars(value);
				Cache.ClearAll();
				_name = value;
			}
		}

		public bool UseHardMount { get; set; }

		private SchemaComponent _schema;

		public List<Schema> Children
		{
			// If mounted then create children
			get => _schema.Children;
			set => _schema.Children = value;
		}

		private CachePropertyComponent<string> _fullName;

		public string FullName { get => _fullName.Value; }

		private CacheComponent _cache;

		public CacheComponent Cache { get => _cache; }

		public bool IsMounted { get; private set; }
	}

	public abstract partial class Schema2
	{
		public abstract bool Exists { get; }
	}

	public abstract partial class Schema2 : ICloneable
	{
		public object Clone()
		{
			Schema other = (Schema)MemberwiseClone();
			other.Parent = (Schema)Parent?.Clone();
			other.Children = Children.Select(child => (Schema)child.Clone()).ToList();
			return other;
		}
	}

	public abstract partial class Schema2
	{
		private Dictionary<string, List<Action>> EventToCallbacks { get; set; }

		public void InvokeEvent(string type)
		{
			Assertion.AssertIsInitialized();
			foreach (Action callback in EventToCallbacks[type])
			{
				callback();
			}
		}
	}

#nullable enable

	public abstract partial class Schema2
	{
		private InitializerPropertyComponent<ScriptBlock?> _psBuild;

		public ScriptBlock? PSBuild
		{
			get => _psBuild.Value;
			set => _psBuild.Value = value;
		}

		public Schema? Parent
		{
			get => _schema.Parent;
			set => _schema.Parent = value;
		}

#nullable disable

		public class SchemaComponent : Component
		{
			protected virtual void Build()
			{
				Assertion.AssertIsInitialized();
				Cache.Clear();
				EventToCallbacks.Clear();
			}

			public SchemaComponent(Schema model)
				: base(model)
			{
			}

			private Schema? _parent;

			public Schema? Parent
			{
				get => _parent.Value;
				set
				{
					if (value == null)
					{
						_parent.Value = value;
					}
					else
					{
						value.AddChildren(this);
					}
				}
			}

			private List<Schema> _children;

			public List<Schema> Children
			{
				get => _children;
				set
				{
					_children.Clear();
					AddChildren(value.ToArray());
				}
			}

			public Schema AddChildren(params Schema[] models)
			{
				return AddChildren(false, models);
			}

			public Schema AddChildren(bool force, params Schema[] models)
			{
				foreach (Schema model in models)
				{
					if (model.IsMounted)
					{
						throw;
					}
					int existingChild = Children.FindIndex(child => child.Name == model.Name);
					if (existingChild != -1)
					{
						if (force)
						{
							Children.RemoveAt(existingChild);
						}
						else
						{
							throw;
						}
					}
					model.Cache.ClearAll();
					model._parent.Value = this;
					Children.Add(model);
				}
				return this;
			}

			public Schema? GetChild(string name)
			{
				return Children.Find(child => child.Name == name);
			}
		}
	}
}
