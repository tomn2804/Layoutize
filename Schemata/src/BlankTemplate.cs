using System;
using System.Collections;

namespace Schemata;

public sealed class BlankTemplate : Template<Model>
{
    public BlankTemplate(IDictionary details)
        : base(details)
    {
    }

    protected override Blueprint ToBlueprint()
    {
        Blueprint blueprint = base.ToBlueprint();
        Blueprint.Builder builder = blueprint.ToBuilder();

        Activity.Builder createActivity = new();
        if (Details.TryGetValue(OptionalDetails.OnCreating, out object? onCreatingValue) && onCreatingValue is EventHandler<Activity.ProcessingEventArgs> onCreating)
        {
            createActivity.Processing.Enqueue(onCreating);
        }
        if (Details.TryGetValue(OptionalDetails.OnCreated, out object? onCreatedValue) && onCreatedValue is EventHandler<Activity.ProcessedEventArgs> onCreated)
        {
            createActivity.Processed.Push(onCreated);
        }
        builder.Activities[Model.DefaultActivity.Create] = createActivity.ToActivity();

        Activity.Builder mountActivity = new();
        if (Details.TryGetValue(OptionalDetails.OnMounting, out object? onMountingValue) && onMountingValue is EventHandler<Activity.ProcessingEventArgs> onMounting)
        {
            mountActivity.Processing.Enqueue(onMounting);
        }
        if (Details.TryGetValue(OptionalDetails.OnMounted, out object? onMountedValue) && onMountedValue is EventHandler<Activity.ProcessedEventArgs> onMounted)
        {
            mountActivity.Processed.Push(onMounted);
        }
        builder.Activities[Model.DefaultActivity.Mount] = mountActivity.ToActivity();

        return builder.ToBlueprint();
    }
}
