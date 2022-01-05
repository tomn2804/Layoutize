using System;
using System.IO;
using System.Reflection;

namespace Schemata;

public partial class Model
{
    public sealed class Workbench
    {
        public Workbench(Template template)
        {
            Template = template;
        }

        public Model Build()
        {
            return BuildTo(Directory.GetCurrentDirectory());
        }

        public Model BuildTo(string path)
        {
            Model model = FillTo(path);
            foreach (Node node in model.Tree)
            {
                if (node.Model.Activities.TryGetValue(FileSystemTemplate.ActivityOption.Mount, out Activity? activity))
                {
                    node.Invoke(activity!);
                }
            }
            return model;
        }

        public Model FillTo(string path)
        {
            Blueprint blueprint = (Template)Activator.CreateInstance(Template.GetType(), Template.Details.SetItem(Template.DetailOption.Path, path))!;
            Model model = (Model)Activator.CreateInstance(blueprint.ModelType, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { blueprint }, null)!;
            return model;
        }

        public Model FillTo(DirectoryModel parent)
        {
            Model child = FillTo(parent.FullName);
            child.Parent = parent;
            return child;
        }

        private Template Template { get; }
    }
}
