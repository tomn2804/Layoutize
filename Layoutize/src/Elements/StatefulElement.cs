using System.Diagnostics;
using Layoutize.Layouts;

namespace Layoutize.Elements;

internal sealed class StatefulElement : ComponentElement
{
	public StatefulElement(StatefulLayout layout)
		: base(layout)
	{
		_state = Layout.CreateState();
		_state.Element = this;
		_state.StateUpdated += (sender, e) => UpdateChild();
		Model.Validate(_state);
	}

	protected override Layout Build()
	{
		return State.Build(this);
	}

	private void UpdateChild()
	{
		Debug.Assert(IsMounted);
		var newChildLayout = Build();
		if (Child.Layout.GetType() == newChildLayout.GetType())
		{
			Child.Layout = newChildLayout;
		}
		else
		{
			Child = newChildLayout.CreateElement();
		}
		Debug.Assert(IsMounted);
	}

	private new StatefulLayout Layout => (StatefulLayout)base.Layout;

	private State State
	{
		get
		{
			Debug.Assert(Model.IsValid(_state));
			return _state;
		}
	}

	private readonly State _state;
}
