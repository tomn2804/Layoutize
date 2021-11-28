using System;
using System.IO;
using System.Linq;
using System.Management.Automation;
using Xunit;

namespace Schemata.Tests
{
    public partial class WorkbenchTests
    {
        public static string WorkingDirectory => $"{Path.GetTempPath()}Schemata.Tests";

        public WorkbenchTests()
        {
            Directory.CreateDirectory(WorkingDirectory);
        }

        [Fact]
        public void BuildMethod_EmptyDirectory_ReturnsMountedModel()
        {
            using PowerShell instance = PowerShell.Create();
            Model model = (Model)instance.AddScript($@"
            	using module Schemata
            	using namespace Schemata

            	class TestDirectorySchema : Schema<DirectoryModel> {{
                    TestDirectorySchema([ImmutableHashtable]$hashtable) : base($hashtable) {{}}

                    [Schema]Build() {{
                        return [DirectorySchema]$hashtable
                    }}
                }}

                [Workbench]::new('{WorkingDirectory}').Build(
                    [TestDirectorySchema]@{{ Name = 'Test' }}
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

            	class TestFileSchema : Schema<FileModel> {{
                    TestFileSchema([ImmutableHashtable]$hashtable) : base($hashtable) {{}}

                    [Schema]Build() {{
                        return [FileSchema]$hashtable
                    }}
                }}

                [Workbench]::new('{WorkingDirectory}').Build(
                    [TestFileSchema]@{{ Name = 'Test.txt' }}
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

        //    	class TestDirectorySchema : Schema<DirectoryModel> {{
        //            TestDirectorySchema([ImmutableHashtable]$hashtable) : base($hashtable) {{}}

        //            [Schema]Build() {{
        //                return [DirectorySchema]$hashtable
        //            }}
        //        }}

        //    	class TestFileSchema : Schema<FileModel> {{
        //            TestFileSchema([ImmutableHashtable]$hashtable) : base($hashtable) {{}}

        //            [Schema]Build() {{
        //                return [FileSchema]$hashtable
        //            }}
        //        }}

        //        [Workbench]::new('{WorkingDirectory}').Build(
        //            [TestDirectorySchema]@{{
        //                Name = 'Test'
        //                Children = [TestFileSchema]@{{ Name = 'Test.txt' }}
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
            if (Directory.Exists(WorkingDirectory))
            {
                Directory.Delete(WorkingDirectory, true);
            }
            GC.SuppressFinalize(this);
        }
    }
}
