using System;
using System.IO;

namespace Templata;

public partial class DirectoryModel : Model
{
    public ChildrenSet Children { get; }

    public override bool Exists => Directory.Exists(FullName);

    public override Tree Tree { get; }

    public virtual void Create()
    {
        Directory.CreateDirectory(FullName);
    }

    protected DirectoryModel(Blueprint blueprint)
        : base(blueprint)
    {
        if (Name.IndexOfAny(System.IO.Path.GetInvalidPathChars()) != -1)
        {
            throw new ArgumentException($"Details value property '{Template.DetailOption.Name}' cannot contain invalid system characters.", nameof(blueprint));
        }

        Children = new(this);

        blueprint.Details.TryGetValue(DirectoryTemplate.DetailOption.Traversal, out object? traversalValue);
        switch (traversalValue)
        {
            case TraversalOption.PreLevelOrder:
                Tree = new DirectoryPreLevelOrderTree(this);
                break;

            case TraversalOption.Preorder:
            default:
                Tree = new DirectoryPreorderTree(this);
                break;
        }
    }
}
