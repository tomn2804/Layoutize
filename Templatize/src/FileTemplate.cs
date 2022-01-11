using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;

namespace Templatize;

public sealed partial class FileTemplate : Template
{
    public FileTemplate(IDictionary details)
        : base(details)
    {
    }

    protected override Layout ToContext()
    {
        return new FileSystemTemplate(Details.SetItems(new[] { GetOnCreatingDetail(), GetOnMountingDetail() }));
    }

    private KeyValuePair<object, object> GetOnCreatingDetail()
    {
        EventHandler<Activity.InvokingEventArgs> handler = (object? sender, Activity.InvokingEventArgs args) =>
        {
            if (Details.TryGetValue(FileSystemTemplate.DetailOption.OnCreating, out object? onCreatingValue))
            {
                switch (onCreatingValue)
                {
                    case ScriptBlock scriptBlock:
                        scriptBlock.Invoke(sender, args);
                        break;

                    default:
                        ((EventHandler<Activity.InvokingEventArgs>)onCreatingValue).Invoke(sender, args);
                        break;
                }
            }
            Node node = (Node)sender!;
            ((FileView)node.View).Create();
        };
        return KeyValuePair.Create<object, object>(FileSystemTemplate.DetailOption.OnCreating, handler);
    }

    private KeyValuePair<object, object> GetOnMountingDetail()
    {
        EventHandler<Activity.InvokingEventArgs> handler = (object? sender, Activity.InvokingEventArgs args) =>
        {
            if (Details.TryGetValue(FileSystemTemplate.DetailOption.OnMounting, out object? onMountingValue))
            {
                switch (onMountingValue)
                {
                    case ScriptBlock scriptBlock:
                        scriptBlock.Invoke(sender, args);
                        break;

                    default:
                        ((EventHandler<Activity.InvokingEventArgs>)onMountingValue).Invoke(sender, args);
                        break;
                }
            }
            Node node = (Node)sender!;
            if (!node.View.Exists)
            {
                node.Invoke(node.View.Activities[View.ActivityOption.Create]);
            }
        };
        return KeyValuePair.Create<object, object>(FileSystemTemplate.DetailOption.OnMounting, handler);
    }
}
