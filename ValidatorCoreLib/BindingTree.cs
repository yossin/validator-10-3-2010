using System;
using System.Collections.Generic;
using System.Text;

namespace ValidatorCoreLib
{


    public class BindingContainer
    {
        public BindingContainer(Dictionary<string, string> contextBinding, Dictionary<string, string> referenceBinding)
        {
            this.contextBinding = contextBinding;
            this.referenceBinding = referenceBinding;
        }
        //definitionKey, string runtimeKey
        Dictionary<string, string> contextBinding;
        //referenceKey, string propertyChain
        Dictionary<string, string> referenceBinding;
        public Dictionary<string, string> ContextBinding { get { return contextBinding; } }
        public Dictionary<string, string> ReferenceBinding { get { return referenceBinding; } }
    }


     
}
