using Layoutize.Layouts;

namespace Layoutize.Elements;

internal sealed class FileElement : ViewElement
{
	public FileElement(Element parent, FileLayout layout)
		: base(parent, layout)
	{
	}
}
