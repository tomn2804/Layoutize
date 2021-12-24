using System;
using System.Diagnostics;

namespace Schemata;

public partial class Blueprint
{
    public abstract partial class Owner
    {
        protected Blueprint Blueprint { get; private set; }

        protected Owner(Blueprint blueprint)
        {
            Blueprint = blueprint;
            foreach (Template template in Blueprint.Templates)
            {
                template.DetailsUpdating += UpdateBlueprint;
            }
        }

        protected virtual void UpdateBlueprint(object? sender, Template.DetailsUpdatingEventArgs args)
        {
            Blueprint newBlueprint = (Template)Activator.CreateInstance(sender!.GetType(), args.Details)!;

            Debug.Assert(newBlueprint.Templates.Count - 1 == Blueprint.Templates.IndexOf((Template)sender));

            for (int i = 0; i < newBlueprint.Templates.Count; ++i)
            {
                Template oldTemplate = Blueprint.Templates[i];
                Template newTemplate = newBlueprint.Templates[i];

                Debug.Assert(oldTemplate != newTemplate);

                oldTemplate.DetailsUpdating -= UpdateBlueprint;
                newTemplate.DetailsUpdating += UpdateBlueprint;
            }

            for (int i = newBlueprint.Templates.Count; i < Blueprint.Templates.Count; ++i)
            {
                newBlueprint.Templates.Add(Blueprint.Templates[i]);
            }

            Blueprint = newBlueprint;
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
