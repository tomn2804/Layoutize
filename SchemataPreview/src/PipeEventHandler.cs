using System;
using System.Management.Automation;

namespace SchemataPreview
{
	public class PipeEventHandler
	{
		public PipeEventHandler(Model model)
		{
			Model = model;
		}

		public static PipeEventHandler operator +(PipeEventHandler lhs, ScriptBlock rhs)
		{
			return lhs += (segment, args) => lhs.Model.CaptureContext(rhs, segment, args).Invoke();
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

		protected Action<Pipe, EventArgs>? Action { get; set; }
		protected Model Model { get; }
	}
}
