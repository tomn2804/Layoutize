using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using Xunit;

namespace Schemata.Tests;

public sealed partial class FileTemplateTests
{
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
                {templateName}([IEnumerable]$details) : base($details) {{}}

                [Blueprint]ToBlueprint() {{
                    return [FileTemplate]$this.Details
                }}
            }}

            [Blueprint][{templateName}]@{{ Name = '{templateName}' }}
        ").Invoke().Last().BaseObject;

        PropertyInfo templatesInfo = typeof(Blueprint).GetProperty("Templates", BindingFlags.NonPublic | BindingFlags.Instance);
        ICollection<Template> actualTemplates = (ICollection<Template>)templatesInfo.GetValue(result);

        Assert.Equal(new string[] { typeof(BlankTemplate).FullName, typeof(FileTemplate).FullName, templateName }, actualTemplates.Select(t => t.GetType().FullName));
        Assert.Equal(templateName, result.Details[Template.RequiredDetails.Name]);
        Assert.Equal(typeof(FileModel), result.ModelType);
    }

    [Fact]
    public void ToBlueprint_WithNonDerivedModelType_ThrowsException()
    {
        Dictionary<object, object> details = new() { { Template.RequiredDetails.Name, "_" } };
        InvalidData.NonDerivedModelTypeTemplate template = new(details);
        Assert.Throws<InvalidOperationException>(() => (Blueprint)template);
    }

    [Theory, MemberData(nameof(InvalidData.NullNames), MemberType = typeof(InvalidData))]
    public void Constructor_WithNullInvalidName_ThrowsException(string name)
    {
        Dictionary<object, object> details = new() { { Template.RequiredDetails.Name, name } };
        FileTemplate template = new(details);
        Assert.Throws<ArgumentNullException>("details", () => (Blueprint)template);
    }

    [Theory, MemberData(nameof(InvalidData.NonNullNames), MemberType = typeof(InvalidData))]
    public void Constructor_WithNonNullInvalidName_ThrowsException(string name)
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
            public NonDerivedModelTypeTemplate(IEnumerable details)
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
