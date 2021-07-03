using System;
using SchemataPreview.Models;
using System.IO;

namespace SchemataPreview
{
	public class Controller
	{
		public Model Model { get; private set; }

		public Controller(Model model)
		{
			Model = model;
		}

		public Controller Mount(string path)
		{
			Model.FullName = path;
			ControllerHandler.Mount(Model);
			return this;
		}

		public Controller Dismount()
		{
			ControllerHandler.Dismount(Model);
			return this;
		}

		public Controller Create()
		{
			ControllerHandler.Create(Model);
			return this;
		}

		public Controller Delete()
		{
			ControllerHandler.Delete(Model);
			return this;
		}

		//public Controller Clear()
		//{
		//	ControllerHandler.Clear(Model);
		//	return this;
		//}

		//public Controller Format()
		//{
		//	ControllerHandler.Format(Model);
		//	return this;
		//}
	}
}
