using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Layoutize;

public abstract class Layout
{
    private protected Layout(IEnumerable attributes)
    {
        Attributes = attributes switch
        {
            IImmutableDictionary<object, object> dictionary => dictionary,
            IEnumerable<KeyValuePair<object, object>> entries => entries.ToImmutableDictionary(),
            _ => attributes.Cast<DictionaryEntry>().ToImmutableDictionary(entry => entry.Key, entry => entry.Value!)
        };
    }

    public IImmutableDictionary<object, object> Attributes { get; }

    internal abstract Element CreateElement();
}

public abstract class ComponentLayout : Layout
{
    private protected ComponentLayout(IDictionary attributes)
        : base(attributes)
    {
    }

    internal abstract override ComponentElement CreateElement();
}

public abstract class StatelessLayout : ComponentLayout
{
    protected StatelessLayout(IDictionary attributes)
        : base(attributes)
    {
    }

    internal override sealed StatelessElement CreateElement()
    {
        return new(this);
    }

    protected internal abstract Layout Build(IBuildContext context);
}

public abstract class StatefulLayout : ComponentLayout
{
    protected StatefulLayout(IDictionary attributes)
        : base(attributes)
    {
    }

    internal override sealed StatefulElement CreateElement()
    {
        return new(this);
    }

    protected internal abstract State CreateState();
}

public abstract class ViewLayout : Layout
{
    private protected ViewLayout(IDictionary attributes)
        : base(attributes)
    {
    }

    internal abstract override ViewElement CreateElement();

    internal abstract View CreateView(IBuildContext context);
}

public abstract class ViewGroupLayout : ViewLayout
{
    private protected ViewGroupLayout(IDictionary attributes)
       : base(attributes)
    {
    }

    internal abstract override ViewGroupElement CreateElement();
}

public sealed class FileLayout : ViewLayout
{
    public FileLayout(IDictionary attributes)
        : base(attributes)
    {
    }

    internal override sealed FileElement CreateElement()
    {
        return new(this);
    }

    internal override sealed FileView CreateView(IBuildContext context)
    {
        return new(new(System.IO.Path.Combine(Path.Of(context), (string)Attributes["Name"])));
    }
}

public sealed class DirectoryLayout : ViewGroupLayout
{
    public DirectoryLayout(IDictionary attributes)
        : base(attributes)
    {
    }

    internal override sealed DirectoryElement CreateElement()
    {
        return new(this);
    }

    internal override sealed DirectoryView CreateView(IBuildContext context)
    {
        return new(new(System.IO.Path.Combine(Path.Of(context), (string)Attributes["Name"])));
    }
}
