using System.Collections.Generic;

namespace Templatize;

public sealed class DirectoryPreorderTree : Tree
{
    public override IEnumerator<Node> GetEnumerator()
    {
        Node parentNode = new(View);
        yield return parentNode;
        foreach (View child in View.Children)
        {
            foreach (Node childNode in child.Tree)
            {
                yield return childNode;
            }
        }
        parentNode.Dispose();
    }

    internal DirectoryPreorderTree(DirectoryView view)
    {
        View = view;
    }

    protected override DirectoryView View { get; }
}
