using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Schemata;

public sealed partial class Blueprint
{
    public IReadOnlyDictionary<object, EventHandler<Model.ProcessingEventArgs>> Activities { get; }

    public Type ModelType { get; }

    public string Name { get; }

    public string Path { get; }

    public Builder ToBuilder()
    {
        return new(this);
    }

    private Blueprint(IReadOnlyDictionary<object, EventHandler<Model.ProcessingEventArgs>> activities, Type modelType, string name, string path, IReadOnlyList<Template> templates)
    {
        Activities = activities;
        ModelType = modelType;
        Name = name;
        Path = path;
        Templates = templates;
    }

    private IReadOnlyList<Template> Templates { get; }
}
