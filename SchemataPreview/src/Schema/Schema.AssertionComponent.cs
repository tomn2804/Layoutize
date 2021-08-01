using System.Diagnostics;
using System.IO;

namespace SchemataPreview
{
	public abstract partial class Schema
	{
		internal class AssertionComponent : Property
		{
			public AssertionComponent(Schema model)
				: base(model)
			{
			}

			public void AssertIsInitialized()
			{
				AssertHasValidName();
				AssertHasValidFullName();
			}

			public void AssertHasValidName()
			{
				Debug.Assert(!string.IsNullOrWhiteSpace(Schema.Name));
				Debug.Assert(Schema.Name.IndexOfAny(Path.GetInvalidFileNameChars()) == -1);
			}

			public void AssertHasValidFullName()
			{
				Debug.Assert(!string.IsNullOrWhiteSpace(this));
				Debug.Assert(Path.IsPathFullyQualified(this));
				Debug.Assert(this.IndexOfAny(Path.GetInvalidPathChars()) == -1);
			}
		}
	}
}
