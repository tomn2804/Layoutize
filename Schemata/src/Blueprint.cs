using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Schemata;

public partial class Blueprint
{
    public IImmutableDictionary<object, object?> Details => Templates.First().Details;

    public Type ModelType => Templates.LastOrDefault()?.ModelType ?? typeof(Model);

    private Blueprint()
    {
    }

    private List<Template> Templates { get; } = new();
}
