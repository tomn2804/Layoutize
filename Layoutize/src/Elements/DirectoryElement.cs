using System.Diagnostics;
using Layoutize.Layouts;

namespace Layoutize.Elements;

internal class DirectoryElement : ViewGroupElement
{
	public DirectoryElement(DirectoryLayout layout)
		: base(layout)
	{
	}

	public override string Name
	{
		get
		{
			var name = Layout.Name;
			Debug.Assert(Contexts.Name.IsValid(name));
			return name;
		}
	}

	private new DirectoryLayout Layout => (DirectoryLayout)base.Layout;
}
