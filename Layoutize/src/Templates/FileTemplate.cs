using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;

namespace Templatize.Templates;

public sealed partial class FileTemplate : Template
{
    public FileTemplate(IDictionary attributes)
        : base(attributes)
    {
    }

    protected override Layout Build()
    {
        return new FileSystemTemplate(Attributes.SetItems(new[] { GetOnCreatingDetail(), GetOnMountingDetail() }));
    }

    private KeyValuePair<object, object> GetOnCreatingDetail()
    {
        EventHandler<Activity.InvokingEventArgs> handler = (object? sender, Activity.InvokingEventArgs args) =>
        {
            if (Attributes.TryGetValue(FileSystemTemplate.DetailOption.OnCreating, out object? onCreatingValue))
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
            if (Attributes.TryGetValue(FileSystemTemplate.DetailOption.OnMounting, out object? onMountingValue))
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
