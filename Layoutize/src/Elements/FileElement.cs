using System.Diagnostics;
using Layoutize.Layouts;

namespace Layoutize.Elements;

internal class FileElement : ViewElement
{
	public FileElement(FileLayout layout)
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

	private new FileLayout Layout => (FileLayout)base.Layout;
}
