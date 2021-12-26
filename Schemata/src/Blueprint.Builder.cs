using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace Schemata;

public partial class Blueprint
{
    public static readonly Blueprint Empty = new(ImmutableDictionary.Create<object, object>(), typeof(Model), ImmutableList.Create<Template>());

    public Builder ToBuilder()
    {
        return new(this);
    }

    public class Builder
    {
        public IImmutableDictionary<object, object> Details => Templates.FirstOrDefault()?.Details ?? ImmutableDictionary.Create<object, object>();

        public Type ModelType => Templates.LastOrDefault()?.ModelType ?? typeof(Model);

        public List<Template> Templates { get; }

        public Builder()
        {
            Templates = new();
        }

        public Builder(Blueprint blueprint)
        {
            Templates = blueprint.Templates.ToList();
            Debug.Assert(blueprint.Details == Details);
            Debug.Assert(blueprint.ModelType == ModelType);
        }

        public Blueprint ToBlueprint()
        {
            return new(Details, ModelType, Templates.ToImmutableList());
        }
    }
}
