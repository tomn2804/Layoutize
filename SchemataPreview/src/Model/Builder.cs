using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;

namespace SchemataPreview
{
	public partial class Model
	{
		public partial class Builder
		{
			public void AddEventListener(string eventName, ScriptBlock callback)
			{
				AddEventListener(eventName, () => callback.InvokeWithContext(null, new List<PSVariable>() { new PSVariable("this", CurrentModel) }));
			}

			public void AddEventListener(string eventName, Action callback)
			{
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

			private void ClearEvent()
			{
				CurrentModel.EventToCallbacks = new();
			}
		}

		public partial class Builder
		{
			public void AddChildren(params Model[] models)
			{
				foreach (Model model in models)
				{
					if (model.IsMounted)
					{
						Dismount(model);
					}
					Bind(CurrentModel, model);
				}
			}

			private static void Bind(Model parent, Model child)
			{
				child.FullName = Path.Combine(parent.FullName, child.Name);
				child.Parent = parent;
				parent.Children.RemoveAll(child => child.Name == child.Name);
				parent.Children.Add(child);
			}
		}

		public partial class Builder
		{
			private Model CurrentModel { get; set; }

			public Model Mount(string path, Model model)
			{
				CurrentModel = model;
				CurrentModel.FullName = Path.GetPathRoot(path) == CurrentModel.Name ? CurrentModel.Name : Path.Combine(path, CurrentModel.Name);
				CurrentModel.Parent = null;
				ClearEvent();
				CurrentModel.Build(this);
				CurrentModel.PSBuild?.InvokeWithContext(null, new List<PSVariable>() { new PSVariable("_", this) }, new PSVariable("this", CurrentModel));
				Mount(CurrentModel);
				return CurrentModel;
			}

			public static void Dismount(Model model)
			{
				if (model.IsMounted)
				{
					model.InvokeEvent(EventOption.PreDismount);
					model.FullName = null;
					model.IsMounted = false;
				}
				foreach (Model child in model.Children)
				{
					Dismount(child);
				}
			}

			private void Mount(Model model)
			{
				if (model.Exists)
				{
					if (model.ShouldHardMount)
					{
						model.InvokeEvent(EventOption.Delete);
						model.InvokeEvent(EventOption.Create);
					}
				}
				else
				{
					model.InvokeEvent(EventOption.Create);
				}
				foreach (Model child in model.Children)
				{
					child.FullName = Path.Combine(model.FullName, child.Name);
					child.Parent = model;
					CurrentModel = child;
					ClearEvent();
					child.Build(this);
					child.PSBuild.InvokeWithContext(null, new List<PSVariable>() { new PSVariable("_", this) }, new PSVariable("this", CurrentModel));
					Mount(child);
				}
				if (!model.IsMounted)
				{
					model.IsMounted = true;
					model.InvokeEvent(EventOption.PostMount);
				}
			}
		}
	}
}
