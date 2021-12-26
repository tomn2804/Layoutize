using System;
using System.Collections.Generic;
using System.Linq;

namespace Schemata;

public sealed partial class Connection : Connector.Owner
{
    public Model Model { get; }

    public void Push(Connector connector)
    {
        OnProcessing(connector, new(Model));
        Callbacks.Push(connector);
    }

    internal Connection(Model model)
    {
        Model = model;
    }

    private Stack<Connector> Callbacks { get; } = new();
}

public sealed partial class Connection : IDisposable
{
    public void Dispose()
    {
        while (Callbacks.Any())
        {
            OnProcessed(Callbacks.Pop(), new(Model));
        }
    }
}
