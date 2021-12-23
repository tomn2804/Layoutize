using System;
using System.Collections;

namespace Schemata
{
    public class TextSchema : Schema<TextModel>
    {
        public TextSchema(ImmutableOutline outline)
            : base(outline)
        {
        }

        protected override Schema Build()
        {
            ImmutableOutline.Builder outline = new();
            if (Outline.TryGetValue("Contents", out object? @object))
            {
                switch (@object)
                {
                    case string line:
                        outline["OnCreated"] = (Action<TextModel>)((model) =>
                        {
                            model.Contents = new string[] { line };
                            ((Action<Model>?)Outline["OnCreated"])?.Invoke(model);
                        });
                        break;

                    case string[] lines:
                        outline["OnCreated"] = (Action<TextModel>)((model) =>
                        {
                            model.Contents = lines;
                            ((Action<Model>?)Outline["OnCreated"])?.Invoke(model);
                        });
                        break;
                }
            }
            return new FileSchema(outline.MergeTo(Outline));
        }
    }
}
