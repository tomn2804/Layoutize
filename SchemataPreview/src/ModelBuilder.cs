namespace SchemataPreview
{
	public static class ModelBuilder
	{
		public static void HandleCreate(in Model model)
		{
			model.InvokeMethod(MethodOption.Create);
			model.InvokeCallback(PipeOption.OnCreated);
		}

		public static void HandleDelete(in Model model)
		{
			model.InvokeMethod(MethodOption.Delete);
			model.InvokeCallback(PipeOption.OnDeleted);
		}

		public static void HandleMount(in Model model)
		{
			model.InvokeMethod(MethodOption.Mount);
			model.InvokeCallback(PipeOption.OnMounted);
		}

		public static void HandleMount(in Model.ModelSet models)
		{
			models.InvokeMethod(MethodOption.Mount);
			models.InvokeCallback(PipeOption.OnMounted);
		}
	}
}
