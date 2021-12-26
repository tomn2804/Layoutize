using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Schemata.Tests;

public class BlueprintTests
{
    public static Template CreateValidDirectoryTemplate()
    {
        PowerShell instance = PowerShell.Create();
        return (Template)instance.AddScript($@"
            using module Schemata
            using namespace Schemata
            using namespace System.Collections

            class ValidDirectoryTemplate : Template[DirectoryModel] {{
                ValidDirectoryTemplate([IEnumerable]$details) : base($details) {{}}

                [Blueprint]ToBlueprint() {{ return [DirectoryTemplate]$this.Details }}
            }}

            [ValidDirectoryTemplate]@{{ [Template+RequiredDetails]::Name = 'Test' }}
        ").Invoke().Last().BaseObject;
    }

    public static readonly object[][] ValidTemplates =
    {
        new object[]
        {
            new FileTemplate(new Dictionary<object, object> { { Template.RequiredDetails.Name, "Test" } }),
            typeof(FileModel),
            new Type[] { typeof(BlankTemplate), typeof(FileTemplate) }
        },
        new object[]
        {
            new TextFileTemplate(new Dictionary<object, object> { { Template.RequiredDetails.Name, "Test" } }),
            typeof(FileModel),
            new Type[] { typeof(BlankTemplate), typeof(FileTemplate), typeof(TextFileTemplate) }
        },
        new object[]
        {
            new StrictTextFileTemplate(new Dictionary<object, object> { { Template.RequiredDetails.Name, "Test" } }),
            typeof(FileModel),
            new Type[] { typeof(BlankTemplate), typeof(FileTemplate), typeof(TextFileTemplate), typeof(StrictTextFileTemplate) }
        },
        new object[]
        {
            new DirectoryTemplate(new Dictionary<object, object> { { Template.RequiredDetails.Name, "Test" } }),
            typeof(DirectoryModel),
            new Type[] { typeof(BlankTemplate), typeof(DirectoryTemplate) }
        },
        new object[]
        {
            CreateValidDirectoryTemplate(),
            typeof(DirectoryModel),
            new Type[] { typeof(BlankTemplate), typeof(DirectoryTemplate), CreateValidDirectoryTemplate().GetType() }
        }
    };

    [Theory, MemberData(nameof(ValidTemplates))]
    public void Constructor_InputValidTemplate_ReturnsValidBlueprint(Template inputTemplate, Type expectedModelType, Type[] expectedTemplateLayers)
    {
        Blueprint blueprint = inputTemplate;

        PropertyInfo modelTypeInfo = typeof(Blueprint).GetProperty("ModelType", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.Equal(expectedModelType, modelTypeInfo.GetValue(blueprint));

        PropertyInfo templatesInfo = typeof(Blueprint).GetProperty("Templates", BindingFlags.NonPublic | BindingFlags.Instance);
        ICollection<Template> templates = (ICollection<Template>)templatesInfo.GetValue(blueprint);
        Assert.Equal(expectedTemplateLayers.Select(t => t.FullName), templates.Select(t => t.GetType().FullName));
    }
}
