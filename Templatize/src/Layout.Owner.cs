using System;
using System.Diagnostics;

namespace Templatize;

public sealed partial class Layout
{
    public abstract partial class Owner
    {
        public event EventHandler? LayoutUpdated;

        protected Owner(Layout layout)
        {
            _layout = layout;
            foreach (Template template in Layout.Templates)
            {
                template.DetailsUpdating += UpdateLayout;
            }
        }

        protected Layout Layout
        {
            get => _layout;
            private set
            {
                _layout = value;
                OnLayoutUpdated(EventArgs.Empty);
            }
        }

        protected virtual void OnLayoutUpdated(EventArgs args)
        {
            LayoutUpdated?.Invoke(this, args);
        }

        private Layout _layout;

        private void UpdateLayout(object? sender, Template.DetailsUpdatingEventArgs args)
        {
            Layout newLayout = (Template)Activator.CreateInstance(sender!.GetType(), args.Details)!;
            Builder builder = newLayout.ToBuilder();

            Debug.Assert(Layout.Templates[builder.Templates.Count - 1] == sender);

            for (int i = 0; i < builder.Templates.Count; ++i)
            {
                Template oldTemplate = Layout.Templates[i];
                Template newTemplate = builder.Templates[i];

                Debug.Assert(oldTemplate != newTemplate);

                oldTemplate.DetailsUpdating -= UpdateLayout;
                newTemplate.DetailsUpdating += UpdateLayout;
            }

            for (int i = builder.Templates.Count; i < Layout.Templates.Count; ++i)
            {
                builder.Templates.Add(Layout.Templates[i]);
            }

            Layout = builder.ToContext();
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
            foreach (Template template in Layout.Templates)
            {
                template.DetailsUpdating -= UpdateLayout;
            }
        }
    }
}
