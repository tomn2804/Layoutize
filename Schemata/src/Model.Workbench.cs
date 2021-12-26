using System;
using System.Reflection;

namespace Schemata;

public abstract partial class Model
{
    public sealed class Workbench
    {
        public Workbench(string path)
        {
            WorkingDirectoryPath = path;
        }

        public string WorkingDirectoryPath { get; }

        public Model Build(Blueprint blueprint)
        {
            return (Model)Activator.CreateInstance(blueprint.ModelType, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { blueprint }, null)!;
        }
    }
}
