﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;

namespace Templata;

public sealed partial class StrictTextFileTemplate
{
    public new class DetailOption : TextFileTemplate.DetailOption
    {
    }
}

public sealed partial class StrictTextFileTemplate : Template<FileView>
{
    public StrictTextFileTemplate(IDictionary details)
        : base(details)
    {
    }

    protected override Context ToBlueprint()
    {
        return new TextFileTemplate(Details.SetItems(new[] { GetOnMountedDetail() }));
    }

    private KeyValuePair<object, object> GetOnMountedDetail()
    {
        EventHandler<Activity.ProcessedEventArgs> handler = (object? sender, Activity.ProcessedEventArgs args) =>
        {
            Node node = (Node)sender!;
            File.WriteAllLines(node.View.FullName, TextEditor.Format(File.ReadLines(node.View.FullName)));
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
