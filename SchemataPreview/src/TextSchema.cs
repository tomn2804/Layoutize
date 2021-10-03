using System;
using System.Collections;

namespace SchemataPreview
{
    public class TextSchema : Schema<TextModel>
    {
        public TextSchema(Definition definition)
            : base(definition)
        {
        }

        protected override Schema Build()
        {
            Definition definition = new();
            if (Definition.TryGetValue("Contents", out object? @object))
            {
                switch (@object)
                {
                    case string line:
                        definition["OnCreated"] = (Action<TextModel>)((model) =>
                        {
                            model.Contents = new string[] { line };
                            ((Action<Model>?)Definition["OnCreated"])?.Invoke(model);
                        });
                        break;

                    case string[] lines:
                        definition["OnCreated"] = (Action<TextModel>)((model) =>
                        {
                            model.Contents = lines;
                            ((Action<Model>?)Definition["OnCreated"])?.Invoke(model);
                        });
                        break;
                }
            }
            definition.Add(DefinitionOperator.Spread, definition);
            return new FileSchema(definition);
        }
    }
}
