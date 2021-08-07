using System;
using System.Collections.Generic;
using System.IO;

namespace SchemataPreview
{
	public class ExcludeModel : Model
	{
		public override bool Exists => Directory.GetFiles(Parent, Name).Length != 0;
		public override List<Model>? Children { get => null; protected internal set { } }
	}
}
