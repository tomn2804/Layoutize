using System.Collections.Generic;
using System.Diagnostics;
using System.Management.Automation;

namespace SchemataPreview
{
	[Cmdlet(VerbsLifecycle.Invoke, "Event")]
	public class InvokeEventCmdlet : Cmdlet
	{
		[Parameter(Mandatory = true, Position = 0)]
		[ValidateNotNull]
		public ScriptBlock? Callback { get; set; }

		[Parameter(Mandatory = true, ValueFromPipeline = true)]
		[ValidateNotNull]
		public Model? InputObject { get; set; }

		[Parameter]
		public string? Traversal { get; set; }

		protected override void ProcessRecord()
		{
			Debug.Assert(Callback != null);
			Debug.Assert(InputObject != null);
			Callback.InvokeWithContext(null, new List<PSVariable>() { new PSVariable("_", InputObject) });
			switch (Traversal)
			{
				case "ReversePostOrder":
					InvokeReversePostOrder(InputObject);
					break;

				case "ReversePreOrder":
				default:
					InvokeReversePreOrder(InputObject);
					break;
			}
		}

		private void InvokeReversePostOrder(Model model)
		{
			Debug.Assert(Callback != null);
			if (model.Children != null)
			{
				foreach (Model child in model.Children)
				{
					Callback.InvokeWithContext(null, new List<PSVariable>() { new PSVariable("_", child) });
				}
				foreach (Model child in model.Children)
				{
					InvokeReversePostOrder(child);
				}
			}
		}

		private void InvokeReversePreOrder(Model model)
		{
			Debug.Assert(Callback != null);
			if (model.Children != null)
			{
				foreach (Model child in model.Children)
				{
					Callback.InvokeWithContext(null, new List<PSVariable>() { new PSVariable("_", child) });
					InvokeReversePostOrder(child);
				}
			}
		}
	}
}
