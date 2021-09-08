namespace SchemataPreview
{
	public static class ModelBuilder
	{
		public static void HandleCreate(in Model model)
		{
			model.InvokeMethod(MethodOption.Create);
			model.InvokeCallback(EventOption.OnCreated);
		}

		public static void HandleDelete(in Model model)
		{
			model.InvokeMethod(MethodOption.Delete);
			model.InvokeCallback(EventOption.OnDeleted);
		}

		public static void HandleMount(in Model model)
		{
			model.InvokeMethod(MethodOption.Mount);
			model.InvokeCallback(EventOption.OnMounted);
		}

		public static void HandleMount(in Model.ModelSet models)
		{
			models.InvokeMethod(MethodOption.Mount);
			models.InvokeCallback(EventOption.OnMounted);
		}
	}
}
