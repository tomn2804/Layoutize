using System;
using System.Collections.Generic;
using System.Linq;

namespace Schemata;

public partial class Connection : Connector.Owner
{
    public Connection(Model model)
    {
        Model = model;
    }

    public Model Model { get; }

    public void Push(Connector connector)
    {
        OnProcessing(connector, new(Model));
        Callbacks.Push(connector);
    }

    private Stack<Connector> Callbacks { get; } = new();
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
