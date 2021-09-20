using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Linq;

namespace SchemataPreview
{
	public class EventHandler
	{
		public EventHandler(Model model)
		{
			Model = model;
		}

		public List<Func<Action>> this[string key]
		{
			get
			{
				List<Func<Action>>? callbacks;
				if (!Handler.TryGetValue(key, out callbacks))
				{
					callbacks = new();
					Handler.Add(key, callbacks);
				}
				return callbacks;
			}
		}

		public void Add(string key, Action callback)
		{
			Handler[key].Add(() =>
			{
				callback.Invoke();
				return () => { };
			});
		}

		public void Add(string key, Func<Action> callback)
		{
			Handler[key].Add(callback);
		}

		public void Add(string key, ScriptBlock callback)
		{
			Handler[key].Add(() =>
			{
				PSObject obj = callback.InvokeWithContext(null, new List<PSVariable>() { new PSVariable("this", Model.Schema), new PSVariable("_", Model) }).Last();
				if (obj.BaseObject is ScriptBlock f)
				{
					return () => { f.InvokeWithContext(null, new List<PSVariable>() { new PSVariable("this", Model.Schema), new PSVariable("_", Model) }); };
				}
				return () => { };
			});
		}

		protected Dictionary<string, List<Func<Action>>> Handler { get; } = new();
		protected Model Model { get; init; }
	}
}
