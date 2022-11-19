using System.Diagnostics;
using Layoutize.Layouts;
using Layoutize.Annotations;

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
		};
		Debug.Assert(State == _state);
	}

	protected override Layout Build()
	{
		var layout = State.Build(this);
		layout.InitState(this);
		layout.Validate();
		return layout;
	}

	private new StatefulLayout Layout => (StatefulLayout)base.Layout;

	private State State
	{
		get
		{
			Debug.Assert(_state.IsValid());
			return _state;
		}
	}

	private readonly State _state;
}
