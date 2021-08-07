namespace SchemataPreview
{
	public class StrictTextModel : TextModel
	{
		public override void Format()
		{
			base.Format();
			Contents = TextEditor.Format(Contents);
		}
	}
}
