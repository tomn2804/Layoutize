using System;
using System.IO;
using System.Reflection;

namespace Schemata;

public sealed class Workbench
{
    public Workbench(Template template)
    {
        Template = template;
    }

    private Template Template { get; }

    public Model Build()
    {
        return BuildTo(Directory.GetCurrentDirectory());
    }

    public Model BuildTo(string path)
    {
        Blueprint blueprint = (Template)Activator.CreateInstance(Template.GetType(), Template.Details.SetItem(Template.DetailOption.Path, path))!;
        Model model = (Model)Activator.CreateInstance(blueprint.ModelType, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { blueprint }, null)!;
        foreach (Node node in model.Tree)
        {
            node.Invoke(node.Model.Activities[FileSystemTemplate.ActivityOption.Mount]);
        }
        return model;
    }
}
