using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using Xunit;

namespace Schemata.Tests;

public sealed partial class DirectoryTemplateTests
{
    [Collection("Working directory")]
    public sealed partial class WorkbenchTests
    {
        private WorkingDirectoryFixture Fixture { get; }

        public WorkbenchTests(WorkingDirectoryFixture fixture)
        {
            Fixture = fixture;
        }

        [Theory]
        [InlineData("Test")]
        [InlineData("Test.txt")]
        public void BuildTo_WorkingDirectoryFromDynamicComposition_ReturnsModel(string name)
        {
            using PowerShell instance = PowerShell.Create();

            int replication = 3;
            string templateName = nameof(DirectoryTemplateTests);
            string workingDirectoryPath = Fixture.CreateUniqueWorkingDirectory();

            IEnumerable<PSObject> results = instance.AddScript($@"
                using module Schemata
                using namespace Schemata
                using namespace System.Collections

                class {templateName} : Template[DirectoryModel] {{
                    {templateName}([IDictionary]$details) : base($details) {{}}

                    [Blueprint]ToBlueprint() {{
                        return [DirectoryTemplate]$this.Details
                    }}
                }}

                $workbench = [Workbench]::new([{templateName}]@{{ Name = '{name}' }})

                1..{replication} | ForEach-Object -Process {{
                    New-Item -Path '{workingDirectoryPath}' -Name $_ -ItemType 'Directory' | Out-Null
                    $workbench.BuildTo(""{workingDirectoryPath}\$_"")
                }}
            ").Invoke().TakeLast(replication);

            Assert.All(Enumerable.Range(1, replication), i =>
            {
                Assert.True(Directory.Exists($"{workingDirectoryPath}\\{i}\\{name}"));
                Assert.False(Directory.EnumerateFileSystemEntries($"{workingDirectoryPath}\\{i}\\{name}").Any());
            });
            Assert.All(results, result => Assert.IsType<DirectoryModel>(result.BaseObject));
        }

        [Theory, MemberData(nameof(InvalidData.NonNullNames), MemberType = typeof(InvalidData))]
        public void Build_WithInvalidNonNullName_ThrowsException(string name)
        {
            Dictionary<object, object> details = new() { { Template.DetailOption.Name, name } };
            DirectoryTemplate template = new(details);
            Workbench workbench = new(template);
            Assert.Throws<ArgumentException>("blueprint", () =>
            {
                try
                {
                    workbench.Build();
                }
                catch (TargetInvocationException e)
                {
                    throw e.InnerException;
                }
            });
        }

        [Theory, MemberData(nameof(InvalidData.NullNames), MemberType = typeof(InvalidData))]
        public void Build_WithInvalidNullName_ThrowsException(string name)
        {
            Dictionary<object, object> details = new() { { Template.DetailOption.Name, name } };
            DirectoryTemplate template = new(details);
            Workbench workbench = new(template);
            Assert.Throws<ArgumentNullException>("blueprint", () =>
            {
                try
                {
                    workbench.Build();
                }
                catch (TargetInvocationException e)
                {
                    throw e.InnerException;
                }
            });
        }
    }

    public sealed partial class WorkbenchTests
    {
        public sealed class InvalidData
        {
            public static IEnumerable<object[]> NonNullNames => Path.GetInvalidPathChars().Where(name => !string.IsNullOrWhiteSpace(name.ToString())).Select(name => new object[] { name.ToString() });
            public static IEnumerable<object[]> NullNames => Path.GetInvalidPathChars().Where(name => string.IsNullOrWhiteSpace(name.ToString())).Select(name => new object[] { name.ToString() });
        }
    }
}
