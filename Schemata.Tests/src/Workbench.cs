using System;
using System.IO;
using System.Linq;
using System.Management.Automation;
using Xunit;

namespace Schemata.Tests
{
    public partial class WorkbenchTests
    {
        public static string WorkingDirectoryPath => $"{Path.GetTempPath()}Schemata.Tests";

        public WorkbenchTests()
        {
            Directory.CreateDirectory(WorkingDirectoryPath);
        }

        [Fact]
        public void BuildMethod_EmptyDirectory_ReturnsMountedModel()
        {
            using PowerShell instance = PowerShell.Create();
            Model model = (Model)instance.AddScript($@"
            	using module Schemata
            	using namespace Schemata
                using namespace System.Collections

            	class TestDirectoryTemplate : Template<DirectoryModel> {{
                    TestDirectoryTemplate([IDictionary]$outline) : base($outline) {{}}

                    [Blueprint]Build() {{
                        return [DirectoryTemplate]$this.Outline
                    }}
                }}

                [Workbench]::new('{WorkingDirectoryPath}').Build(
                    [TestDirectoryTemplate]@{{ Name = 'Test' }}
                )
            ").Invoke().Last().BaseObject;
            Assert.True(model.Exists);
        }

        [Fact]
        public void BuildMethod_EmptyFile_ReturnsMountedModel()
        {
            using PowerShell instance = PowerShell.Create();
            Model model = (Model)instance.AddScript($@"
            	using module Schemata
            	using namespace Schemata
                using namespace System.Collections

            	class TestFileTemplate : Template<FileModel> {{
                    TestFileTemplate([IDictionary]$outline) : base($outline) {{}}

                    [Blueprint]Build() {{
                        return [FileTemplate]$this.Outline
                    }}
                }}

                [Workbench]::new('{WorkingDirectoryPath}').Build(
                    [TestFileTemplate]@{{ Name = 'Test' }}
                )
            ").Invoke().Last().BaseObject;
            Assert.True(model.Exists);
        }

        //[Fact]
        //public void BuildMethod_DirectoryWithSingleChild_ReturnsMountedModel()
        //{
        //    using PowerShell instance = PowerShell.Create();
        //    Model model = (Model)instance.AddScript($@"
        //    	using module Schemata
        //    	using namespace Schemata
        //      using namespace System.Collections

        //    	class TestDirectoryTemplate : Template<DirectoryModel> {{
        //            TestDirectoryTemplate([IDictionary]$outline) : base($outline) {{}}

        //            [Template]Build() {{
        //                return [DirectoryTemplate]$this.Outline
        //            }}
        //        }}

        //    	class TestFileTemplate : Template<FileModel> {{
        //            TestFileTemplate([IDictionary]$outline) : base($outline) {{}}

        //            [Template]Build() {{
        //                return [FileTemplate]$this.Outline
        //            }}
        //        }}

        //        [Workbench]::new('{WorkingDirectory}').Build(
        //            [TestDirectoryTemplate]@{{
        //                Name = 'Test'
        //                Children = [TestFileTemplate]@{{ Name = 'Test.txt' }}
        //            }}
        //        )
        //    ").Invoke().Last().BaseObject;
        //    Assert.True(model.Exists);
        //    Assert.Collection(model.Children, child => Assert.True(model.Exists));
        //}
    }

    public partial class WorkbenchTests : IDisposable
    {
        public void Dispose()
        {
            if (Directory.Exists(WorkingDirectoryPath))
            {
                Directory.Delete(WorkingDirectoryPath, true);
            }
            GC.SuppressFinalize(this);
        }
    }
}
