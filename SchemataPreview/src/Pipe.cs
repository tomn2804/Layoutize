using System;

namespace SchemataPreview
{
	public class Pipe
	{
		public Action<PipeSegment>? OnProcessed { get; set; }
		public Action<PipeSegment>? OnProcessing { get; set; }
	}
}
