using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using Xunit;

namespace Templatize.Tests;

public sealed partial class FileTemplateTests
{
    [Collection("Working directory")]
    public sealed partial class WorkbenchTests
    {
        public WorkbenchTests(WorkingDirectoryFixture fixture)
        {
            Fixture = fixture;
        }

        [Theory, MemberData(nameof(InvalidData.NonNullNames), MemberType = typeof(InvalidData))]
        public void Build_WithInvalidNonNullName_ThrowsException(string name)
        {
            Dictionary<object, object> details = new() { { Template.DetailOption.Name, name } };
            FileTemplate template = new(details);
            View.Factory workbench = new(template);
            Assert.Throws<ArgumentException>("context", () =>
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
            FileTemplate template = new(details);
            View.Factory workbench = new(template);
            Assert.Throws<ArgumentNullException>("context", () =>
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

        [Theory]
        [InlineData("Test")]
        [InlineData("Test.txt")]
        public void BuildTo_WorkingDirectoryFromDynamicComposition_ReturnsView(string viewName)
        {
            using PowerShell instance = PowerShell.Create();

            int replication = 3;
            string templateName = nameof(FileTemplateTests);
            string workingDirectoryPath = GetWorkingDirectory(nameof(BuildTo_WorkingDirectoryFromDynamicComposition_ReturnsView));

            IEnumerable<PSObject> results = instance.AddScript($@"
                using module Templatize
                using namespace Templatize
                using namespace System.Collections

                class {templateName} : Template[FileView] {{
                    {templateName}([IDictionary]$details) : base($details) {{}}

                    [Context]ToBlueprint() {{
                        return [FileTemplate]$this.Details
                    }}
                }}

                $workbench = [View+Workbench]::new([{templateName}]@{{ [Template+DetailOption]::Name = '{viewName}' }})

                1..{replication} | ForEach-Object -Process {{
                    New-Item -Path '{workingDirectoryPath}' -Name $_ -ItemType 'Directory' | Out-Null
                    $workbench.BuildTo(""{workingDirectoryPath}\$_"")
                }}
            ").Invoke().TakeLast(replication);

            Assert.All(Enumerable.Range(1, replication), i =>
            {
                Assert.True(File.Exists($"{workingDirectoryPath}\\{i}\\{viewName}"));
                Assert.False(File.ReadAllLines($"{workingDirectoryPath}\\{i}\\{viewName}").Any());
            });
            Assert.All(results, result => Assert.IsType<FileView>(result.BaseObject));
        }

        private WorkingDirectoryFixture Fixture { get; }

        private string GetWorkingDirectory(string testName)
        {
            return Directory.CreateDirectory(Path.Combine(WorkingDirectoryFixture.Path, $"{nameof(FileTemplateTests)}_{testName}")).FullName;
        }
    }

    public sealed partial class WorkbenchTests
    {
        public sealed class InvalidData
        {
            public static IEnumerable<object[]> NonNullNames => Path.GetInvalidFileNameChars().Where(name => !string.IsNullOrWhiteSpace(name.ToString())).Select(name => new object[] { name.ToString() });
            public static IEnumerable<object[]> NullNames => Path.GetInvalidFileNameChars().Where(name => string.IsNullOrWhiteSpace(name.ToString())).Select(name => new object[] { name.ToString() });
        }
    }
}
