using Layoutize.Layouts;

namespace Layoutize.Elements;

internal sealed class StatelessElement : ComponentElement
{
	public StatelessElement(Element? parent, StatelessLayout layout)
		: base(parent, layout)
	{
	}

	protected override Layout Build()
	{
		var layout = Layout.Build(this);
		Model.Validate(layout);
		return layout;
	}

	private new StatelessLayout Layout => (StatelessLayout)base.Layout;
}
