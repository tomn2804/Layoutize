using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Schemata;

public sealed class FileTemplate : Template<FileModel>
{
    public FileTemplate(IDictionary details)
        : base(details)
    {
    }

    protected override Blueprint ToBlueprint()
    {
        Blueprint blueprint = new BlankTemplate(Details);
        Blueprint.Builder builder = blueprint.ToBuilder();

        if (Details[RequiredDetails.Name].ToString()!.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
        {
            throw new ArgumentException($"Details value property '{RequiredDetails.Name}' cannot contain invalid characters.", "details");
        }

        Activity.Builder createActivity = builder.Activities[Model.DefaultActivity.Create].ToBuilder();
        createActivity.Processing.Enqueue((object? sender, Activity.ProcessingEventArgs args) => ((FileModel)args.Model).Create());
        builder.Activities[Model.DefaultActivity.Create] = createActivity.ToActivity();

        Activity.Builder mountActivity = builder.Activities[Model.DefaultActivity.Mount].ToBuilder();
        mountActivity.Processing.Enqueue((object? sender, Activity.ProcessingEventArgs args) => ((Node)sender!).Invoke(args.Model.Activities[Model.DefaultActivity.Create]));
        builder.Activities[Model.DefaultActivity.Mount] = mountActivity.ToActivity();

        return builder.ToBlueprint();
    }
}
