using Layoutize.Elements;
using System.IO;

namespace Layoutize;

public class Tree
{
    public Tree(Layout layout)
    {
        Root = layout.CreateElement();
    }

    public void MountTo(string path)
    {
        Directory.SetCurrentDirectory(path);
        Root.MountTo(null);
    }

    public void Unmount()
    {
        Root.Unmount();
    }

    private Element Root { get; }
}
