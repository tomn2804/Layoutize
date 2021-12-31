using System;
using System.IO;
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
        Blueprint.Builder builder = Blueprint.ToBuilder();
        builder.Path = path;
        Model model = (Model)Activator.CreateInstance(Blueprint.ModelType, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { builder.ToBlueprint() }, null)!;
        foreach (Node node in model.Tree)
        {
            node.Invoke(node.Model.Activities[Model.DefaultActivity.Mount]);
        }
        return model;
    }
}
