using System;

namespace Schemata;

public partial class Connection
{
    public event EventHandler? Processing;

    public event EventHandler? Processed;

    protected virtual void OnProcessing()
    {
        Processing?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnProcessed()
    {
        Processed?.Invoke(this, EventArgs.Empty);
    }
}
