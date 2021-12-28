using System;
using System.Collections.Generic;
using System.IO;

namespace Schemata;

public class FileModel : Model
{
    public override FileTree Tree { get; }

    protected FileModel(string path, Blueprint blueprint)
        : base(path, blueprint)
    {
        Schemata.Activity.Builder builder = new();
        builder.Processing.Push((object? sender, Schemata.Activity.ProcessingEventArgs args) => Console.WriteLine("Processing File " + Name));
        builder.Processing.Push((object? sender, Schemata.Activity.ProcessingEventArgs args) => Create());

        builder.Processed.Enqueue((object? sender, Schemata.Activity.ProcessedEventArgs args) => Console.WriteLine("Processed File " + Name));

        Activities = Activities.SetItem(Activity.Mount, builder.ToConnector());

        Tree = new(this);
    }

    protected virtual void Create()
    {
        File.Create($"{Path}\\{Name}").Dispose();
    }
}
