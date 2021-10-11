using Microsoft.VisualBasic.FileIO;
using System.IO;

namespace SchemataPreview
{
    public class DirectoryModel : FileSystemModel, IDirectoryModel
    {
        public DirectoryModel(Schema schema)
            : base(schema)
        {
            Children = new ChildrenProperty(this);
        }

        public ModelSet Children { get; }
        public override bool Exists => Directory.Exists(FullName);

        public override void Create()
        {
            Directory.CreateDirectory(FullName);
        }

        public override void Delete()
        {
            FileSystem.DeleteDirectory(FullName, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
        }
    }
}
