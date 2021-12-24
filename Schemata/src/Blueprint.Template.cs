using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Schemata;

public partial class Blueprint
{
    public abstract partial class Template
    {
        protected Template(IEnumerable details)
        {
            switch (details)
            {
                case IImmutableDictionary<object, object?> dictionary:
                    _details = dictionary;
                    break;

                case IDictionary<object, object?> dictionary:
                    _details = dictionary.ToImmutableDictionary();
                    break;

                case IDictionary dictionary:
                    _details = dictionary.Cast<DictionaryEntry>().ToImmutableDictionary(entry => entry.Key, entry => entry.Value);
                    break;

                default:
                    int key = 0;
                    _details = details.Cast<object?>().ToImmutableDictionary(_ => (object)key++, value => value);
                    break;
            }
        }

        public static implicit operator Blueprint(Template template)
        {
            Blueprint blueprint = template.ToBlueprint();
            if (!template.ModelType.IsAssignableTo(blueprint.ModelType))
            {
                throw new InvalidOperationException();
            }
            blueprint.Templates.Add(template);
            return blueprint;
        }

        public event EventHandler<DetailsUpdatingEventArgs>? DetailsUpdating;

        private readonly IImmutableDictionary<object, object?> _details;

        public IImmutableDictionary<object, object?> Details
        {
            get => _details;
            set
            {
                if (value != _details)
                {
                    OnDetailsUpdating(new(value));
                }
            }
        }

        protected virtual void OnDetailsUpdating(DetailsUpdatingEventArgs args)
        {
            DetailsUpdating?.Invoke(this, args);
        }

        public abstract Type ModelType { get; }

        protected virtual Blueprint ToBlueprint()
        {
            return new();
        }
    }
}
