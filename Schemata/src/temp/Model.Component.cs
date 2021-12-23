namespace Schemata
{
    public abstract partial class Model
    {
        public abstract class Component
        {
            protected Component(Model model)
            {
                Model = model;
            }

            protected Model Model { get; }
            protected Schema Schema => Model.Schema;
        }
    }
}
