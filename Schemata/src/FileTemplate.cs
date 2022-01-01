using System;
using System.Collections;
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

        builder.Activities[Model.DefaultActivity.Create] = CreateModel;
        builder.Activities[Model.DefaultActivity.Mount] = MountModel;

        return builder.ToBlueprint();
    }

    private void CreateModel(object? sender, Model.ProcessingEventArgs args)
    {
        if (Details.TryGetValue(OptionalDetails.OnCreating, out object? onCreatingValue) && onCreatingValue is EventHandler<Model.ProcessingEventArgs> onCreating)
        {
            onCreating.Invoke(sender, args);
        }
        ((FileModel)args.Model).Create();
        if (Details.TryGetValue(OptionalDetails.OnCreated, out object? onCreatedValue) && onCreatedValue is EventHandler<Model.ProcessedEventArgs> onCreated)
        {
            ((Node)sender!).Push(onCreated);
        }
    }

    private void MountModel(object? sender, Model.ProcessingEventArgs args)
    {
        if (Details.TryGetValue(OptionalDetails.OnMounting, out object? onMountingValue) && onMountingValue is EventHandler<Model.ProcessingEventArgs> onMounting)
        {
            onMounting.Invoke(sender, args);
        }
        args.Model.Activities[Model.DefaultActivity.Create].Invoke(sender, args);
        if (Details.TryGetValue(OptionalDetails.OnMounted, out object? onMountedValue) && onMountedValue is EventHandler<Model.ProcessedEventArgs> onMounted)
        {
            ((Node)sender!).Push(onMounted);
        }
    }
}
