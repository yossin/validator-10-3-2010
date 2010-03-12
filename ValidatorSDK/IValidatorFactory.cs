using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValidatorCoreLib;


namespace ValidatorSDK
{
   
    public interface IValidatorFactory
    {

        Validator CreateValidator(string flowName);
    }
}
