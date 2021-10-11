using System;
using System.Collections;

namespace SchemataPreview
{
    public class FileSchema : Schema<FileModel>
    {
        public FileSchema(ImmutableProps props)
            : base(props)
        {
        }

        protected override Schema Build()
        {
            ImmutableProps.Builder props = new();
            props["OnCreated"] = (Action<FileModel>)((model) =>
            {
                model.Create();
                ((Action<Model>?)props["OnCreated"])?.Invoke(model);
            });
            props["OnCreated"] = (Action<FileModel>)((model) =>
            {
                model.Delete();
                ((Action<Model>?)props["OnCreated"])?.Invoke(model);
            });
            props.Add(PropsOperator.Spread, props);
            return new FileSchema(props);
        }
    }
}
