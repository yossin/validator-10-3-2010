using System;
using System.Collections.Generic;
using System.Text;

namespace ValidatorCoreLib
{
    public class PropertySelection
    {
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
