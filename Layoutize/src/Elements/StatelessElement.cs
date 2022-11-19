using Layoutize.Layouts;
using Layoutize.Annotations;

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
		layout.InitState(this);
		layout.Validate();
		return layout;
	}

	private new StatelessLayout Layout => (StatelessLayout)base.Layout;
}
