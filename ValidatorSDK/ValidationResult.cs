using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValidatorCoreLib;


namespace ValidatorSDK
{
    public class ValidationResult
    {
        private string message;
        public ValidationResult(string message)
        {
            this.message = message;
        }
        public string Message
        {
            get { return message; }
        }
    }


}
