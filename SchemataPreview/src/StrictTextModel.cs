namespace SchemataPreview
{
	public class StrictTextModel : TextModel
	{
		public virtual void Format()
		{
			Contents = TextEditor.Format(Contents);
		}
	}
}
