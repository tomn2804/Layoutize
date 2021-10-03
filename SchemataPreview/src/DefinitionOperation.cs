using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;

namespace SchemataPreview
{
    public enum DefinitionOperator
    {
        Spread
    }

    public static class DefinitionOperation
    {
        public static void Spread(IDictionary<object, object> x, IDictionary y)
        {
            foreach (DictionaryEntry entry in y)
            {
                if (!x.ContainsKey(entry.Key))
                {
                    switch (entry.Value)
                    {
                        case null:
                            throw new ArgumentNullException(entry.Key.ToString());

                        case PSObject @object:
                            x.Add(entry.Key, @object.BaseObject);
                            break;

                        default:
                            x.Add(entry.Key, entry.Value);
                            break;
                    }
                }
            }
        }
    }
}
