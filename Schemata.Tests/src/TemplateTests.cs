using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Xunit;

namespace Schemata.Tests;

public class TemplateTests
{
    public static readonly object[][] ValidDetails =
    {
        new object[]
        {
            "@{ [Template+RequiredDetails]::Name = 'Test'; 1 = 2 }",
            new Dictionary<object, object> { { Template.RequiredDetails.Name, "Test" }, { 1, 2 } }
        },
        new object[]
        {
            "@{ 'Name' = 'Test'; '1' = 2 }",
            new Dictionary<object, object> { { Template.RequiredDetails.Name, "Test" }, { "1", 2 } }
        },
        new object[]
        {
            "@{ Name = 'Test'; 1 = '2' }",
            new Dictionary<object, object> { { Template.RequiredDetails.Name, "Test" }, { 1, "2" } }
        }
    };
    
    [Theory, MemberData(nameof(ValidDetails))]
    public void Constructor_InputValidDetails_ReturnsValidTemplate(string inputDetails, Dictionary<object, object> expectedDetails)
    {
        using PowerShell instance = PowerShell.Create();

        Template template = (Template)instance.AddScript($@"
            using module Schemata
            using namespace Schemata
            using namespace System.Collections

            class TestTemplate : Template[Model] {{
                TestTemplate([IEnumerable]$details) : base($details) {{}}
            }}

            [TestTemplate]{inputDetails}
        ").Invoke().Last().BaseObject;

        Assert.Equal(expectedDetails, template.Details);
    }

    public static readonly object[][] InvalidDetailsWithMissingRequiredVariables =
    {
        new object[] { new Hashtable(), Template.RequiredDetails.Name },
        new object[] { new Hashtable { { "1", 2} }, Template.RequiredDetails.Name },
        new object[] { new Hashtable { { 1, "2" } }, Template.RequiredDetails.Name }
    };

    [Theory, MemberData(nameof(InvalidDetailsWithMissingRequiredVariables))]
    public void Constructor_InputInvalidDetailsWithMissingRequiredVariables_ThrowsException(Hashtable inputDetails, string expectedMissingRequiredVariables)
    {
        Assert.Throws<ArgumentNullException>(expectedMissingRequiredVariables, () => new FileTemplate(inputDetails));
    }

    public static readonly object[][] ValidMutableDetails =
    {
        new object[] { new Dictionary<object, object> { { Template.RequiredDetails.Name, "Test" }, { 1, 2 } } },
        new object[] { new Dictionary<object, object> { { Template.RequiredDetails.Name, "Test" }, { "1", 2 } } },
        new object[] { new Dictionary<object, object> { { Template.RequiredDetails.Name, "Test" }, { "1", 2 } } }
    };

    [Theory, MemberData(nameof(ValidMutableDetails))]
    public void Constructor_InputValidMutableDetails_ReturnsValidImmutableDetails(Dictionary<object, object> inputDetails)
    {
        FileTemplate file = new(inputDetails);
        DirectoryTemplate directory = new(inputDetails);

        Assert.Equal(inputDetails, file.Details);
        Assert.Equal(inputDetails, directory.Details);
        Assert.Equal(file.Details, directory.Details);

        inputDetails.Clear();
        Assert.Empty(inputDetails);
        Assert.NotEmpty(file.Details);
        Assert.NotEmpty(directory.Details);

        Assert.NotEqual(inputDetails, file.Details);
        Assert.NotEqual(inputDetails, directory.Details);
        Assert.Equal(file.Details, directory.Details);
    }
}
