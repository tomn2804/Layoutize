using System;
using System.Collections;
using System.Linq;
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
        Blueprint blueprint = new BlankTemplate((IDictionary)Details);
        Blueprint.Builder builder = blueprint.ToBuilder();

        if (((string)Details[RequiredDetails.Name]).IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
        {
            throw new ArgumentException($"Details value property '{RequiredDetails.Name}' cannot contain invalid characters.", "details");
        }

        Activity.Builder mountActivity = new();
        mountActivity.Processing.Push((object? sender, Activity.ProcessingEventArgs args) => ((FileModel)sender!).Create());
        if (Details.TryGetValue(OptionalDetails.OnMounting, out object? onMountingValue) && onMountingValue is EventHandler<Activity.ProcessingEventArgs> onMounting)
        {
            mountActivity.Processing.Push(onMounting);
        }
        if (Details.TryGetValue(OptionalDetails.OnMounted, out object? onMountedValue) && onMountedValue is EventHandler<Activity.ProcessedEventArgs> onMounted)
        {
            mountActivity.Processed.Enqueue(onMounted);
        }
        builder.Activities[Model.DefaultActivity.Mount] = mountActivity.ToActivity();

        return builder.ToBlueprint();
    }
}
