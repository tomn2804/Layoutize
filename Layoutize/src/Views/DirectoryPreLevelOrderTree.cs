using System.Collections.Generic;
using System.Linq;

namespace Layoutize.Views;

public sealed class DirectoryPreLevelOrderTree : Tree
{
    public override IEnumerator<Node> GetEnumerator()
    {
        Node parentNode = new(View);
        yield return parentNode;
        Queue<IEnumerator<Node>> childNodes = new(View.Children.Count);
        foreach (View child in View.Children)
        {
            IEnumerator<Node> childNode = child.Tree.GetEnumerator();
            if (childNode.MoveNext())
            {
                childNodes.Enqueue(childNode);
                yield return childNode.Current;
            }
        }
        while (childNodes.Any())
        {
            IEnumerator<Node> childNode = childNodes.Dequeue();
            while (childNode.MoveNext())
            {
                yield return childNode.Current;
            }
        }
        parentNode.Dispose();
    }

    internal DirectoryPreLevelOrderTree(DirectoryView view)
    {
        View = view;
    }

    protected override DirectoryView View { get; }
}
