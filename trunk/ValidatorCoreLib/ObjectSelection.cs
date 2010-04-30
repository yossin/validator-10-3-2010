using System;
using System.Collections.Generic;
using System.Text;

namespace ValidatorCoreLib
{
    public class ObjectSelection
    {
        // TODO: create object selection by using feilds
        public ObjectSelection(string contextKey, string propertyPath)
        {
            this.ContextKey = contextKey;
            int lastDotIndex = propertyPath.LastIndexOf(".");
            if (lastDotIndex > -1)
            {
                this.EntityName = propertyPath.Substring(0, lastDotIndex);
                this.PropertyName = propertyPath.Substring(lastDotIndex + 1);
            }
            else
            {
                this.EntityName = propertyPath;
                this.PropertyName = null;
            }
        }
        public string EntityName { get; set; }
        public string PropertyName { get; set; }
        public string ContextKey { get; set; }

    }
}
