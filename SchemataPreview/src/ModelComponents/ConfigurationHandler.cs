using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace SchemataPreview
{
	internal class ConfigurationHandler : Handler
	{
		public ConfigurationHandler(Model model)
			: base(model)
		{
		}

		private List<Action> Callbacks { get; } = new();

		public void Add(ScriptBlock callback)
		{
			Add(() => callback.InvokeWithContext(null, new List<PSVariable>() { new PSVariable("this", Model) }));
		}

		public void Add(Action callback)
		{
			Callbacks.Add(callback);
		}

		public void Invoke()
		{
			foreach (Action callback in Callbacks)
			{
				callback();
			}
		}
	}
}
