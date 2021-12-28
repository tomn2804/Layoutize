using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using Xunit;

namespace Schemata.Tests;

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
    public void BuildTo_WorkingDirectoryPathWithEmptyDirectoryTemplate_ReturnsMountedDirectoryModel(string directoryName)
    {
        using PowerShell instance = PowerShell.Create();

        IEnumerable<PSObject> results = instance.AddScript($@"
            using module Schemata
            using namespace Schemata
            using namespace System.Collections

            class EmptyDirectoryTemplate : Template[DirectoryModel] {{
                EmptyDirectoryTemplate([IEnumerable]$details) : base($details) {{}}

                [Blueprint]ToBlueprint() {{
                    return [DirectoryTemplate]$this.Details
                }}
            }}

            $workbench = [Workbench]::new([EmptyDirectoryTemplate]@{{ Name = '{directoryName}' }})

            $workbench.BuildTo('{WorkingDirectoryPath}\1')
            $workbench.BuildTo('{WorkingDirectoryPath}\2')
            $workbench.BuildTo('{WorkingDirectoryPath}\3')
        ").Invoke().TakeLast(3);

        Assert.All(Enumerable.Range(1, 3), i =>
        {
            Assert.True(Directory.Exists($"{WorkingDirectoryPath}\\{i}\\{directoryName}"));
            Assert.False(Directory.EnumerateFileSystemEntries($"{WorkingDirectoryPath}\\{i}\\{directoryName}").Any());
        });
        Assert.All(results, result => Assert.IsType<DirectoryModel>(result.BaseObject));
    }

    [Theory]
    [InlineData("Test2")]
    [InlineData("Test2.txt")]
    public void BuildTo_WorkingDirectoryPathWithEmptyFileTemplate_ReturnsMountedFileModel(string fileName)
    {
        using PowerShell instance = PowerShell.Create();

        IEnumerable<PSObject> results = instance.AddScript($@"
            using module Schemata
            using namespace Schemata
            using namespace System.Collections

            class EmptyFileTemplate : Template[FileModel] {{
                EmptyFileTemplate([IEnumerable]$details) : base($details) {{}}

                [Blueprint]ToBlueprint() {{
                    return [FileTemplate]$this.Details
                }}
            }}

            $workbench = [Workbench]::new([EmptyFileTemplate]@{{ Name = '{fileName}' }})

            New-Item -Path '{WorkingDirectoryPath}' -Name '1' -ItemType 'Directory'
            New-Item -Path '{WorkingDirectoryPath}' -Name '2' -ItemType 'Directory'
            New-Item -Path '{WorkingDirectoryPath}' -Name '3' -ItemType 'Directory'

            $workbench.BuildTo('{WorkingDirectoryPath}\1')
            $workbench.BuildTo('{WorkingDirectoryPath}\2')
            $workbench.BuildTo('{WorkingDirectoryPath}\3')
        ").Invoke().TakeLast(3);

        Assert.All(Enumerable.Range(1, 3), i =>
        {
            Assert.True(File.Exists($"{WorkingDirectoryPath}\\{i}\\{fileName}"));
            Assert.False(File.ReadAllLines($"{WorkingDirectoryPath}\\{i}\\{fileName}").Any());
        });
        Assert.All(results, result => Assert.IsType<FileModel>(result.BaseObject));
    }

    //[Theory]
    //[InlineData(@"{
    //    $onMounted = {
    //        if ($this.Details['ShouldUpdate']) {
    //            $this.Details = $this.Details.SetItem('Children', @([FileTemplate]@{ Name = 'FileTemplate' }))
    //        }
    //    }
    //    return [DirectoryTemplate]$this.Details.SetItem('OnMounted', $onMounted)
    //}")]
    //public void BuildTo_WorkingDirectoryPathWithUpdatableDirectoryTemplate_ReturnsIndependentlyUpdatedDirectoryModel(string toBlueprintDefinition)
    //{
    //    using PowerShell instance = PowerShell.Create();

    //    IEnumerable<PSObject> results = instance.AddScript($@"
    //        using module Schemata
    //        using namespace Schemata
    //        using namespace System.Collections

    //        class UpdatableDirectoryTemplate : Template[DirectoryModel] {{
    //            UpdatableDirectoryTemplate([IEnumerable]$details) : base($details) {{}}

    //            [Blueprint]ToBlueprint() {toBlueprintDefinition}
    //        }}

    //        $workbench = [Workbench]::new([UpdatableDirectoryTemplate]@{{ Name = 'UpdatableDirectoryTemplate' }})

    //        $workbench.BuildTo('{WorkingDirectoryPath}\1')
    //        $workbench.BuildTo('{WorkingDirectoryPath}\2')
    //        $workbench.BuildTo('{WorkingDirectoryPath}\3')
    //    ").Invoke().TakeLast(3);

    //    Assert.All(Enumerable.Range(1, 3), i =>
    //    {
    //        Assert.True(Directory.Exists($"{WorkingDirectoryPath}\\{directoryName}\\{i}"));
    //        Assert.False(Directory.EnumerateFileSystemEntries($"{WorkingDirectoryPath}\\{directoryName}\\{i}").Any());
    //    });
    //    Assert.All(results, result => Assert.IsType<DirectoryModel>(result.BaseObject));
    //}
}

public sealed partial class WorkbenchTests : IDisposable
{
    public void Dispose()
    {
        if (Directory.Exists(WorkingDirectoryPath))
        {
            //Directory.Delete(WorkingDirectoryPath, true);
        }
    }
}
