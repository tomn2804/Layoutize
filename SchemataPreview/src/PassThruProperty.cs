namespace SchemataPreview
{
    public class PassThruProperty : DefaultProperty<bool>
    {
        public PassThruProperty(Model model)
            : base(model)
        {
        }

        public PassThruProperty(Model model, bool value)
            : base(model, value)
        {
        }

        public override string Key => "PassThru";

        protected override bool GetDefaultValue()
        {
            return false;
        }
    }
}
