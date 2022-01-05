using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;

namespace Schemata;

public sealed partial class StrictTextFileTemplate : Template<FileModel>
{
    public StrictTextFileTemplate(IDictionary details)
        : base(details)
    {
    }

    protected override Blueprint ToBlueprint()
    {
        return new TextFileTemplate(Details.SetItems(new[] { GetOnMountedDetail() }));
    }

    private KeyValuePair<object, object> GetOnMountedDetail()
    {
        EventHandler<Activity.ProcessedEventArgs> handler = (object? sender, Activity.ProcessedEventArgs args) =>
        {
            Node node = (Node)sender!;
            File.WriteAllLines(node.Model.FullName, TextEditor.Format(File.ReadLines(node.Model.FullName)));
            if (Details.TryGetValue(DetailOption.OnMounted, out object? onMountedValue))
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
        return KeyValuePair.Create<object, object>(DetailOption.OnMounted, handler);
    }
}
