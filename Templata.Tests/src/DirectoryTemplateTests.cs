using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using Xunit;

namespace Templata.Tests;

public sealed partial class DirectoryTemplateTests : TemplateTests<DirectoryTemplate>
{
    [Fact]
    public override void ToBlueprint_BasicCase_ReturnsBlueprint()
    {
        Dictionary<object, object> details = new() { { Template.DetailOption.Name, nameof(DirectoryTemplateTests) } };
        DirectoryTemplate template = new(details);

        Blueprint result = template;

        PropertyInfo templatesInfo = typeof(Blueprint).GetProperty("Templates", BindingFlags.NonPublic | BindingFlags.Instance);
        ICollection<Template> actualTemplates = (ICollection<Template>)templatesInfo.GetValue(result);

        Assert.Equal(new string[] { typeof(BlankTemplate).FullName, typeof(DirectoryTemplate).FullName }, actualTemplates.Select(t => t.GetType().FullName));
        Assert.Equal(typeof(DirectoryModel), result.ModelType);
    }

    [Fact]
    public override void ToBlueprint_FromDynamicComposition_ReturnsBlueprint()
    {
        using PowerShell terminal = PowerShell.Create();

        string templateName = nameof(DirectoryTemplateTests);

        Blueprint result = (Blueprint)terminal.AddScript($@"
            using module Templata
            using namespace Templata
            using namespace System.Collections

            class {templateName} : Template[DirectoryModel] {{
                {templateName}([IDictionary]$details) : base($details) {{}}

                [Blueprint]ToBlueprint() {{
                    return [DirectoryTemplate]$this.Details
                }}
            }}

            [Blueprint][{templateName}]@{{ [Template+DetailOption]::Name = '{templateName}' }}
        ").Invoke().Last().BaseObject;

        PropertyInfo templatesInfo = typeof(Blueprint).GetProperty("Templates", BindingFlags.NonPublic | BindingFlags.Instance);
        ICollection<Template> actualTemplates = (ICollection<Template>)templatesInfo.GetValue(result);

        Assert.Equal(new string[] { typeof(BlankTemplate).FullName, typeof(DirectoryTemplate).FullName, templateName }, actualTemplates.Select(t => t.GetType().FullName));
        Assert.Equal(templateName, result.Details[Template.DetailOption.Name]);
        Assert.Equal(typeof(DirectoryModel), result.ModelType);
    }

    [Fact]
    public void ToBlueprint_FromNonDerivedModelType_ThrowsException()
    {
        Dictionary<object, object> details = new() { { Template.DetailOption.Name, "_" } };
        InvalidData.NonDerivedModelTypeTemplate template = new(details);
        Assert.Throws<InvalidOperationException>(() => (Blueprint)template);
    }
}

public sealed partial class DirectoryTemplateTests
{
    public sealed class InvalidData
    {
        public class NonDerivedModelTypeTemplate : Template<FileModel>
        {
            public NonDerivedModelTypeTemplate(IDictionary details)
                : base(details)
            {
            }

            protected override Blueprint ToBlueprint()
            {
                return new DirectoryTemplate(Details);
            }
        }
    }
}
