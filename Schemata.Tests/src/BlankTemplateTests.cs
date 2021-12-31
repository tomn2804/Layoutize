using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using Xunit;

namespace Schemata.Tests;

public sealed class BlankTemplateTests
{
    [Theory]
    [InlineData("Test")]
    public void ToBlueprint_Basic_ReturnsBlueprint(string name)
    {
        Dictionary<object, object> details = new() { { Template.RequiredDetails.Name, name } };
        BlankTemplate template = new(details);

        Blueprint result = template;

        PropertyInfo templatesInfo = typeof(Blueprint).GetProperty("Templates", BindingFlags.NonPublic | BindingFlags.Instance);
        ICollection<Template> actualTemplates = (ICollection<Template>)templatesInfo.GetValue(result);

        Assert.Equal(new string[] { typeof(BlankTemplate).FullName }, actualTemplates.Select(t => t.GetType().FullName));
        Assert.Equal(typeof(Model), result.ModelType);
    }

    [Fact]
    public void ToBlueprint_FromDynamicComposition_ReturnsBlueprint()
    {
        using PowerShell terminal = PowerShell.Create();

        string templateName = "BlankFileTemplate";

        Blueprint result = (Blueprint)terminal.AddScript($@"
            using module Schemata
            using namespace Schemata
            using namespace System.Collections

            class {templateName} : Template[FileModel] {{
                {templateName}([IDictionary]$details) : base($details) {{}}

                [Blueprint]ToBlueprint() {{
                    return [BlankTemplate]$this.Details
                }}
            }}

            [Blueprint][{templateName}]@{{ Name = '{templateName}' }}
        ").Invoke().Last().BaseObject;

        PropertyInfo templatesInfo = typeof(Blueprint).GetProperty("Templates", BindingFlags.NonPublic | BindingFlags.Instance);
        ICollection<Template> actualTemplates = (ICollection<Template>)templatesInfo.GetValue(result);

        Assert.Equal(new string[] { typeof(BlankTemplate).FullName, templateName }, actualTemplates.Select(t => t.GetType().FullName));
        Assert.Equal(templateName, result.Name);
        Assert.Equal(typeof(FileModel), result.ModelType);
    }

    [Fact]
    public void ToBlueprint_WithMissingNameDetail_ThrowsException()
    {
        Dictionary<object, object> details = new();
        BlankTemplate template = new(details);
        Assert.Throws<KeyNotFoundException>(() => (Blueprint)template);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(" \t ")]
    [InlineData(" \n\r ")]
    public void ToBlueprint_WithNullInvalidName_ThrowsException(string name)
    {
        Dictionary<object, object> details = new() { { Template.RequiredDetails.Name, name } };
        BlankTemplate template = new(details);
        Assert.Throws<ArgumentNullException>("details", () => (Blueprint)template);
    }
}
