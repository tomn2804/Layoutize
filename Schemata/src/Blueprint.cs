using System;
using System.Collections.Generic;

namespace Schemata;

public partial class Blueprint
{
    public IReadOnlyDictionary<object, object> Details { get; }

    public Type ModelType { get; }

    private Blueprint(IReadOnlyDictionary<object, object> details, Type modelType, IReadOnlyList<Template> templates)
    {
        Details = details;
        ModelType = modelType;
        Templates = templates;
    }

    private IReadOnlyList<Template> Templates { get; }
}
