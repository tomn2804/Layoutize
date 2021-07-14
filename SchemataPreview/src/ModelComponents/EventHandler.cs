using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace SchemataPreview
{
	internal class EventHandler : Handler
	{
		public EventHandler(Model model)
			: base(model)
		{
		}

		private Dictionary<string, List<Action>> EventToCallbacks { get; } = new();

		public void Add(string type, ScriptBlock callback)
		{
			Add(type, () => callback.InvokeWithContext(null, new List<PSVariable>() { new PSVariable("this", Model) }));
		}

		public void Add(string type, Action callback)
		{
			List<Action> callbacks;
			if (EventToCallbacks.TryGetValue(type, out callbacks))
			{
				callbacks.Add(callback);
			}
			else
			{
				EventToCallbacks.Add(type, new List<Action>() { callback });
			}
		}

		public void Invoke(string type)
		{
			foreach (Action callback in EventToCallbacks[type])
			{
				callback();
			}
		}
	}
}
