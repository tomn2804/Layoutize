using System;

namespace Schemata;

public partial class Connection
{
    public event EventHandler<ProcessingEventArgs>? Processing;

    public event EventHandler<ProcessedEventArgs>? Processed;
}
