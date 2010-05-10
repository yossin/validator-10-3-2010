using System;
using System.Collections.Generic;
using System.Text;

namespace ValidatorCoreLib
{
    public interface ObjectBinder
    {
        //todo: remove this when all code will use ObjectSelection!
        IComparable Bind (string tableKey, string propertyPath);
        IComparable Bind(PropertySelection selection);
    }
}
