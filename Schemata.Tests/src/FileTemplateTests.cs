using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using Xunit;

namespace Schemata.Tests;

public sealed partial class FileTemplateTests : TemplateTests<FileTemplate>
{
    [Theory]
    [InlineData("Test")]
    public override void ToBlueprint_WithValidName_ReturnsBlueprint(string name)
    {
        Dictionary<object, object> details = new() { { Template.RequiredDetails.Name, name } };
        FileTemplate template = new(details);

        Blueprint result = template;

        PropertyInfo templatesInfo = typeof(Blueprint).GetProperty("Templates", BindingFlags.NonPublic | BindingFlags.Instance);
        ICollection<Template> actualTemplates = (ICollection<Template>)templatesInfo.GetValue(result);

        Assert.Equal(new string[] { typeof(BlankTemplate).FullName, typeof(FileTemplate).FullName }, actualTemplates.Select(t => t.GetType().FullName));
        Assert.Equal(typeof(FileModel), result.ModelType);
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
                    return [FileTemplate]$this.Details
                }}
            }}

            [Blueprint][{templateName}]@{{ Name = '{templateName}' }}
        ").Invoke().Last().BaseObject;

        PropertyInfo templatesInfo = typeof(Blueprint).GetProperty("Templates", BindingFlags.NonPublic | BindingFlags.Instance);
        ICollection<Template> actualTemplates = (ICollection<Template>)templatesInfo.GetValue(result);

        Assert.Equal(new string[] { typeof(BlankTemplate).FullName, typeof(FileTemplate).FullName, templateName }, actualTemplates.Select(t => t.GetType().FullName));
        Assert.Equal(templateName, result.Name);
        Assert.Equal(typeof(FileModel), result.ModelType);
    }

    [Fact]
    public void ToBlueprint_FromNonDerivedModelType_ThrowsException()
    {
        Dictionary<object, object> details = new() { { Template.RequiredDetails.Name, "_" } };
        InvalidData.NonDerivedModelTypeTemplate template = new(details);
        Assert.Throws<InvalidOperationException>(() => (Blueprint)template);
    }

    [Theory, MemberData(nameof(InvalidData.NullNames), MemberType = typeof(InvalidData))]
    public override void ToBlueprint_WithInvalidNullName_ThrowsException(string name)
    {
        base.ToBlueprint_WithInvalidNullName_ThrowsException(name);
    }

    [Theory, MemberData(nameof(InvalidData.NonNullNames), MemberType = typeof(InvalidData))]
    public void ToBlueprint_WithInvalidNonNullName_ThrowsException(string name)
    {
        Dictionary<object, object> details = new() { { Template.RequiredDetails.Name, name } };
        FileTemplate template = new(details);
        Assert.Throws<ArgumentException>("details", () => (Blueprint)template);
    }
}

public sealed partial class FileTemplateTests
{
    public sealed class InvalidData
    {
        public static IEnumerable<object[]> NullNames => Path.GetInvalidFileNameChars().Where(name => string.IsNullOrWhiteSpace(name.ToString())).Select(name => new object[] { name.ToString() });

        public static IEnumerable<object[]> NonNullNames => Path.GetInvalidFileNameChars().Where(name => !string.IsNullOrWhiteSpace(name.ToString())).Select(name => new object[] { name.ToString() });

        public class NonDerivedModelTypeTemplate : Template<DirectoryModel>
        {
            public NonDerivedModelTypeTemplate(IDictionary details)
                : base(details)
            {
            }

            protected override Blueprint ToBlueprint()
            {
                return new FileTemplate((IDictionary)Details);
            }
        }
    }
}
