using Layoutize.Layouts;

namespace Layoutize.Elements;

internal sealed class StatelessElement : ComponentElement
{
	public StatelessElement(StatelessLayout layout)
		: base(layout)
	{
	}

	protected override Layout Build()
	{
		return Layout.Build(this);
	}

	private new StatelessLayout Layout => (StatelessLayout)base.Layout;
}
