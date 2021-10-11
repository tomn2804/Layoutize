using System;
using System.IO;

namespace SchemataPreview
{
    public abstract partial class Model
    {
        public string FullName => System.IO.Path.Combine(Parent?.FullName ?? Path ?? throw new ArgumentNullException(), Name);
        public string Name { get; }
        public Model? Parent { get; }
        public int Priority { get; }
        public string RelativeName => System.IO.Path.Combine(Parent?.RelativeName ?? string.Empty, Name);

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
            Name = new NameProperty(this);
            Parent = new ParentProperty(this);
            Path = new PathProperty(this);
            Priority = new PriorityProperty(this);
        }

        protected Schema Schema { get; }
        private string? Path { get; }
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
