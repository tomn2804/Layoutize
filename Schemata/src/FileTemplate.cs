using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Schemata;

public sealed partial class FileTemplate : Template<FileModel>
{
    public FileTemplate(IDictionary details)
        : base(details)
    {
    }

    private KeyValuePair<object, object> GetOnCreatingDetail()
    {
        EventHandler<Activity.ProcessingEventArgs> handler = (object? sender, Activity.ProcessingEventArgs args) =>
        {
            if (Details.TryGetValue(FileSystemTemplate.DetailOption.OnCreating, out object? onCreatingValue) && onCreatingValue is EventHandler<Activity.ProcessingEventArgs> onCreating)
            {
                onCreating.Invoke(sender, args);
            }
            Node node = (Node)sender!;
            ((FileModel)node.Model).Create();
        };
        return KeyValuePair.Create<object, object>(FileSystemTemplate.DetailOption.OnCreating, handler);
    }

    private KeyValuePair<object, object> GetOnMountingDetail()
    {
        EventHandler<Activity.ProcessingEventArgs> handler = (object? sender, Activity.ProcessingEventArgs args) =>
        {
            if (Details.TryGetValue(FileSystemTemplate.DetailOption.OnMounting, out object? onMountingValue) && onMountingValue is EventHandler<Activity.ProcessingEventArgs> onMounting)
            {
                onMounting.Invoke(sender, args);
            }
            Node node = (Node)sender!;
            node.Invoke(node.Model.Activities[FileSystemTemplate.ActivityOption.Create]);
        };
        return KeyValuePair.Create<object, object>(FileSystemTemplate.DetailOption.OnMounting, handler);
    }

    protected override Blueprint ToBlueprint()
    {
        return new FileSystemTemplate(Details.SetItems(new[] { GetOnCreatingDetail(), GetOnMountingDetail(), }));
    }
}
