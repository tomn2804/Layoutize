using System;

namespace SchemataPreview
{
	public class ModelNotMountedException : Exception
	{
		public ModelNotMountedException(Model model)
			: base($"Model of type '{model.GetType().Name}' and name '{model.Name}' is not mounted")
		{
		}
	}
}
