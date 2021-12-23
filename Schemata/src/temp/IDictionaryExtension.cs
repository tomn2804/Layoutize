using System;
using System.Collections;
using System.Management.Automation;

namespace Schemata
{
    public static class IDictionaryExtension
    {
        public static void Merge(this IDictionary @this, params IDictionary[] dictionaries)
        {
            foreach (IDictionary dictionary in dictionaries)
            {
                foreach (DictionaryEntry entry in dictionary)
                {
                    if (!@this.Contains(entry.Key))
                    {
                        switch (entry.Value)
                        {
                            case null:
                                throw new ArgumentNullException(entry.Key.ToString());

                            case PSObject @object:
                                @this.Add(entry.Key, @object.BaseObject);
                                break;

                            default:
                                @this.Add(entry.Key, entry.Value);
                                break;
                        }
                    }
                }
            }
        }
    }
}
