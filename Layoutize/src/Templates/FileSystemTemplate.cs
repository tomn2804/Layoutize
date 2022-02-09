using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using System.Linq;
using System.IO;
using Layoutize.Attributes;

namespace Layoutize.Templates;

[FileSystemBinding]
public sealed class FileSystemTemplate : Template
{
    public FileSystemTemplate(IDictionary attributes)
        : base(attributes)
    {
    }

    protected override Layout Build()
    {
        string name;
        if (Attributes.TryGetValue(DetailOption.Name, out object? nameValue))
        {
            name = nameValue.ToString()!;
        }
        else
        {
            throw new KeyNotFoundException();
        }

        Type viewType;
        if (Attributes.TryGetValue(DetailOption.ViewType, out object? viewTypeValue))
        {
            viewType = (Type)viewTypeValue;
        }
        else
        {
            throw new KeyNotFoundException();
        }

        string path;
        if (Attributes.TryGetValue(DetailOption.Path, out object? pathValue))
        {
            path = pathValue.ToString()!;
        }
        else
        {
            path = Directory.GetCurrentDirectory();
        }

        Layout.Builder builder = new(name, path, viewType);

        int priority = 0;
        if (Attributes.TryGetValue(DetailOption.Priority, out object? priorityValue))
        {
            priority = priorityValue;
        }

        View view = (View)Activator.CreateInstance(viewType, new Layout() { Activities = activities, Name = name, Path = path, Priority = priority })!;

        view.Activities[View.ActivityOption.Create] = CreateActivity(Template.DetailOption.OnCreating, Template.DetailOption.OnCreated);
        view.Activities[View.ActivityOption.Mount] = CreateActivity(Template.DetailOption.OnCreating, Template.DetailOption.OnCreated);

        return view;
    }

    private Activity CreateActivity(object invokingOption, object invokedOption)
    {
        Activity.Builder builder = new();
        if (Attributes.TryGetValue(invokingOption, out object? invokingValue))
        {
            EventHandler<Activity.InvokingEventArgs> handler;
            switch (invokingValue)
            {
                case ScriptBlock scriptBlock:
                    handler = (sender, args) => scriptBlock.Invoke(sender, args);
                    break;

                case Action<object?, Activity.InvokingEventArgs> action:
                    handler = new(action);
                    break;

                default:
                    handler = (EventHandler<Activity.InvokingEventArgs>)invokingValue;
                    break;
            }
            builder.Invoking.Push(handler);
        }
        if (Attributes.TryGetValue(invokedOption, out object? invokedValue))
        {
            EventHandler<Activity.InvokedEventArgs> handler;
            switch (invokedValue)
            {
                case ScriptBlock scriptBlock:
                    handler = (sender, args) => scriptBlock.Invoke(sender, args);
                    break;

                case Action<object?, Activity.InvokedEventArgs> action:
                    handler = new(action);
                    break;

                default:
                    handler = (EventHandler<Activity.InvokedEventArgs>)invokedValue;
                    break;
            }
            builder.Invoked.Enqueue(handler);
        }
        return builder.ToActivity();
    }
}
