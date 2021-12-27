using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace Schemata;

public class DirectoryModel : Model
{
    public List<Model> Children { get; } = new();

    public override DirectoryLevelOrderNetwork Tree { get; }

    public DirectoryModel(string path, Blueprint blueprint)
        : base(path, blueprint)
    {
        Schemata.Activity.Builder builder = new();
        builder.Processing.Push((object? sender, Schemata.Activity.ProcessingEventArgs args) => Console.WriteLine("Processing Directory " + Name));
        builder.Processing.Push((object? sender, Schemata.Activity.ProcessingEventArgs args) => Create());

        builder.Processed.Enqueue((object? sender, Schemata.Activity.ProcessedEventArgs args) => Console.WriteLine("Processed Directory " + Name));

        Activities = Activities.SetItem(Activity.Mount, builder.ToConnector());

        Tree = new(this);
    }

    protected virtual void Create()
    {
        Directory.CreateDirectory($"{Path}\\{Name}");
    }
}
