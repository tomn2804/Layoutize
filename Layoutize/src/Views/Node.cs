using System;
using System.Collections.Generic;
using System.Linq;

namespace Templatize.Views;

public sealed partial class Node : Activity.Caller
{
    public View View { get; }

    public void Invoke(Activity activity)
    {
        Callbacks.Push(activity);
        OnProcessing(new(activity));
    }

    internal Node(View view)
    {
        View = view;
    }

    private Stack<Activity> Callbacks { get; } = new();
}

public sealed partial class Node : IDisposable
{
    public void Dispose()
    {
        while (Callbacks.Any())
        {
            OnProcessed(new(Callbacks.Pop()));
        }
    }
}
