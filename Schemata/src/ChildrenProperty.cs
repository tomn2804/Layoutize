using System;

namespace Schemata
{
    public class ChildrenProperty : DefaultProperty<ModelSet>
    {
        public ChildrenProperty(Model model)
            : base(model)
        {
        }

        public ChildrenProperty(Model model, ModelSet value)
            : base(model, value)
        {
        }

        public override string Key => "Children";

        protected override ModelSet GetDefaultValue()
        {
            return new(Model);
        }

        protected override ModelSet? GetValue()
        {
            if (Schema.Outline.TryGetValue(Key, out object? @object))
            {
                switch (@object)
                {
                    case object[]:
                        return new(Model, (Schema[])@object);

                    case object:
                        return new(Model, new Schema[] { (Schema)@object });

                    default:
                        throw new ArgumentException($"Outline property value at key '{Key}' must be of type 'Schema[]'. Recieved type: '{@object.AssertNotNull().GetType()}'.", Key);
                }
            }
            return default;
        }
    }
}
