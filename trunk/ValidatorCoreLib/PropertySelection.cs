using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ValidatorCoreLib
{
    [Serializable(), XmlRoot("PropertySelection", Namespace = "ValidatorCoreLib", IsNullable = false)]
    public class PropertySelection
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

    }
}
