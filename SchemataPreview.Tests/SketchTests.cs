using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SchemataPreview.Tests
{
    internal class SketchTests
    {
        public void ReturnsSketch_OnValidInputs()
        {
            // Arrange
            using PowerShell instance = PowerShell.Create();
            Schema schema = (Schema)instance.AddScript($@"
            		using module SchemataPreview
            		using namespace SchemataPreview
            		class TestModel : Model {{ TestModel([ImmutableSchema]$schema) : base($schema) {{}} }}
            		[Schema[TestModel]]@{{ Name = {name} }}
            	").Invoke().Last().BaseObject;

            // Act

            // Assert
            Assert.Throws<ArgumentException>("Name", () => new DirectoryModel(schema.ToImmutable()));
        }
    }
}
