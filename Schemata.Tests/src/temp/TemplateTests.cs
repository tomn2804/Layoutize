using System;
using System.Collections.Generic;
using Xunit;

namespace Schemata.Tests;

public sealed class TemplateTests
{
    [Theory]
    [InlineData("")]
    public void ToBlueprint_WithNullInvalidName_ThrowsException(string name)
    {
        Dictionary<object, object> details = new() { { Template.RequiredDetails.Name, name } };
        FileTemplate template = new(details);

        Assert.Throws<ArgumentNullException>("Details", () => (Blueprint)template);
    }
}
