using System;
using System.Reflection;

namespace Layoutize.Views;

public partial class View
{
    public static class Factory
    {
        //public Factory(Layout layout)
        //{
        //    Layout = layout;
        //}

        public static View Build(Layout layout)
        {
            View view = (View)Activator.CreateInstance(Layout.ViewType, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { Layout }, null)!;
            foreach (Node node in view.Tree)
            {
                node.Invoke(node.View.Activities[ActivityOption.Mount]);
            }
            view.IsMounted = true;
            return view;
        }

        public static View BuildTo(Layout layout, string path)
        {
            View view = FillTo(path);
            foreach (Node node in view.Tree)
            {
                node.Invoke(node.View.Activities[ActivityOption.Mount]);
            }
            view.IsMounted = true;
            return view;
        }

        //public View FillTo(string path)
        //{
        //    if (string.IsNullOrWhiteSpace(path))
        //    {
        //        throw new ArgumentNullException(nameof(path), $"'{nameof(path)}' cannot be null or containing only white spaces.");
        //    }
        //    if (path.IndexOfAny(System.IO.Path.GetInvalidPathChars()) != -1)
        //    {
        //        throw new ArgumentException($"'{nameof(path)}' cannot contain invalid system characters.", nameof(path));
        //    }
        //    Layout.Builder builder = Layout.ToBuilder();
        //    builder.Path = path;
        //    View view = (View)Activator.CreateInstance(Layout.ViewType, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { builder.ToBlueprint() }, null)!;
        //    return view;
        //}

        //public View FillTo(DirectoryView parent)
        //{
        //    View child = FillTo(parent.FullName);
        //    child.Parent = parent;
        //    return child;
        //}

        //private Layout Layout { get; }
    }
}
