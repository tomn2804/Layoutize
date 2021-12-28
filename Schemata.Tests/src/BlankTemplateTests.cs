using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using Xunit;

namespace Schemata.Tests;

public sealed class BlankTemplateTests
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
                    return [BlankTemplate]$this.Details
                }}
            }}

            [Blueprint][{templateName}]@{{ Name = '{templateName}' }}
        ").Invoke().Last().BaseObject;

        PropertyInfo templatesInfo = typeof(Blueprint).GetProperty("Templates", BindingFlags.NonPublic | BindingFlags.Instance);
        ICollection<Template> actualTemplates = (ICollection<Template>)templatesInfo.GetValue(result);

        Assert.Equal(new string[] { typeof(BlankTemplate).FullName, templateName }, actualTemplates.Select(t => t.GetType().FullName));
        Assert.Equal(templateName, result.Details[Template.RequiredDetails.Name]);
        Assert.Equal(typeof(FileModel), result.ModelType);
    }
}
