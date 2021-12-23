using System;
using System.Diagnostics;

namespace Schemata;

public partial class Blueprint
{
    public abstract class Owner : IDisposable
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

        protected virtual void UpdateBlueprint(object? sender, DetailsUpdatingEventArgs args)
        {
            Blueprint newBlueprint = (Template)Activator.CreateInstance(sender!.GetType(), args.NewDetails)!;

            int senderIndex = Blueprint.Templates.IndexOf((Template)sender);
            Debug.Assert(newBlueprint.Templates.Count - 1 == senderIndex);

            for (int i = 0; i <= senderIndex; ++i)
            {
                Template oldTemplate = Blueprint.Templates[i];
                Template newTemplate = newBlueprint.Templates[i];

                Debug.Assert(oldTemplate != newTemplate);

                oldTemplate.DetailsUpdating -= UpdateBlueprint;
                newTemplate.DetailsUpdating += UpdateBlueprint;
            }

            for (int i = senderIndex + 1; i < Blueprint.Templates.Count; ++i)
            {
                newBlueprint.Templates.Add(Blueprint.Templates[i]);
            }

            Blueprint = newBlueprint;
        }

        public void Dispose()
        {
            foreach (Template template in Blueprint.Templates)
            {
                template.DetailsUpdating -= UpdateBlueprint;
            }
        }
    }
}
