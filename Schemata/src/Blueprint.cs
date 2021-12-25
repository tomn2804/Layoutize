using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Schemata;

public partial class Blueprint
{
    public IImmutableDictionary<object, object> Details { get; }

    public Type ModelType { get; }

    private Blueprint(ImmutableList<Template> templates)
    {
        Templates = templates;
        Details = Templates.FirstOrDefault()?.Details ?? ImmutableDictionary.Create<object, object>();
        ModelType = Templates.LastOrDefault()?.ModelType ?? typeof(Model);
    }

    private ImmutableList<Template> Templates { get; }
}
