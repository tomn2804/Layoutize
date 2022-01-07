using System;
using System.Diagnostics;

namespace Templata;

public sealed partial class Context
{
    public abstract partial class Owner
    {
        public event EventHandler? BlueprintUpdated;

        protected Owner(Context context)
        {
            _blueprint = context;
            foreach (Template template in Context.Templates)
            {
                template.DetailsUpdating += UpdateBlueprint;
            }
        }

        protected Context Context
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

        private Context _blueprint;

        private void UpdateBlueprint(object? sender, Template.DetailsUpdatingEventArgs args)
        {
            Context newBlueprint = (Template)Activator.CreateInstance(sender!.GetType(), args.Details)!;
            Builder builder = newBlueprint.ToBuilder();

            Debug.Assert(Context.Templates[builder.Templates.Count - 1] == sender);

            for (int i = 0; i < builder.Templates.Count; ++i)
            {
                Template oldTemplate = Context.Templates[i];
                Template newTemplate = builder.Templates[i];

                Debug.Assert(oldTemplate != newTemplate);

                oldTemplate.DetailsUpdating -= UpdateBlueprint;
                newTemplate.DetailsUpdating += UpdateBlueprint;
            }

            for (int i = builder.Templates.Count; i < Context.Templates.Count; ++i)
            {
                builder.Templates.Add(Context.Templates[i]);
            }

            Context = builder.ToBlueprint();
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
            foreach (Template template in Context.Templates)
            {
                template.DetailsUpdating -= UpdateBlueprint;
            }
        }
    }
}
