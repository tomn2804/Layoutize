using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Automation;

namespace SchemataPreview
{
	public partial class Schema
	{
		public partial class Builder
		{
			private Schema CurrentModel { get; set; }

			public void AddEventListener(string eventName, ScriptBlock callback)
			{
				Debug.Assert(CurrentModel != null);
				AddEventListener(eventName, () => callback.(null, new List<PSVariable>() { new PSVariable("this", CurrentModel) }));
			}

			public void AddEventListener(string eventName, Action callback)
			{
				Debug.Assert(CurrentModel != null);
				List<Action> callbacks;
				if (CurrentModel.EventToCallbacks.TryGetValue(eventName, out callbacks))
				{
					callbacks.Add(callback);
				}
				else
				{
					CurrentModel.EventToCallbacks.Add(eventName, new List<Action>() { callback });
				}
			}
		}

		public partial class Builder
		{
			public Schema AddChildren(params Schema[] models)
			{
				Debug.Assert(CurrentModel != null);
				foreach (Schema model in models)
				{
					if (model.IsMounted)
					{
						Dismount(model);
					}
					model.Parent = CurrentModel;
					CurrentModel.Children.RemoveAll(child => child.Name == model.Name);
					CurrentModel.Children.Add(model);
				}
				return CurrentModel;
			}
		}

		public partial class Builder
		{
			public Schema Mount(string path, Schema model)
			{
				model.FullName = Path.GetPathRoot(path) == model.Name ? model.Name : Path.Combine(path, model.Name);
				model.Parent = null;
				Mount(model);
				return model;
			}

			private void Build(Schema model)
			{
				CurrentModel = model;
				model.Build(this);
				model.PSBuild.(null, new List<PSVariable>() { new PSVariable("_", this) }, new PSVariable("this", CurrentModel));
			}

			private void Mount(Schema model)
			{
				Build(model);
				if (model.Exists)
				{
					if (model.UseHardMount)
					{
						model.InvokeEvent(EventOption.Delete);
						model.InvokeEvent(EventOption.Create);
					}
				}
				else
				{
					model.InvokeEvent(EventOption.Create);
				}
				foreach (Schema child in model.Children)
				{
					child.FullName = Path.Combine(model.FullName, child.Name);
					Mount(child);
				}
				if (!model.IsMounted)
				{
					model.IsMounted = true;
					model.InvokeEvent(EventOption.PostMount);
				}
			}
		}

		public partial class Builder
		{
			public void Dismount(Schema model)
			{
				if (model.IsMounted)
				{
					CurrentModel = model;
					model.InvokeEvent(EventOption.PreDismount);
					model.FullName = null;
					model.Parent = null;
					model.IsMounted = false;
				}
				foreach (Schema child in model.Children)
				{
					Dismount(child);
				}
			}
		}
	}
}
