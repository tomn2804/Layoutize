namespace Schemata
{
    public class PriorityProperty : DefaultProperty<int>
    {
        public PriorityProperty(Model model)
                    : base(model)
        {
        }

        public PriorityProperty(Model model, int value)
            : base(model, value)
        {
        }

        public override string Key => "Priority";

        protected override int GetDefaultValue()
        {
            return 0;
        }
    }
}
