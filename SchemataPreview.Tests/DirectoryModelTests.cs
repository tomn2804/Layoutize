using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SchemataPreview.Tests
{
	public class DirectoryModelTests
	{
		public class NameProperty
		{
			[Theory]
			[InlineData("")]
			[InlineData("Name = '' ")]
			[InlineData("Name = ' ' ")]
			[InlineData("Name = \"`t\"")]
			[InlineData("Name = \"`n`r\"")]
			[InlineData("Name = $null")]
			public void ThrowsArgumentNullException_OnNullValue(string property)
			{
				// Arrange
				using PowerShell instance = PowerShell.Create();
				Schema schema = (Schema)instance.AddScript($@"
					using module SchemataPreview
					using namespace SchemataPreview
					[Schema[DirectoryModel]]@{{ {property} }}
				").Invoke().Last().BaseObject;

				// Act & Assert
				Assert.Throws<ArgumentNullException>("Name", () => new DirectoryModel(schema.ToImmutable()));
			}
		}
	}
}
