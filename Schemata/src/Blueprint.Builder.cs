using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace Schemata;

public sealed partial class Blueprint
{
    internal sealed class Builder
    {
        internal Builder()
        {
            Templates = new();
        }

        internal Builder(Blueprint blueprint)
        {
            Templates = blueprint.Templates.ToList();
            Debug.Assert(blueprint.Details == Details);
            Debug.Assert(blueprint.ModelType == ModelType);
        }

        internal IReadOnlyDictionary<object, object> Details => Templates.FirstOrDefault()?.Details ?? ImmutableDictionary.Create<object, object>();

        internal Type ModelType => Templates.LastOrDefault()?.ModelType ?? typeof(Model);

        internal List<Template> Templates { get; }

        internal Blueprint ToBlueprint()
        {
            return new() { Details = Details, ModelType = ModelType, Templates = Templates.ToImmutableList() };
        }
    }
}
