namespace SchemataPreview
{
    public class ParentProperty : NullableProperty<Model?>
    {
        public ParentProperty(Model model)
            : base(model)
        {
        }

        public ParentProperty(Model model, Model? value)
            : base(model, value)
        {
        }

        public override string Key => "Parent";
    }
}
