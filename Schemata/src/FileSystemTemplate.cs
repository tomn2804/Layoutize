using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Schemata;

public sealed partial class FileSystemTemplate : Template<Model>
{
    public FileSystemTemplate(IDictionary details)
        : base(details)
    {
    }

    protected override Blueprint ToBlueprint()
    {
        return new BlankTemplate(Details.SetItems(new[] { GetActivitiesDetail() }));
    }

    private KeyValuePair<object, object> GetActivitiesDetail()
    {
        ImmutableDictionary<object, Activity> activities = ImmutableDictionary.CreateRange(new[]
        {
            KeyValuePair.Create<object, Activity>(ActivityOption.Create, CreateActivity(DetailOption.OnCreating, DetailOption.OnCreated)),
            KeyValuePair.Create<object, Activity>(ActivityOption.Mount, CreateActivity(DetailOption.OnMounting, DetailOption.OnMounted))
        });
        return KeyValuePair.Create<object, object>(Template.DetailOption.Activities, activities);
    }

    private Activity CreateActivity(object processingOption, object processedOption)
    {
        Activity.Builder builder = new();
        if (Details.TryGetValue(processingOption, out object? processingValue) && processingValue is EventHandler<Activity.ProcessingEventArgs> onProcessing)
        {
            builder.Processing.Enqueue(onProcessing);
        }
        if (Details.TryGetValue(processedOption, out object? processedValue) && processedValue is EventHandler<Activity.ProcessedEventArgs> onProcessed)
        {
            builder.Processed.Push(onProcessed);
        }
        return builder.ToActivity();
    }
}
