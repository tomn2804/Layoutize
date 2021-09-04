using System;
using System.Management.Automation;

namespace SchemataPreview
{
	public static class CurrentShell
	{
		public static void WriteWarning(Exception exception)
		{
			using PowerShell shell = PowerShell.Create(RunspaceMode.CurrentRunspace);
			shell.AddCommand("Write-Warning").AddParameter("Message", exception).Invoke();
		}
	}
}
