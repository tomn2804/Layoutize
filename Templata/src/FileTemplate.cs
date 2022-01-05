using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;

namespace Templata;

public sealed partial class FileTemplate
{
    public new class DetailOption : Template.DetailOption
    {
    }
}

public sealed partial class FileTemplate : Template<FileModel>
{
    public FileTemplate(IDictionary details)
        : base(details)
    {
    }

    protected override Blueprint ToBlueprint()
    {
        return new BlankTemplate(Details.SetItems(new[] { GetOnCreatingDetail(), GetOnMountingDetail() }));
    }

    private KeyValuePair<object, object> GetOnCreatingDetail()
    {
        EventHandler<Activity.ProcessingEventArgs> handler = (object? sender, Activity.ProcessingEventArgs args) =>
        {
            if (Details.TryGetValue(DetailOption.OnCreating, out object? onCreatingValue))
            {
                switch (onCreatingValue)
                {
                    case ScriptBlock scriptBlock:
                        scriptBlock.Invoke(sender, args);
                        break;

                    default:
                        ((EventHandler<Activity.ProcessingEventArgs>)onCreatingValue).Invoke(sender, args);
                        break;
                }
            }
            Node node = (Node)sender!;
            ((FileModel)node.Model).Create();
        };
        return KeyValuePair.Create<object, object>(DetailOption.OnCreating, handler);
    }

    private KeyValuePair<object, object> GetOnMountingDetail()
    {
        EventHandler<Activity.ProcessingEventArgs> handler = (object? sender, Activity.ProcessingEventArgs args) =>
        {
            if (Details.TryGetValue(DetailOption.OnMounting, out object? onMountingValue))
            {
                switch (onMountingValue)
                {
                    case ScriptBlock scriptBlock:
                        scriptBlock.Invoke(sender, args);
                        break;

                    default:
                        ((EventHandler<Activity.ProcessingEventArgs>)onMountingValue).Invoke(sender, args);
                        break;
                }
            }
            Node node = (Node)sender!;
            if (!node.Model.Exists)
            {
                node.Invoke(node.Model.Activities[Model.ActivityOption.Create]);
            }
        };
        return KeyValuePair.Create<object, object>(DetailOption.OnMounting, handler);
    }
}
