using System;
using System.Collections.Generic;

namespace Schemata;

public class DirectoryNetwork : Network
{
    public DirectoryNetwork(DirectoryModel model)
        : this(model, typeof(DirectoryPreorderEnumerator))
    {
    }

    public DirectoryNetwork(DirectoryModel model, Type enumeratorType)
    {
        Model = model;
        EnumeratorType = enumeratorType;
    }

    public Type EnumeratorType { get; }

    public override DirectoryModel Model { get; }

    public override IEnumerator<Connection> GetEnumerator()
    {
        return (IEnumerator<Connection>)Activator.CreateInstance(EnumeratorType, this)!;
    }
}
