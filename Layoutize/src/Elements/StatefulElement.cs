using System.ComponentModel.DataAnnotations;
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
		_state.StateUpdated += (sender, e) =>
		{
			Validator.ValidateObject(_state, new(_state));
			UpdateChild();
		};
		Validator.ValidateObject(_state, new(_state));
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
			Debug.Assert(Validator.TryValidateObject(_state, new(_state), null));
			return _state;
		}
	}

	private readonly State _state;
}
