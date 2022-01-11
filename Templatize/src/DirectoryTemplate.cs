using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace Templatize;

public sealed partial class DirectoryTemplate
{
    public new class DetailOption : Template.DetailOption
    {
        public const string Children = nameof(Children);
        public const string Traversal = nameof(Traversal);
    }
}

public sealed partial class DirectoryTemplate : Template<DirectoryView>
{
    public DirectoryTemplate(IDictionary details)
        : base(details)
    {
    }

    protected override Layout ToBlueprint()
    {
        return new BlankTemplate(Details.SetItems(new[] { GetOnCreatingDetail(), GetOnMountedDetail(), GetOnMountingDetail() }));
    }

    private KeyValuePair<object, object> GetOnCreatingDetail()
    {
        EventHandler<Activity.ProcessingEventArgs> handler = (object? sender, Activity.ProcessingEventArgs args) =>
        {
            if (Details.TryGetValue(Template.DetailOption.OnCreating, out object? onCreatingValue))
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
            ((DirectoryView)node.View).Create();
        };
        return KeyValuePair.Create<object, object>(Template.DetailOption.OnCreating, handler);
    }

    private KeyValuePair<object, object> GetOnMountedDetail()
    {
        EventHandler<Activity.ProcessedEventArgs> handler = (object? sender, Activity.ProcessedEventArgs args) =>
        {
            if (Details.TryGetValue(DetailOption.Children, out object? childrenValue))
            {
                Node node = (Node)sender!;
                IEnumerable<object>? children = childrenValue as IEnumerable<object>;
                if (children is null)
                {
                    children = new[] { childrenValue };
                }
                ((DirectoryView)node.View).Children.AddRange(children.Cast<Template>());
            }
            if (Details.TryGetValue(Template.DetailOption.OnMounted, out object? onMountedValue))
            {
                switch (onMountedValue)
                {
                    case ScriptBlock scriptBlock:
                        scriptBlock.Invoke(sender, args);
                        break;

                    default:
                        ((EventHandler<Activity.ProcessedEventArgs>)onMountedValue).Invoke(sender, args);
                        break;
                }
            }
        };
        return KeyValuePair.Create<object, object>(Template.DetailOption.OnMounted, handler);
    }

    private KeyValuePair<object, object> GetOnMountingDetail()
    {
        EventHandler<Activity.ProcessingEventArgs> handler = (object? sender, Activity.ProcessingEventArgs args) =>
        {
            if (Details.TryGetValue(Template.DetailOption.OnMounting, out object? onMountingValue))
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
            if (!node.View.Exists)
            {
                node.Invoke(node.View.Activities[View.ActivityOption.Create]);
            }
        };
        return KeyValuePair.Create<object, object>(Template.DetailOption.OnMounting, handler);
    }
}
