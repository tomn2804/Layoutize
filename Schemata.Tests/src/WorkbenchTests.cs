using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using SchemataPreview;
using System.Management.Automation;

namespace Schemata.Tests
{
    public class WorkbenchTests
    {
        [Fact]
        public void BuildMethod_EmptyFile_ReturnsMountedModel()
        {
            using PowerShell instance = PowerShell.Create();
            Model model = (Model)instance.AddScript($@"
            	using module Schemata
            	using namespace Schemata

            	class EmptyFileSchema : Schema<FileModel> {{
                    EmptyFileSchema([ImmutableHashtable]$hashtable) : base($hashtable) {{}}

                    [Schema]Build() {{
                        return [FileSchema]$hashtable
                    }}
                }}

            	$schema = [EmptyFileSchema]@{{ Name = 'Empty.txt' }}

                [Workbench]::new($schema).Build('C:\Users\Tom\Desktop')
            ").Invoke().Last().BaseObject;
            Assert.True(model.Exists);
        }
    }
}
