using System;

namespace Schemata;

public partial class Connection
{
    public event EventHandler? Processing;

    public event EventHandler? Processed;

    protected virtual void OnProcessing(EventArgs args)
    {
        Processing?.Invoke(this, args);
    }

    protected virtual void OnProcessed(EventArgs args)
    {
        Processed?.Invoke(this, args);
    }
}
