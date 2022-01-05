using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Collections;

namespace Schemata;

public class DirectoryModel : Model
{
    public ChildrenSet Children { get; }

    public override Tree Tree { get; }

    public DirectoryModel(Blueprint blueprint)
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
            case DirectoryTemplate.TraversalOption.LevelOrder:
                Tree = new DirectoryLevelOrderTree(this);
                break;
            case DirectoryTemplate.TraversalOption.Preorder:
            default:
                Tree = new DirectoryPreorderTree(this);
                break;
        }
    }

    public virtual void Create()
    {
        Directory.CreateDirectory(FullName);
    }
}
