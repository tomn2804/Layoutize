using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Schemata;

public partial class Blueprint
{
    private List<Template> Templates { get; } = new();

    public Type ModelType => Templates.LastOrDefault()?.ModelType ?? typeof(Model);

    public IImmutableDictionary<object, object?> Details => Templates.First().Details;

    private Blueprint()
    {
    }
}
