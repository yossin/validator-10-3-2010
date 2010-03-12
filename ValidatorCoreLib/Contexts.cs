using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValidatorCoreLib
{
    public class Context : IContext
    {
        public Context(string sName, int nID, object oValue)
        {
            this.property = new Property(sName, nID,oValue.GetType());
            this.oValue = oValue;
        }
        public override string GetValueType()   // lower case!
        {
            if (oValue != null)
            {
                if (oValue.GetType().FullName.Equals("System.String"))
                    return "string";
                if (oValue.GetType().FullName.Equals("System.Int32"))
                    return "int";
            }
            return "TypeUndefined";
        }
    }
    /*
    public class MultiContext
    {
        public string sMultiContextName { get; set; }
        Dictionary<int, Context> SingleContexts;    // map ID to SingleContext

        public MultiContext(string sName)
        {
            this.sMultiContextName = sName;
        }

        public Context Get(int nID)
        {
            return SingleContexts.ContainsKey(nID) ? SingleContexts[nID] : null;
        }

        public void Add(Context singleContext)
        {
            SingleContexts[singleContext.nID] = singleContext;
        }

        public bool CompareTo(MultiContext otherMultiContext)
        {
            return (sMultiContextName.Equals(otherMultiContext.sMultiContextName) &&
                    SingleContexts.Equals(otherMultiContext.SingleContexts));
        }

        public bool HasID(Context Context)
        {
            return SingleContexts.ContainsKey(Context.nID);
        }
    }*/
}
