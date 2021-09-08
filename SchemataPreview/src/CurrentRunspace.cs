using System.Management.Automation;

namespace SchemataPreview
{
	public static class CurrentRunspace
	{
		public static void WriteWarning(string message)
		{
			using PowerShell instance = PowerShell.Create(RunspaceMode.CurrentRunspace);
			instance.AddCommand("Write-Warning").AddParameter("Message", message).Invoke();
		}
	}
}
