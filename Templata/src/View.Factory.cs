using System;
using System.Reflection;

namespace Templata;

public partial class View
{
    public static class Factory
    {
        //public Factory(Context context)
        //{
        //    Context = context;
        //}

        public static View Build(Context context)
        {
            View view = (View)Activator.CreateInstance(Context.ViewType, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { Context }, null)!;
            foreach (Node node in view.Tree)
            {
                node.Invoke(node.View.Activities[ActivityOption.Mount]);
            }
            view.IsMounted = true;
            return view;
        }

        public static View BuildTo(Context context, string path)
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
        //    Context.Builder builder = Context.ToBuilder();
        //    builder.Path = path;
        //    View view = (View)Activator.CreateInstance(Context.ViewType, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { builder.ToBlueprint() }, null)!;
        //    return view;
        //}

        //public View FillTo(DirectoryView parent)
        //{
        //    View child = FillTo(parent.FullName);
        //    child.Parent = parent;
        //    return child;
        //}

        //private Context Context { get; }
    }
}
