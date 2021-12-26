using System;
using System.Diagnostics;
using System.Linq;

namespace Schemata;

public partial class Blueprint
{
    public abstract partial class Owner
    {
        protected Owner(Blueprint blueprint)
        {
            Blueprint = blueprint;
            foreach (Template template in Blueprint.Templates)
            {
                template.DetailsUpdating += UpdateBlueprint;
            }
        }

        protected Blueprint Blueprint { get; private set; }

        protected virtual void UpdateBlueprint(object? sender, Template.DetailsUpdatingEventArgs args)
        {
            Builder builder = new((Template)Activator.CreateInstance(sender!.GetType(), args.Details)!);

            Debug.Assert(Blueprint.Templates[builder.Templates.Count - 1] == sender);

            for (int i = 0; i < builder.Templates.Count; ++i)
            {
                Template oldTemplate = Blueprint.Templates[i];
                Template newTemplate = builder.Templates[i];

                Debug.Assert(oldTemplate != newTemplate);

                oldTemplate.DetailsUpdating -= UpdateBlueprint;
                newTemplate.DetailsUpdating += UpdateBlueprint;
            }

            for (int i = builder.Templates.Count; i < Blueprint.Templates.Count; ++i)
            {
                builder.Templates.Add(Blueprint.Templates[i]);
            }

            Blueprint = builder.ToBlueprint();
        }
    }

    public abstract partial class Owner : IDisposable
    {
        public virtual void Dispose()
        {
            foreach (Template template in Blueprint.Templates)
            {
                template.DetailsUpdating -= UpdateBlueprint;
            }
            GC.SuppressFinalize(this);
        }
    }
}
