using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValidatorCoreLib;


namespace ValidatorSDK
{
    public class Validator
    {
        public static ValidationResult Validate(ValidationData data)
        {
            if (data.flow.Validate(data.Contexts))
            {
                return new ValidationResult("Has NO conflict");
            }
            else
            {
                return new ValidationResult("Has conflict");
            }
        }
    }

}
