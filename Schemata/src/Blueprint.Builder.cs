using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Schemata;

public sealed partial class Blueprint
{
    public sealed class Builder
    {
        public Builder(string name)
        {
            Activities = new();
            Name = name;
            Path = Directory.GetCurrentDirectory();
            Templates = new();
        }

        public Builder(Blueprint blueprint)
        {
            Activities = blueprint.Activities.ToDictionary(entry => entry.Key, entry => entry.Value);
            Name = blueprint.Name;
            Path = blueprint.Path;
            Templates = blueprint.Templates.ToList();
            Debug.Assert(blueprint.ModelType == ModelType);
        }

        public Dictionary<object, Activity> Activities { get; }

        public ImmutableDictionary<object, object> Details => Templates.FirstOrDefault()?.Details ?? ImmutableDictionary.Create<object, object>();

        public Type ModelType => Templates.LastOrDefault()?.ModelType ?? typeof(Model);

        public string Name { get; set; }

        public string Path { get; set; }

        public List<Template> Templates { get; }

        public Blueprint ToBlueprint()
        {
            return new(Activities.ToImmutableDictionary(), Details, ModelType, Name, Path, Templates.ToImmutableList());
        }
    }
}
