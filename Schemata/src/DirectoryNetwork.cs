using System;
using System.Collections.Generic;

namespace Schemata;

public sealed class DirectoryNetwork : Network
{
    public override IEnumerator<Connection> GetEnumerator()
    {
        return (IEnumerator<Connection>)Activator.CreateInstance(EnumeratorType, this)!;
    }

    internal DirectoryNetwork(DirectoryModel model)
            : this(model, typeof(DirectoryPreorderEnumerator))
    {
    }

    internal DirectoryNetwork(DirectoryModel model, Type enumeratorType)
    {
        Model = model;
        EnumeratorType = enumeratorType;
    }

    internal Type EnumeratorType { get; }

    internal override DirectoryModel Model { get; }
}
