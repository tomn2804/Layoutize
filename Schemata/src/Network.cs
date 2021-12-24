using System;
using System.Collections;
using System.Collections.Generic;

namespace Schemata;

public abstract class Network : IEnumerable<Connection.Segment>
{
    public abstract Model Model { get; }

    public abstract IEnumerator<Connection.Segment> GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public class FileNetwork : Network
{
    public override FileModel Model { get; }

    public FileNetwork(FileModel model)
    {
        Model = model;
    }

    public override IEnumerator<Connection.Segment> GetEnumerator()
    {
        return new FileEnumerator(this);
    }
}

public class DirectoryNetwork : Network
{
    public override DirectoryModel Model { get; }

    public Type EnumeratorType { get; }

    public DirectoryNetwork(DirectoryModel model)
        : this(model, typeof(DirectoryPreorderEnumerator))
    {
    }

    public DirectoryNetwork(DirectoryModel model, Type enumeratorType)
    {
        Model = model;
        EnumeratorType = enumeratorType;
    }

    public override IEnumerator<Connection.Segment> GetEnumerator()
    {
        return (IEnumerator<Connection.Segment>)Activator.CreateInstance(EnumeratorType, this)!;
    }
}
