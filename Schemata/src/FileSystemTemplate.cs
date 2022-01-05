using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Management.Automation;

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

    private Activity CreateActivity(object processingOption, object processedOption)
    {
        Activity.Builder builder = new();
        if (Details.TryGetValue(processingOption, out object? processingValue))
        {
            EventHandler<Activity.ProcessingEventArgs> handler;
            switch (processingValue)
            {
                case ScriptBlock scriptBlock:
                    handler = new((sender, args) => scriptBlock.Invoke(sender, args));
                    break;

                case Action<object?, Activity.ProcessingEventArgs> action:
                    handler = new(action);
                    break;

                default:
                    handler = (EventHandler<Activity.ProcessingEventArgs>)processingValue;
                    break;
            }
            builder.Processing.Enqueue(handler);
        }
        if (Details.TryGetValue(processedOption, out object? processedValue))
        {
            EventHandler<Activity.ProcessedEventArgs> handler;
            switch (processedValue)
            {
                case ScriptBlock scriptBlock:
                    handler = new((sender, args) => scriptBlock.Invoke(sender, args));
                    break;

                case Action<object?, Activity.ProcessedEventArgs> action:
                    handler = new(action);
                    break;

                default:
                    handler = (EventHandler<Activity.ProcessedEventArgs>)processedValue;
                    break;
            }
            builder.Processed.Push(handler);
        }
        return builder.ToActivity();
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
}
