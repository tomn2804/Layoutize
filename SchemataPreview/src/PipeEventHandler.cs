using System;
using System.Management.Automation;

namespace SchemataPreview
{
    public class PipeEventArgs : EventArgs
    {
    }

    public class PipeEventHandler
    {
        public PipeEventHandler(Model model)
        {
            Model = model;
        }

        public static PipeEventHandler operator +(PipeEventHandler lhs, ScriptBlock rhs)
        {
            return lhs += (pipe, args) => lhs.Model.CopyClosureTo(rhs, pipe, args).Invoke();
        }

        public static PipeEventHandler operator +(PipeEventHandler lhs, Action<Pipe, EventArgs> rhs)
        {
            lhs.Action += rhs;
            return lhs;
        }

        public void Invoke(Pipe segment, EventArgs args)
        {
            Action?.Invoke(segment, args);
        }

        protected Action<Model, EventArgs>? Action { get; set; }
        protected Model Model { get; }
    }
}
