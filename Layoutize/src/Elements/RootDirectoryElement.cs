using Layoutize.Layouts;
using System.Diagnostics;

namespace Layoutize.Elements
{
	internal sealed class RootDirectoryElement : ViewGroupElement
	{
		public RootDirectoryElement(RootDirectoryLayout layout)
			: base(layout)
		{
		}

		public override Element? Parent => null;

		public override bool IsMounted
		{
			get
			{
				if (View != null)
				{
					Debug.Assert(View.Exists);
					return true;
				}
				return false;
			}
		}
	}
}
