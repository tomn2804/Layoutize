using System;
using System.Diagnostics;

namespace Templatize.Templates;

public sealed partial class Layout
{
    public abstract partial class Holder
    {
        public event EventHandler? LayoutUpdated;

        protected Holder(Layout layout)
        {
            _layout = layout;
            foreach (Template template in Layout.Templates)
            {
                template.AttributesUpdating += UpdateLayout;
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

        private void UpdateLayout(object? sender, Template.AttributesUpdatingEventArgs args)
        {
            Layout newLayout = (Template)Activator.CreateInstance(sender!.GetType(), args.Attributes)!;
            Builder builder = newLayout.ToBuilder();

            Debug.Assert(Layout.Templates[builder.Templates.Count - 1] == sender);

            for (int i = 0; i < builder.Templates.Count; ++i)
            {
                Template oldTemplate = Layout.Templates[i];
                Template newTemplate = builder.Templates[i];

                Debug.Assert(oldTemplate != newTemplate);

                oldTemplate.AttributesUpdating -= UpdateLayout;
                newTemplate.AttributesUpdating += UpdateLayout;
            }

            for (int i = builder.Templates.Count; i < Layout.Templates.Count; ++i)
            {
                builder.Templates.Add(Layout.Templates[i]);
            }

            Layout = builder.ToLayout();
        }
    }

    public abstract partial class Holder : IDisposable
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
                template.AttributesUpdating -= UpdateLayout;
            }
        }
    }
}
