using System;
using System.Reflection;
using System.Linq;
using Layoutize.Elements;
using Layoutize.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Layoutize.Layouts;

public abstract class Layout
{
	[Required]
	[FromContext(nameof(Name))]
	[Name]
	public virtual string Name
	{
		get
		{
			Debug.Assert(this.IsMemberValid(nameof(Name), _name));
			return _name;
		}
		init
		{
			Debug.Assert(this.IsMemberValid(nameof(Name), value));
			_name = value;
			Debug.Assert(Name == value);
		}
	}

	[Required]
	[FromContext(nameof(FullName))]
	[Path]
	public string Path
	{
		get
		{
			Debug.Assert(this.IsMemberValid(nameof(Path), _path));
			return _path;
		}
		init
		{
			Debug.Assert(this.IsMemberValid(nameof(Path), value));
			_path = value;
			Debug.Assert(Path == value);
		}
	}

	[FullName]
	public virtual string FullName
	{
		get
		{
			var fullName = System.IO.Path.Combine(Path, Name);
			Debug.Assert(this.IsMemberValid(nameof(FullName), fullName));
			return fullName;
		}
	}

	internal abstract Element CreateElement();

	protected internal void InitState(IBuildContext context)
	{
		foreach (var thisProperty in GetType().GetProperties().Where(property => property.CanWrite && Attribute.IsDefined(property, typeof(FromContextAttribute))))
		{
			var thisAttribute = thisProperty.GetCustomAttribute<FromContextAttribute>();
			Debug.Assert(thisAttribute != null);
			void visitParent(Element? element)
			{
				var parent = element?.Parent;
				if (parent != null)
				{
					foreach (var parentProperty in parent.Layout.GetType().GetProperties().Where(property => property.CanRead && Attribute.IsDefined(property, typeof(ToContextAttribute))))
					{
						var parentAttribute = parentProperty.GetCustomAttribute<ToContextAttribute>();
						Debug.Assert(parentAttribute != null);
						if (thisAttribute.Equals(parentAttribute))
						{
							thisProperty.SetValue(this, parentProperty.GetValue(parent));
							return;
						}
					}
					visitParent(parent.Parent);
				}
			}
			visitParent(context.Element);
		}
	}

	private string? _name;

	private string? _path;
}
