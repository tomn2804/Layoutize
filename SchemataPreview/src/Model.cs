using System;
using System.IO;

namespace SchemataPreview
{
    public abstract partial class Model
    {
        public string Name { get; }
        public Model? Parent { get; }
        public int Priority { get; }

        public string FullName => Path.Combine(Parent?.FullName ?? _path ?? throw new ArgumentNullException(), Name);
        public string RelativeName => Path.Combine(Parent?.RelativeName ?? string.Empty, Name);

        public static implicit operator string(Model @this)
        {
            return @this.FullName;
        }

        public override string ToString()
        {
            return FullName;
        }

        protected Model(Schema schema)
        {
            Schema = schema;
            //PipeAssembly.Register("Create", Schema.Definition["OnCreating"], Schema.Definition["OnCreated"]);

            Name = (string)Activator.CreateInstance(NameType, Schema.definition).AssertNotNull();
            Parent = (Model?)Activator.CreateInstance(ParentType, Schema.definition);
            _path = (string?)Activator.CreateInstance(PathType, Schema.definition);
            Priority = (int)Activator.CreateInstance(PriorityType, Schema.definition).AssertNotNull();
        }

        //protected PipeAssembly PipeAssembly { get; }
        protected Schema Schema { get; }

        protected virtual Type NameType { get; } = typeof(NameProperty);
        protected virtual Type ParentType { get; } = typeof(ParentProperty);
        protected virtual Type PathType { get; } = typeof(PathProperty);
        protected virtual Type PriorityType { get; } = typeof(PriorityProperty);

        private string? _path;
    }

    public abstract partial class Model : IComparable<Model>
    {
        public int CompareTo(Model? other)
        {
            if (other is not null)
            {
                if ((Priority != 0) || (other.Priority != 0))
                {
                    return other.Priority.CompareTo(Priority);
                }
                return Name.CompareTo(other.Name);
            }
            return 1;
        }
    }
}
