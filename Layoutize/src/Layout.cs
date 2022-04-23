using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Layoutize;

public abstract class Layout
{
    protected Layout(IDictionary attributes)
    {
        Attributes = attributes switch
        {
            ImmutableDictionary<object, object> dictionary => dictionary,
            IDictionary<object, object> dictionary => dictionary.ToImmutableDictionary(),
            _ => attributes.Cast<DictionaryEntry>().ToImmutableDictionary(entry => entry.Key, entry => entry.Value!),
        };
    }

    public ImmutableDictionary<object, object> Attributes { get; }

    protected internal abstract Element CreateElement();
}

public abstract class StatelessLayout : Layout
{
    protected StatelessLayout(IDictionary attributes)
        : base(attributes)
    {
    }

    protected internal override sealed StatelessElement CreateElement()
    {
        return new(this);
    }

    protected internal abstract Layout Build(IBuildContext context);
}

public abstract class StatefulLayout : Layout
{
    protected StatefulLayout(IDictionary attributes)
        : base(attributes)
    {
    }

    protected internal override sealed StatefulElement CreateElement()
    {
        return new(this);
    }

    protected internal abstract State CreateState();
}

public abstract class ViewLayout : Layout
{
    protected ViewLayout(IDictionary attributes)
        : base(attributes)
    {
    }

    protected internal abstract override ViewElement CreateElement();

    protected internal abstract View CreateView(IBuildContext context);
}

public abstract class ViewGroupLayout : ViewLayout
{
    protected ViewGroupLayout(IDictionary attributes)
        : base(attributes)
    {
    }

    protected internal abstract override ViewGroupElement CreateElement();
}

public sealed class ScopeLayout : Layout
{
    public ScopeLayout(IDictionary attributes)
        : base(attributes)
    {
    }

    protected internal override ScopeElement CreateElement()
    {
        return new(this);
    }
}

public sealed class FileLayout : ViewLayout
{
    public FileLayout(IDictionary attributes)
        : base(attributes)
    {
    }

    protected internal override FileElement CreateElement()
    {
        return new(this);
    }

    protected internal override FileView CreateView(IBuildContext context)
    {
        throw new System.NotImplementedException();
    }
}

public sealed class DirectoryLayout : ViewGroupLayout
{
    public DirectoryLayout(IDictionary attributes)
        : base(attributes)
    {
    }

    protected internal override DirectoryElement CreateElement()
    {
        return new(this);
    }

    protected internal override DirectoryView CreateView(IBuildContext context)
    {
        throw new System.NotImplementedException();
    }
}
