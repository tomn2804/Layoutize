using System;
using System.Linq;
using System.Management.Automation;
using Xunit;

namespace Schemata.UnitTests
{
    public class FileNameProperty
    {
        //[Theory]
        //[InlineData("'<'")]
        //[InlineData("'|'")]
        //[InlineData("'\\'")]
        //[InlineData("'*'")]
        //public void ThrowsArgumentException_OnValueWithInvalidCharacters(string name)
        //{
        //    //// Arrange
        //    //using PowerShell instance = PowerShell.Create();
        //    //Schema schema = (Schema)instance.AddScript($@"
        //    //		using module SchemataPreview
        //    //		using namespace SchemataPreview
        //    //		class TestModel : Model {{ TestModel([ImmutableSchema]$schema) : base($schema) {{}} }}
        //    //		[Schema[TestModel]]@{{ Name = {name} }}
        //    //	").Invoke().Last().BaseObject;

        //    //// Act & Assert
        //    //Assert.Throws<ArgumentException>("Name", () => new DirectoryModel(schema.ToImmutable()));
        //}
    }
}
