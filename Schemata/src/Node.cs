﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Schemata;

public sealed partial class Node : Activity.Owner
{
    public Model Model { get; }
    
    public void Invoke(Activity activity)
    {
        Callbacks.Push(activity);
        OnProcessing(activity, new(Model));
    }


    internal Node(Model model)
    {
        Model = model;
    }

    private Stack<Activity> Callbacks { get; } = new();
}

public sealed partial class Node : IDisposable
{
    public void Dispose()
    {
        while (Callbacks.Any())
        {
            OnProcessed(Callbacks.Pop(), new(Model));
        }
    }
}
