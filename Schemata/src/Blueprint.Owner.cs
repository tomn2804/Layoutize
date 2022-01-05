using System;
using System.Diagnostics;

namespace Schemata;

public sealed partial class Blueprint
{
    public abstract partial class Owner
    {
        public event EventHandler? BlueprintUpdated;

        protected Owner(Blueprint blueprint)
        {
            _blueprint = blueprint;
            foreach (Template template in Blueprint.Templates)
            {
                template.DetailsUpdating += UpdateBlueprint;
            }
        }

        protected Blueprint Blueprint
        {
            get => _blueprint;
            private set
            {
                _blueprint = value;
                OnBlueprintUpdated(EventArgs.Empty);
            }
        }

        protected virtual void OnBlueprintUpdated(EventArgs args)
        {
            BlueprintUpdated?.Invoke(this, args);
        }

        private Blueprint _blueprint;

        private void UpdateBlueprint(object? sender, Template.DetailsUpdatingEventArgs args)
        {
            Blueprint newBlueprint = (Template)Activator.CreateInstance(sender!.GetType(), args.Details)!;
            Builder builder = newBlueprint.ToBuilder();

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
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool _)
        {
            foreach (Template template in Blueprint.Templates)
            {
                template.DetailsUpdating -= UpdateBlueprint;
            }
        }
    }
}
