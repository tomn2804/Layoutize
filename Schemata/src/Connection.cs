using System;
using System.Collections.Generic;
using System.Linq;

namespace Schemata;

public partial class Connection : Connector.Owner
{
    private Stack<Connector> Callbacks { get; } = new();

    public Model Model { get; }

    public Connection(Model model)
    {
        Model = model;
    }

    public void Push(Connector connector)
    {
        OnProcessing(connector, new(Model));
        Callbacks.Push(connector);
    }
}

public partial class Connection : IDisposable
{
    public virtual void Dispose()
    {
        while (Callbacks.Any())
        {
            OnProcessed(Callbacks.Pop(), new(Model));
        }
        GC.SuppressFinalize(this);
    }
}
