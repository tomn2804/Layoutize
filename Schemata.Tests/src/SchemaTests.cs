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
    public class SchemaTests
    {
        [Fact]
        public void MountMethod_DynamicallyDefinedSchema_ReturnsModel()
        {
            // Arrange
            using PowerShell instance = PowerShell.Create();
            Schema schema = (Schema)instance.AddScript($@"
            	using module Schemata
            	using namespace Schemata

            	class TestFileSchema : Schema<FileModel> {{
                    TestFileSchema([ImmutableLegend]$legend) : base($legend) {{}}

                    [Schema]Build() {{
                        return [FileSchema]$this.Legend
                    }}
                }}

            	[TestFileSchema]@{{ Name = 'Test.txt' }}
            ").Invoke().Last().BaseObject;

            // Act
            Model model = schema.Mount();

            // Assert
            Assert.True(model.Exists);
        }
    }
}
