using System.Diagnostics;

namespace Schemata;

public abstract partial class Model : Blueprint.Owner
{
    protected Model(Blueprint blueprint)
        : base(blueprint)
    {
        Debug.Assert(Blueprint.ModelType == GetType());
    }
}

public abstract partial class Model
{
    public class Workbench
    {
        public Workbench(string path)
        {
            WorkingDirectoryPath = path;
        }

        public string WorkingDirectoryPath { get; }
    }
}

public class FileModel : Model
{
    public FileModel(Blueprint blueprint)
        : base(blueprint)
    {
    }
}
