using System;
using System.Collections.Generic;
using System.Linq;

namespace Schemata;

public sealed partial class Node
{
    public Model Model { get; }
    
    public void Invoke(EventHandler<Model.ProcessingEventArgs> activity)
    {
        activity.Invoke(this, new(Model));
    }

    public void Push(EventHandler<Model.ProcessedEventArgs> callback)
    {
        Callbacks.Push(callback);
    }

    internal Node(Model model)
    {
        Model = model;
    }

    private Stack<EventHandler<Model.ProcessedEventArgs>> Callbacks { get; } = new();
}

public sealed partial class Node : IDisposable
{
    public void Dispose()
    {
        while (Callbacks.Any())
        {
            Callbacks.Pop().Invoke(this, new(Model));
        }
    }
}
