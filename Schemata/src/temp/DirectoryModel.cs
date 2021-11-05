using Microsoft.VisualBasic.FileIO;
using System.Diagnostics;
using System.IO;
using System.Management.Automation;

namespace Schemata
{
	public partial class DirectoryModel : FileSystemModel
	{
		public DirectoryModel(ImmutableDefinition Outline)
			: base(outline)
		{
			PipeAssembly[PipeOption.Create].OnProcessing += (_, _) =>
			{
				Directory.CreateDirectory(FullName);
			};
			PipeAssembly[PipeOption.Delete].OnProcessing += (_, _) =>
			{
				FileSystem.DeleteDirectory(FullName, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
			};
			_children = new(this);
		}

		public override ModelSet Children => _children;
		public override bool Exists => Directory.Exists(FullName);
		private readonly ChildrenProperty _children;
	}

	public partial class DirectoryModel
	{
		public class ChildrenProperty : DefaultProperty<ModelSet>
		{
			public ChildrenProperty(Model model)
				: base(model, "Children", () => new ModelSet(model))
			{
			}

			protected override bool TryGetValue(out ModelSet? result)
			{
				if (Outline.TryGetValue(Key, out object? value))
				{
					Debug.Assert(@object is not null and not PSObject);
					result = new(Model, @object.ToArray<Outline>());
					return true;
				}
				result = default;
				return false;
			}
		}
	}
}
