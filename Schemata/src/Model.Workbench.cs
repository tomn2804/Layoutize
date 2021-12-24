namespace Schemata;

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
