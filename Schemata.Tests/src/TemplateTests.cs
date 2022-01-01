using System;
using System.Collections.Generic;
using Xunit;

namespace Schemata.Tests;

public abstract class TemplateTests<T> where T : Template
{
    public virtual void ToBlueprint_WithInvalidNullName_ThrowsException(string name)
    {
        Dictionary<object, object> details = new() { { Template.RequiredDetails.Name, name } };
        T template = (T)Activator.CreateInstance(typeof(T), details)!;
        Assert.Throws<ArgumentNullException>("details", () => (Blueprint)template);
    }

    [Fact]
    public virtual void ToBlueprint_WithMissingNameProperty_ThrowsException()
    {
        Dictionary<object, object> details = new();
        T template = (T)Activator.CreateInstance(typeof(T), details)!;
        Assert.Throws<KeyNotFoundException>(() => (Blueprint)template);
    }

    public abstract void ToBlueprint_WithValidName_ReturnsBlueprint(string name);

    public abstract void ToBlueprint_FromDynamicComposition_ReturnsBlueprint();
}
