using System;
using System.Collections.Generic;
using System.Linq;

namespace Schemata;

public partial class Blueprint
{
    private List<Template> Templates { get; } = new();

    public Type ModelType => Templates.LastOrDefault()?.ModelType ?? typeof(Model);

    private Blueprint()
    {
    }
}
