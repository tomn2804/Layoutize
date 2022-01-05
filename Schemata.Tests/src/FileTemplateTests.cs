using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using Xunit;

namespace Schemata.Tests;

public sealed partial class FileTemplateTests : TemplateTests<FileTemplate>
{
    [Fact]
    public override void ToBlueprint_BasicCase_ReturnsBlueprint()
    {
        Dictionary<object, object> details = new() { { Template.DetailOption.Name, nameof(FileTemplateTests) } };
        FileTemplate template = new(details);

        Blueprint result = template;

        PropertyInfo templatesInfo = typeof(Blueprint).GetProperty("Templates", BindingFlags.NonPublic | BindingFlags.Instance);
        ICollection<Template> actualTemplates = (ICollection<Template>)templatesInfo.GetValue(result);

        Assert.Equal(new string[] { typeof(BlankTemplate).FullName, typeof(FileSystemTemplate).FullName, typeof(FileTemplate).FullName }, actualTemplates.Select(t => t.GetType().FullName));
        Assert.Equal(typeof(FileModel), result.ModelType);
    }

    [Fact]
    public override void ToBlueprint_FromDynamicComposition_ReturnsBlueprint()
    {
        using PowerShell terminal = PowerShell.Create();

        string templateName = nameof(FileTemplateTests);

        Blueprint result = (Blueprint)terminal.AddScript($@"
            using module Schemata
            using namespace Schemata
            using namespace System.Collections

            class {templateName} : Template[FileModel] {{
                {templateName}([IDictionary]$details) : base($details) {{}}

                [Blueprint]ToBlueprint() {{
                    return [FileTemplate]$this.Details
                }}
            }}

            [Blueprint][{templateName}]@{{ Name = '{templateName}' }}
        ").Invoke().Last().BaseObject;

        PropertyInfo templatesInfo = typeof(Blueprint).GetProperty("Templates", BindingFlags.NonPublic | BindingFlags.Instance);
        ICollection<Template> actualTemplates = (ICollection<Template>)templatesInfo.GetValue(result);

        Assert.Equal(new string[] { typeof(BlankTemplate).FullName, typeof(FileSystemTemplate).FullName, typeof(FileTemplate).FullName, templateName }, actualTemplates.Select(t => t.GetType().FullName));
        Assert.Equal(templateName, result.Details[Template.DetailOption.Name]);
        Assert.Equal(typeof(FileModel), result.ModelType);
    }

    [Fact]
    public void ToBlueprint_FromNonDerivedModelType_ThrowsException()
    {
        Dictionary<object, object> details = new() { { Template.DetailOption.Name, "_" } };
        InvalidData.NonDerivedModelTypeTemplate template = new(details);
        Assert.Throws<InvalidOperationException>(() => (Blueprint)template);
    }
}

public sealed partial class FileTemplateTests
{
    public sealed class InvalidData
    {
        public class NonDerivedModelTypeTemplate : Template<DirectoryModel>
        {
            public NonDerivedModelTypeTemplate(IDictionary details)
                : base(details)
            {
            }

            protected override Blueprint ToBlueprint()
            {
                return new FileTemplate(Details);
            }
        }
    }
}
