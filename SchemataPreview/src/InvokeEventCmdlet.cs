using System.Collections.Generic;
using System.Management.Automation;

namespace SchemataPreview
{
	[Cmdlet(VerbsLifecycle.Invoke, "Event")]
	public class InvokeEventCmdlet : Cmdlet
	{
		public ScriptBlock? Begin { get; set; }
		public ScriptBlock? End { get; set; }
		public Model InputObject { get; set; }
		public ScriptBlock? Process { get; set; }
		public string Traversal { get; set; }

		protected override void ProcessRecord()
		{
			Begin?.InvokeWithContext(null, new List<PSVariable>() { new PSVariable("this", InputObject) });
			InvokePostOrder(InputObject);
			End?.InvokeWithContext(null, new List<PSVariable>() { new PSVariable("this", InputObject) });
		}

		private void InvokePostOrder(Model model)
		{
			ScriptBlock? postProcess = Process?.InvokeWithContext(null, new List<PSVariable>() { new PSVariable("_", model) });
			if (model.Children != null)
			{
				foreach (Model child in model.Children)
				{
					InvokePostOrder(child);
				}
			}
			postProcess?.InvokeWithContext(null, new List<PSVariable>() { new PSVariable("_", model) });
		}
	}
}
