using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace SchemataPreview
{
    public partial class ModelSet
    {
        public ModelSet(Model parent, params Schema[] schemata)
        {
            Parent = parent;
            foreach (Schema schema in schemata)
            {
                Schema s = (Schema)Activator.CreateInstance(schema.ModelType, schema.Props.SetItem("Parent", parent)).AssertNotNull();
                Models.Add(s.CreateModel());
            }
        }

        public int Count => Models.Count;
        public Model Parent { get; }
        public Model this[string name] => Models.First(model => model.Name == name);

        public void Add(params Schema[] schemata)
        {
            SortedSet<Model> models = new(new ModelComparer());
            foreach (Schema schema in schemata)
            {
                Model child = (Model)Activator.CreateInstance(schema.ModelType, schema.Props.SetItem("Parent", Parent)).AssertNotNull();
                if (Models.Add(child))
                {
                    models.Add(child);
                }
            }
            switch (Parent.Traversal)
            {
                case PipelineTraversalOption.PostOrder:
                    PipelineSequential.TraverseReversePostOrder(PipeOption.Mount, models);
                    break;

                case PipelineTraversalOption.PreOrder:
                default:
                    PipelineSequential.TraverseReversePreOrder(PipeOption.Mount, models);
                    break;
            }
        }

        public void Add<T>(params string[] patterns) where T : Schema
        {
            Add<T, T>(patterns);
        }

        public void Add<TDirectory, TFile>(params string[] patterns) where TDirectory : Schema where TFile : Schema
        {
            List<Schema> children = new();
            foreach (string pattern in patterns)
            {
                foreach (string path in Directory.GetDirectories(Parent, pattern))
                {
                    Schema s = (Schema)Activator.CreateInstance(typeof(TDirectory), new ImmutableProps.Builder { { "Name", Path.GetFileName(path) } }).AssertNotNull();
                    children.Add(s);
                }
                foreach (string path in Directory.GetFiles(Parent, pattern))
                {
                    Schema s = (Schema)Activator.CreateInstance(typeof(TFile), new ImmutableProps.Builder { { "Name", Path.GetFileName(path) } }).AssertNotNull();
                    children.Add(s);
                }
            }
            Add(children.ToArray());
        }

        public bool Contains(Model model)
        {
            return Models.Contains(model);
        }

        public bool ContainsName(string name)
        {
            return Models.Any(model => model.Equals(name));
        }

        public void Remove(Model model)
        {
            (new Pipeline(model)).Invoke(PipeOption.Delete);
            Models.Remove(model);
        }

        public void RemoveByName(string name)
        {
            Remove(Models.First(model => model.Equals(name)));
        }

        protected SortedSet<Model> Models { get; } = new(new ModelComparer());
    }

    public partial class ModelSet : IEnumerable<Model>
    {
        public IEnumerator<Model> GetEnumerator()
        {
            return Models.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Models.GetEnumerator();
        }
    }
}
