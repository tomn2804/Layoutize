namespace SchemataPreview
{
    public class PathProperty : NullableProperty<string>
    {
        public PathProperty(ImmutableDefinition definition)
            : base(definition)
        {
        }

        public override string Key => "Path";
    }
}
