using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Schemata.UnitTests
{
    public class NameProperty
    {
        //[Theory]
        //[InlineData("''")]
        //[InlineData("' '")]
        //[InlineData("'`t'")]
        //[InlineData("'`n`r'")]
        //[InlineData("$null")]
        //public void ThrowsArgumentNullException_OnValueOfNullOrWhiteSpace(string name)
        //{
        //    // Arrange
        //    //        using PowerShell instance = PowerShell.Create();
        //    //        Schema schema = (Schema)instance.AddScript($@"
        //    //	using module SchemataPreview
        //    //	using namespace SchemataPreview
        //    //	class TestModel : Model {{ TestModel([ImmutableSchema]$schema) : base($schema) {{}} }}
        //    //	[Schema[TestModel]]@{{ Name = {name} }}
        //    //").Invoke().Last().BaseObject;

        //    //        // Act & Assert
        //    //        Assert.Throws<ArgumentNullException>("Name", () => new DirectoryModel(schema.ToImmutable()));
        //}

        //[Fact]
        //public void ThrowsKeyNotFoundException_OnUndefinedProperty()
        //{
        //    // Arrange
        //    //        using PowerShell instance = PowerShell.Create();
        //    //        Schema schema = (Schema)instance.AddScript($@"
        //    //	using module SchemataPreview
        //    //	using namespace SchemataPreview
        //    //	class TestModel : Model {{ TestModel([ImmutableSchema]$schema) : base($schema) {{}} }}
        //    //	[Schema[TestModel]]@{{}}
        //    //").Invoke().Last().BaseObject;

        //    //        // Act & Assert
        //    //        Assert.Throws<KeyNotFoundException>(() => new DirectoryModel(schema.ToImmutable()));
        //}
    }
}
