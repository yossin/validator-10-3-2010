using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ValidatorCoreLib
{
    [Serializable(), XmlRoot("PropertySelection", Namespace = "ValidatorCoreLib", IsNullable = false)]
    public class PropertySelection : IComparable, IComparable<PropertySelection>
    {
        // Used only for xml Serialization
        public PropertySelection(){}

        // TODO: create object selection by using feilds
        public PropertySelection(string contextKey, string referenceMeaning)
        {
            this.ContextKey = contextKey;
            this.ReferenceMeaning = referenceMeaning;
        }
        public string ReferenceMeaning { get; set; }
        public string ContextKey { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("ContextKey=")
                    .Append(ContextKey)
                    .Append(", ReferenceMeaning=")
                    .Append(ReferenceMeaning);
            return builder.ToString();
        }


        public int CompareTo(object obj)
        {
            return CompareTo((PropertySelection)obj);
        }

        public int CompareTo(PropertySelection other)
        {
            string key1=ContextKey+"_"+ReferenceMeaning;
            string key2=other.ContextKey+"_"+other.ReferenceMeaning;
            return key1.CompareTo(key2);
        }
    }
}
