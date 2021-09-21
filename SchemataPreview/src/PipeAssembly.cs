using System.Collections.Generic;

namespace SchemataPreview
{
	public class PipeAssembly
	{
		public Pipe this[object key] { get => KeyToPipe[key]; set => KeyToPipe[key] = value; }

		public bool Contains(object key)
		{
			return KeyToPipe.ContainsKey(key);
		}

		public Pipe Register(object key)
		{
			Pipe value = new();
			KeyToPipe.Add(key, value);
			return value;
		}

		public void Unregister(object key)
		{
			KeyToPipe.Remove(key);
		}

		protected Dictionary<object, Pipe> KeyToPipe { get; } = new();
	}
}
