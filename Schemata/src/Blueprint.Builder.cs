using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Schemata;

public partial class Blueprint
{
    public Builder ToBuilder()
    {
        return new(this);
    }

    public static Blueprint Create()
    {
        return CreateBuilder().ToBlueprint();
    }

    public static Builder CreateBuilder()
    {
        return new();
    }

    public static implicit operator Blueprint(Builder builder)
    {
        return builder.ToBlueprint();
    }

    public class Builder
    {
        public IImmutableDictionary<object, object> Details => Templates.First().Details;

        public Type ModelType => Templates.LastOrDefault()?.ModelType ?? typeof(Model);

        public List<Template> Templates { get; }

        public Builder()
        {
            Templates = new();
        }

        public Builder(Blueprint blueprint)
        {
            Templates = blueprint.Templates.ToList();
        }

        public Blueprint ToBlueprint()
        {
            return new(Templates.ToImmutableList());
        }
    }
}
