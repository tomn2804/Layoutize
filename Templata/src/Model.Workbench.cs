using System;
using System.Reflection;

namespace Templata;

public partial class Model
{
    public sealed class Workbench
    {
        public Workbench(Blueprint blueprint)
        {
            Blueprint = blueprint;
        }

        public Model Build()
        {
            Model model = (Model)Activator.CreateInstance(Blueprint.ModelType, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { Blueprint }, null)!;
            foreach (Node node in model.Tree)
            {
                node.Invoke(node.Model.Activities[ActivityOption.Mount]);
            }
            model.IsMounted = true;
            return model;
        }

        public Model BuildTo(string path)
        {
            Model model = FillTo(path);
            foreach (Node node in model.Tree)
            {
                node.Invoke(node.Model.Activities[ActivityOption.Mount]);
            }
            model.IsMounted = true;
            return model;
        }

        public Model FillTo(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path), $"'{nameof(path)}' cannot be null or containing only white spaces.");
            }
            if (path.IndexOfAny(System.IO.Path.GetInvalidPathChars()) != -1)
            {
                throw new ArgumentException($"'{nameof(path)}' cannot contain invalid system characters.", nameof(path));
            }
            Blueprint.Builder builder = Blueprint.ToBuilder();
            builder.Path = path;
            Model model = (Model)Activator.CreateInstance(Blueprint.ModelType, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { builder.ToBlueprint() }, null)!;
            return model;
        }

        public Model FillTo(DirectoryModel parent)
        {
            Model child = FillTo(parent.FullName);
            child.Parent = parent;
            return child;
        }

        private Blueprint Blueprint { get; }
    }
}
