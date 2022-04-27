using Layoutize.Elements;
using System.IO;

namespace Layoutize;

public class Tree
{
    private Element Root { get; }

    public Tree(Layout layout)
    {
        Root = layout.CreateElement();
    }

    public void Mount()
    {
        Root.MountTo(null);
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
}
