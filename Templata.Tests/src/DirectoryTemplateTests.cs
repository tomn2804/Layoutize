﻿using System;
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

        Context result = template;

        PropertyInfo templatesInfo = typeof(Context).GetProperty("Templates", BindingFlags.NonPublic | BindingFlags.Instance);
        ICollection<Template> actualTemplates = (ICollection<Template>)templatesInfo.GetValue(result);

        Assert.Equal(new string[] { typeof(BlankTemplate).FullName, typeof(DirectoryTemplate).FullName }, actualTemplates.Select(t => t.GetType().FullName));
        Assert.Equal(typeof(DirectoryView), result.ViewType);
    }

    [Fact]
    public override void ToBlueprint_FromDynamicComposition_ReturnsBlueprint()
    {
        using PowerShell terminal = PowerShell.Create();

        string templateName = nameof(DirectoryTemplateTests);

        Context result = (Context)terminal.AddScript($@"
            using module Templata
            using namespace Templata
            using namespace System.Collections

            class {templateName} : Template[DirectoryView] {{
                {templateName}([IDictionary]$details) : base($details) {{}}

                [Context]ToBlueprint() {{
                    return [DirectoryTemplate]$this.Details
                }}
            }}

            [Context][{templateName}]@{{ [Template+DetailOption]::Name = '{templateName}' }}
        ").Invoke().Last().BaseObject;

        PropertyInfo templatesInfo = typeof(Context).GetProperty("Templates", BindingFlags.NonPublic | BindingFlags.Instance);
        ICollection<Template> actualTemplates = (ICollection<Template>)templatesInfo.GetValue(result);

        Assert.Equal(new string[] { typeof(BlankTemplate).FullName, typeof(DirectoryTemplate).FullName, templateName }, actualTemplates.Select(t => t.GetType().FullName));
        Assert.Equal(templateName, result.Details[Template.DetailOption.Name]);
        Assert.Equal(typeof(DirectoryView), result.ViewType);
    }

    [Fact]
    public void ToBlueprint_FromNonDerivedViewType_ThrowsException()
    {
        Dictionary<object, object> details = new() { { Template.DetailOption.Name, "_" } };
        InvalidData.NonDerivedViewTypeTemplate template = new(details);
        Assert.Throws<InvalidOperationException>(() => (Context)template);
    }
}

public sealed partial class DirectoryTemplateTests
{
    public sealed class InvalidData
    {
        public class NonDerivedViewTypeTemplate : Template<FileView>
        {
            public NonDerivedViewTypeTemplate(IDictionary details)
                : base(details)
            {
            }

            protected override Context ToBlueprint()
            {
                return new DirectoryTemplate(Details);
            }
        }
    }
}
