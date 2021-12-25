using System;

namespace Schemata;

public partial class Connector
{
    public event EventHandler<ProcessingEventArgs>? Processing;

    public event EventHandler<ProcessedEventArgs>? Processed;

    public void OnProcessing(object? sender, ProcessingEventArgs args)
    {
        Processing?.Invoke(sender, args);
    }

    public void OnProcessed(object? sender, ProcessedEventArgs args)
    {
        Processed?.Invoke(sender, args);
    }
}
