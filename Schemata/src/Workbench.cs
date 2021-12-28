using System;
using System.Reflection;

namespace Schemata;

public sealed class Workbench
{
    public Workbench(Blueprint blueprint)
    {
        Blueprint = blueprint;
    }

    private Blueprint Blueprint { get; }

    public Model BuildTo(string path)
    {
        Model model = (Model)Activator.CreateInstance(Blueprint.ModelType, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { path, Blueprint }, null)!;
        foreach (Node node in model.Tree)
        {
            node.Invoke(node.Model.Activities[Model.Activity.Mount]);
        }
        return model;
    }
}
