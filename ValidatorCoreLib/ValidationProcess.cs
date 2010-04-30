using System;
using System.Collections.Generic;
using System.Text;

namespace ValidatorCoreLib
{
    public interface ValidationProcess
    {
        bool Validate(ValidationRequest request, ValidationResult result);
    }
}
