using System;
using System.Collections.Generic;

namespace SchemataPreview
{
	public class Pipe
	{
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
	}
}
