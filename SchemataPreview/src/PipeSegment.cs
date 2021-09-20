using System;
using System.Collections.Generic;

namespace SchemataPreview
{
	public class PipeSegment
	{
		public void Extend(Pipe pipe)
		{
			if (pipe.OnProcessed != null)
			{
				Callbacks.Push(pipe.OnProcessed);
			}
			pipe.OnProcessing?.Invoke(this);
		}

		public void Flush()
		{
			while (Callbacks.Count != 0)
			{
				Callbacks.Pop().Invoke(this);
			}
		}

		protected Stack<Action<PipeSegment>> Callbacks { get; } = new();
	}
}
