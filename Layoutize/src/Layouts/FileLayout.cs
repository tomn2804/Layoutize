using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Layoutize.Annotations;
using Layoutize.Contexts;
using Layoutize.Elements;
using Layoutize.Utils;
using Layoutize.Views;
using Path = System.IO.Path;

namespace Layoutize.Layouts;

public class FileLayout : ViewLayout
{
	[Required]
	[Name]
	public string Name
	{
		get
		{
			Debug.Assert(Validator.TryValidateProperty(_name, new(this) { MemberName = nameof(Name) }, null));
			return _name!;
		}
		init
		{
			Validator.ValidateProperty(value, new(this) { MemberName = nameof(Name) });
			_name = value;
			Debug.Assert(Name == value);
		}
	}

	internal override FileElement CreateElement()
	{
		Debug.Assert(Model.IsValid(this));
		return new(this);
	}

	internal override IView CreateView(IBuildContext context)
	{
		Debug.Assert(Model.IsValid(this));
		var view = new FileView(new(Path.Combine(Contexts.Path.Of(context), Name)));
		Debug.Assert(view.Name == Name);
		Debug.Assert(view.FullName == FullName.Of(context));
		return view;
	}

	private string? _name;
}
