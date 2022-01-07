using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using Xunit;

namespace Templatize.Tests;

public sealed class BlankTemplateTests : TemplateTests<BlankTemplate>
{
    [Fact]
    public override void ToBlueprint_BasicCase_ReturnsBlueprint()
    {
        Dictionary<object, object> details = new() { { Template.DetailOption.Name, nameof(BlankTemplateTests) } };
        BlankTemplate template = new(details);

        Context result = template;

        PropertyInfo templatesInfo = typeof(Context).GetProperty("Templates", BindingFlags.NonPublic | BindingFlags.Instance);
        ICollection<Template> actualTemplates = (ICollection<Template>)templatesInfo.GetValue(result);

        Assert.Equal(new string[] { typeof(BlankTemplate).FullName }, actualTemplates.Select(t => t.GetType().FullName));
        Assert.Equal(typeof(View), result.ViewType);
    }

    [Fact]
    public override void ToBlueprint_FromDynamicComposition_ReturnsBlueprint()
    {
        using PowerShell terminal = PowerShell.Create();

        string templateName = nameof(BlankTemplateTests);

        Context result = (Context)terminal.AddScript($@"
            using module Templatize
            using namespace Templatize
            using namespace System.Collections

            class {templateName} : Template[FileView] {{
                {templateName}([IDictionary]$details) : base($details) {{}}

                [Context]ToBlueprint() {{
                    return [BlankTemplate]$this.Details
                }}
            }}

            [Context][{templateName}]@{{ [Template+DetailOption]::Name = '{templateName}' }}
        ").Invoke().Last().BaseObject;

        PropertyInfo templatesInfo = typeof(Context).GetProperty("Templates", BindingFlags.NonPublic | BindingFlags.Instance);
        ICollection<Template> actualTemplates = (ICollection<Template>)templatesInfo.GetValue(result);

        Assert.Equal(new string[] { typeof(BlankTemplate).FullName, templateName }, actualTemplates.Select(t => t.GetType().FullName));
        Assert.Equal(templateName, result.Details[Template.DetailOption.Name]);
        Assert.Equal(typeof(FileView), result.ViewType);
    }
}
