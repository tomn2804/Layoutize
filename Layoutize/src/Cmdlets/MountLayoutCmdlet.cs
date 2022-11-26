using Layoutize.Layouts;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Management.Automation;
using static Layoutize.Cmdlets.LocalElement;

namespace Layoutize.Cmdlets;

[Cmdlet(VerbsOther.Use, "Effect")]
public class UseEffectCmdlet : Cmdlet
{
	[Parameter(Position = 0)]
	public object Key { get; set; } = null!;

	[Parameter(Position = 1)]
	public ScriptBlock Value { get; set; } = null!;

	protected override void ProcessRecord()
	{
		if (!LocalEffect.Current.Contains(Key)) // TODO: LocalStorage need to be another nested dict, because Key can be dup with state
		{
			LocalEffect.Current = LocalEffect.Current.Add(Key);
			LocalElement.Current.Mounted += () => Value.Invoke();
		}
	}
}

[Cmdlet(VerbsOther.Use, "State")]
public class UseStateCmdlet : Cmdlet
{
	[Parameter(Position = 0)]
	public IEnumerable<object> Key { get; set; } = null!;

	[Parameter(Position = 1)]
	public object? Value { get; set; } = default;

	protected override void ProcessRecord()
	{
		var cache = LocalSnapshot.Current;
		if (cache == null) throw new InvalidOperationException();
		WriteObject(cache.Compute(key, Value));
	}
}

internal static class LocalState
{
	public static State Current => LocalContextProvider<State>.Context;
}

internal class State : LocalContextProvider<State>
{
	public State()
	{
		Context = this;
	}
}

internal class LocalSnapshot
{
	private static ImmutableDictionary<Element, Snapshot> _current = ImmutableDictionary<Element, Snapshot>.Empty;

	public static Snapshot Current
	{
		get
		{
			var element = LocalElement.Current;
			if (_current.TryGetValue(element, out Snapshot? value)) return value;
			value = new Snapshot();
			element.Unmounted += (object? sender, EventArgs e) => _current = _current.Remove(element);
			_current = _current.Add(element, value);
			return value;
		}
	}
}

internal class Snapshot : LocalSnapshot
{
	private static ImmutableDictionary<object, object> Cache = ImmutableDictionary<object, object>.Empty;

	public object? Save(object key, Func<object> calculation)
	{
		if (Cache.TryGetValue(key, out object? value)) return value;
		value = calculation.Invoke();
		Cache = Cache.Add(key, value);
		return value;
	}
}

public abstract class LayoutCmdlet : Cmdlet
{
	[Parameter(Mandatory = true, Position = 0)]
	public string Name { get; set; } = null!;

	[Parameter]
	public ScriptBlock? OnMounting { get; set; }

	[Parameter]
	public ScriptBlock? OnMounted { get; set; }

	[Parameter]
	public ScriptBlock? OnUnmounting { get; set; }

	[Parameter]
	public ScriptBlock? OnUnmounted { get; set; }

	protected override void ProcessRecord()
	{
		if (Current is not DirectoryElement) throw new InvalidOperationException();
		var element = CreateElement();
		element.Mounting += (object? sender, EventArgs e) => OnMounting?.ToAction().Invoke();
		element.Mounted += (object? sender, EventArgs e) => OnMounted?.ToAction().Invoke();
		element.Unmounting += (object? sender, EventArgs e) => OnUnmounting?.ToAction().Invoke();
		element.Unmounted += (object? sender, EventArgs e) => OnUnmounted?.ToAction().Invoke();
		WriteObject(element);
	}

	private protected abstract Element CreateElement();
}

[Cmdlet(VerbsOther.Use, "File")]
public class UseFileCmdlet : LayoutCmdlet
{
	private protected override Element CreateElement()
	{
		return new FileElement();
	}
}

[Cmdlet(VerbsOther.Use, "Directory")]
public class UseDirectoryCmdlet : LayoutCmdlet
{
	[Parameter(Mandatory = true, Position = 1)]
	public ScriptBlock Layout { get; set; } = null!;

	private protected override Element CreateElement()
	{
		return new DirectoryElement { Layout = Layout };
	}
}

internal static class LocalElement
{
	public static Element Current => LocalContextProvider<Element>.Context;
}

internal abstract class LocalContextProvider<T>
{
	public static T Context { get; protected set; } = default!; // TODO: Assert Current not null on get
}

internal abstract class Element : LocalContextProvider<Element>
{
	protected Element Parent { get; private set; } = null!; // TODO: Assert Current not null on get, and must be null on set

	public event EventHandler? Mounting;

	public event EventHandler? Mounted;

	public event EventHandler? Unmounting;

	public event EventHandler? Unmounted;

	public virtual void Mount()
	{
		Parent = Context;
		Context = this;
	}

	public virtual void Unmount()
	{
		Mounting = null;
		Mounted = null;
		Unmounting = null;
		Unmounted = null;
	}
}

internal class FileElement : Element
{
}

internal class DirectoryElement : Element
{
	public ScriptBlock? Layout { get; init; }

	public ImmutableList<Element> Children { get; private set; } = ImmutableList<Element>.Empty; // TODO: Assert LocalElement.Current = this, Assert only settable if not mounted

	public override void Mount()
	{
		base.Mount();
		// TODO: View.Create();
		if (Layout != null) Children = ImmutableList.CreateRange(Layout.Invoke().Where(obj => obj.BaseObject is Element).Select(obj => (Element)obj.BaseObject));
		foreach (var child in Children)
		{
			child.Mount();
			Context = this;
		}
	}
}

internal class RootDirectoryElement : DirectoryElement
{
	public string Path { get; init; }
}

[Cmdlet(VerbsData.Mount, "Layout")]
public class MountLayoutCmdlet : PSCmdlet
{
	[Parameter(Mandatory = true, Position = 0)]
	public ScriptBlock Layout { get; set; } = null!;

	protected override void ProcessRecord()
	{
		new DirectoryElement() { Layout = Layout }.Mount();
	}
}

internal static class ScriptBlockExtension
{
	public static Action ToAction(this ScriptBlock script)
	{
		return () => { foreach (var _ in script.Invoke()); };
	}
}
