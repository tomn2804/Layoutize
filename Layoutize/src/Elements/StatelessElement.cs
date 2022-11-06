using Layoutize.Layouts;
using Layoutize.Utils;

namespace Layoutize.Elements;

internal sealed class StatelessElement : ComponentElement
{
	public StatelessElement(StatelessLayout layout)
		: base(layout)
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
