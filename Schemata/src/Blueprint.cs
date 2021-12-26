using System;
using System.Collections.Generic;

namespace Schemata;

public sealed partial class Blueprint
{
    public IReadOnlyDictionary<object, object> Details { get; }

    internal Type ModelType { get; }

    internal Builder ToBuilder()
    {
        return new(this);
    }

    private Blueprint(IReadOnlyDictionary<object, object> details, Type modelType, IReadOnlyList<Template> templates)
    {
        Details = details;
        ModelType = modelType;
        Templates = templates;
    }

    private IReadOnlyList<Template> Templates { get; }
}
