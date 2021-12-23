using System.IO;

namespace Schemata
{
    public class TextModel : FileModel
    {
        public TextModel(Schema schema)
            : base(schema)
        {
        }

        public string[] Contents
        {
            get => File.ReadAllLines(FullName);
            set => File.WriteAllLines(FullName, value);
        }
    }
}
