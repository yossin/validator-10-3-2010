using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValidatorCoreLib;


namespace ValidatorSDK
{
    

    public class Validator
    {
        ValidationFlow flow;
        public Validator(ValidationFlow flow)
        {
            this.flow = flow;
        }
        public ValidationResult Validate(ValidationData data)
        {
            if (flow.Validate(data.Contexts))
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
