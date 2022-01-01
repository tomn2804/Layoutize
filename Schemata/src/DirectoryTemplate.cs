using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Schemata;

public sealed partial class DirectoryTemplate : Template<DirectoryModel>
{
    public DirectoryTemplate(IDictionary details)
        : base(details)
    {
    }

    protected override Blueprint ToBlueprint()
    {
        Blueprint blueprint = new BlankTemplate(Details);
        Blueprint.Builder builder = blueprint.ToBuilder();

        if (Details[RequiredDetails.Name].ToString()!.IndexOfAny(Path.GetInvalidPathChars()) != -1)
        {
            throw new ArgumentException($"Details value property '{RequiredDetails.Name}' cannot contain invalid characters.", "details");
        }

        Activity.Builder createActivity = builder.Activities[Model.DefaultActivity.Create].ToBuilder();
        createActivity.Processing.Enqueue((object? sender, Activity.ProcessingEventArgs args) => ((DirectoryModel)args.Model).Create());
        builder.Activities[Model.DefaultActivity.Create] = createActivity.ToActivity();

        Activity.Builder mountActivity = builder.Activities[Model.DefaultActivity.Mount].ToBuilder();
        mountActivity.Processing.Enqueue((object? sender, Activity.ProcessingEventArgs args) =>
        {
            ((Node)sender!).Invoke(args.Model.Activities[Model.DefaultActivity.Create]);
            if (Details.TryGetValue(OptionalDetails.Children, out object? childrenValue))
            {
                switch (childrenValue)
                {
                    case Template child:
                        ((DirectoryModel)args.Model).Children.Add(child);
                        break;
                    case IEnumerable<Template> children:
                        foreach (Template child in children)
                        {
                            ((DirectoryModel)args.Model).Children.Add(child);
                        }
                        break;
                    default:
                        throw new ArgumentException($"Details value property '{OptionalDetails.Children}' must be of type '{typeof(Template).FullName}' or '{typeof(IEnumerable<Template>).FullName}'.", "details");
                }
            }
        });
        builder.Activities[Model.DefaultActivity.Mount] = mountActivity.ToActivity();

        return builder.ToBlueprint();
    }
}

public sealed partial class DirectoryTemplate
{
    public static new class OptionalDetails
    {
        public static readonly string Children = "Children";
    }
}
