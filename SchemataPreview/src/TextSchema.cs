using System;
using System.Collections;

namespace SchemataPreview
{
    public class TextDefinition : Schema<TextModel>
    {
        public TextDefinition(ImmutableProps props)
            : base(props)
        {
        }

        protected override Schema Build()
        {
            ImmutableProps.Builder props = new();
            if (Props.TryGetValue("Contents", out object? @object))
            {
                switch (@object)
                {
                    case string line:
                        props["OnCreated"] = (Action<TextModel>)((model) =>
                        {
                            model.Contents = new string[] { line };
                            ((Action<Model>?)Props["OnCreated"])?.Invoke(model);
                        });
                        break;

                    case string[] lines:
                        props["OnCreated"] = (Action<TextModel>)((model) =>
                        {
                            model.Contents = lines;
                            ((Action<Model>?)Props["OnCreated"])?.Invoke(model);
                        });
                        break;
                }
            }
            props.Add(PropsOperator.Spread, props);
            return new FileSchema(props);
        }
    }
}
