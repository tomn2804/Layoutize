using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using Xunit;

namespace Schemata.Tests;

public sealed partial class FileTemplateTests
{
    public sealed partial class WorkbenchTests
    {
        public static string WorkingDirectoryPath => $"{Path.GetTempPath()}Schemata.Tests";

        public WorkbenchTests()
        {
            Directory.CreateDirectory(WorkingDirectoryPath);
        }

        [Theory]
        [InlineData("Test")]
        [InlineData("Test.txt")]
        public void BuildTo_WorkingDirectoryFromDynamicComposition_ReturnsModel(string name)
        {
            using PowerShell instance = PowerShell.Create();

            int replication = 3;

            IEnumerable<PSObject> results = instance.AddScript($@"
                using module Schemata
                using namespace Schemata
                using namespace System.Collections

                class EmptyFileTemplate : Template[FileModel] {{
                    EmptyFileTemplate([IDictionary]$details) : base($details) {{}}

                    [Blueprint]ToBlueprint() {{
                        return [FileTemplate]$this.Details
                    }}
                }}

                $workbench = [Workbench]::new([EmptyFileTemplate]@{{ Name = '{name}' }})

                1..{replication} | ForEach-Object -Process {{
                    New-Item -Path '{WorkingDirectoryPath}' -Name $_ -ItemType 'Directory' | Out-Null
                    $workbench.BuildTo(""{WorkingDirectoryPath}\$_"")
                }}
            ").Invoke().TakeLast(replication);

            Assert.All(Enumerable.Range(1, replication), i =>
            {
                Assert.True(File.Exists($"{WorkingDirectoryPath}\\{i}\\{name}"));
                Assert.False(File.ReadAllLines($"{WorkingDirectoryPath}\\{i}\\{name}").Any());
            });
            Assert.All(results, result => Assert.IsType<FileModel>(result.BaseObject));
        }
    }

    public sealed partial class WorkbenchTests : IDisposable
    {
        public void Dispose()
        {
            if (Directory.Exists(WorkingDirectoryPath))
            {
                Directory.Delete(WorkingDirectoryPath, true);
            }
        }
    }
}
