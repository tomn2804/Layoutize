namespace SchemataPreview
{
    public class PathProperty : NullableProperty<string>
    {
        public PathProperty(Model model)
            : base(model)
        {
        }

        public PathProperty(Model model, string value)
            : base(model, value)
        {
        }

        public override string Key => "Path";
    }
}
