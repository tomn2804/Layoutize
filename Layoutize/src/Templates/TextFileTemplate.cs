using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Linq;

namespace Layoutize.Templates;

public sealed partial class TextFileTemplate
{
    public new class DetailOption : Template.DetailOption
    {
        public const string Text = nameof(Text);
    }
}

public sealed partial class TextFileTemplate : Template<FileView>
{
    public TextFileTemplate(IDictionary attributes)
        : base(attributes)
    {
    }

    protected override Layout ToBlueprint()
    {
        return new FileTemplate(Details.SetItems(new[] { GetOnCreatedDetail() }));
    }

    private KeyValuePair<object, object> GetOnCreatedDetail()
    {
        EventHandler<Activity.ProcessedEventArgs> handler = (object? sender, Activity.ProcessedEventArgs args) =>
        {
            if (Details.TryGetValue(DetailOption.Text, out object? textValue))
            {
                Node node = (Node)sender!;
                IEnumerable<object>? texts = textValue as IEnumerable<object>;
                if (texts is null)
                {
                    texts = new[] { textValue };
                }
                File.WriteAllLines(node.View.FullName, texts.Cast<string>());
            }
            if (Details.TryGetValue(Template.DetailOption.OnCreated, out object? onCreatedValue))
            {
                switch (onCreatedValue)
                {
                    case ScriptBlock scriptBlock:
                        scriptBlock.Invoke(sender, args);
                        break;

                    default:
                        ((EventHandler<Activity.ProcessedEventArgs>)onCreatedValue).Invoke(sender, args);
                        break;
                }
            }
        };
        return KeyValuePair.Create<object, object>(Template.DetailOption.OnCreated, handler);
    }
}
