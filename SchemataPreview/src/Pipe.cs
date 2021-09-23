using System;
using System.Collections.Generic;

namespace SchemataPreview
{
	public class Pipe
	{
		public Pipe(Model model)
		{
			Model = model;
		}

		public bool Extend(object key)
		{
			if (Model.PipeAssembly.TryGetValue(key, out PipeSegment? segment))
			{
				Extend(segment);
				return Model.Children != null;
			}
			return Model.PassThru && (Model.Children != null);
		}

		public void Extend(PipeSegment pipe)
		{
			if (pipe.OnProcessed != null)
			{
				Callbacks.Push(pipe.OnProcessed);
			}
			pipe.OnProcessing.Invoke(this, EventArgs.Empty);
		}

		public void Flush()
		{
			while (Callbacks.Count != 0)
			{
				Callbacks.Pop().Invoke(this, EventArgs.Empty);
			}
		}

		protected Stack<PipeEventHandler> Callbacks { get; } = new();
		protected Model Model { get; }
	}
}
