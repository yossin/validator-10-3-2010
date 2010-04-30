using System;
using System.Collections.Generic;
using System.Text;

namespace ValidatorCoreLib
{
    public class ValidationRequest
    {
        ObjectBinder binder = null;
        public ValidationRequest(ObjectBinder binder)
        {
            this.binder = binder;
        }
        public ObjectBinder Binder { get { return binder; } }
    }
}
