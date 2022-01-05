using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using Xunit;

namespace Schemata.Tests;

public sealed class BlankTemplateTests : TemplateTests<BlankTemplate>
{
    [Theory]
    [InlineData("Test")]
    public override void ToBlueprint_WithValidName_ReturnsBlueprint(string name)
    {
        Dictionary<object, object> details = new() { { Template.Details.Name, name } };
        BlankTemplate template = new(details);

        Blueprint result = template;

        PropertyInfo templatesInfo = typeof(Blueprint).GetProperty("Templates", BindingFlags.NonPublic | BindingFlags.Instance);
        ICollection<Template> actualTemplates = (ICollection<Template>)templatesInfo.GetValue(result);

        Assert.Equal(new string[] { typeof(BlankTemplate).FullName }, actualTemplates.Select(t => t.GetType().FullName));
        Assert.Equal(typeof(Model), result.ModelType);
    }

    [Fact]
    public override void ToBlueprint_FromDynamicComposition_ReturnsBlueprint()
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
        Assert.Equal(templateName, result.Details[Template.Details.Name]);
        Assert.Equal(typeof(FileModel), result.ModelType);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(" \t ")]
    [InlineData(" \n\r ")]
    public override void ToBlueprint_WithInvalidNullName_ThrowsException(string name)
    {
        base.ToBlueprint_WithInvalidNullName_ThrowsException(name);
    }
}
