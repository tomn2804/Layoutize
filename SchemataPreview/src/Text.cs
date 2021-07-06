using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace SchemataPreview.Models
{
	public partial class Text : File
	{
		public string[] InitializerContents { get; set; }

		public string[] Contents
		{
			get => System.IO.File.ReadAllLines(FullName);
			set => System.IO.File.WriteAllLines(FullName, value);
		}

		public Text(string name)
			: base(name)
		{
			InitializerContents = Array.Empty<string>();
		}

		public Text(string name, string[] contents)
			: base(name)
		{
			InitializerContents = contents;
		}
	}

	public partial class Text : File
	{
		public override void Create()
		{
			base.Create();
			Contents = InitializerContents;
		}

		public override void Cleanup()
		{
			base.Create();
			Contents = Format(Contents);
		}
	}

	public partial class Text : File
	{
		public static string[] Format(string[] contents)
		{
			return contents.Distinct().Select(
				line => Regex.Replace(Regex.Replace(line.TrimEnd(), "(?<=\t) +| +(?=\t)", ""), " {2,}", " ")
			).ToArray();
		}
	}
}
