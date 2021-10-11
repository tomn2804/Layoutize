using Microsoft.VisualBasic.FileIO;
using System.Diagnostics;
using System.IO;
using System.Management.Automation;

namespace SchemataPreview
{
	public partial class DirectoryModel : FileSystemModel
	{
		public DirectoryModel(ImmutableDefinition Props)
			: base(props)
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
				if (Props.TryGetValue(Key, out object? value))
				{
					Debug.Assert(@object is not null and not PSObject);
					result = new(Model, @object.ToArray<Props>());
					return true;
				}
				result = default;
				return false;
			}
		}
	}
}
