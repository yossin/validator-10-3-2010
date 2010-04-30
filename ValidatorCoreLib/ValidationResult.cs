using System;
using System.Collections.Generic;
using System.Text;
using ValidatorCoreLib;
using ValidatorCoreLib.ValidationErrorEvents;

namespace ValidatorCoreLib
{
    public class ValidationResult
    {
        ValidationErrorTrace errorTrace=null;
        // number of errors!



        public void AddErrorEvent(ErrorValidationEvent error)
        {
            if (errorTrace == null)
            {
                errorTrace = new ValidationErrorTrace();
            }
            errorTrace.AddErrorValidationEvent(error);
        }
        // todo: fix a bug here...
        internal void NotifiyFlowValidationEndIteration()
        {
            if (errorTrace != null)
            {
                errorTrace = new ValidationErrorTrace(errorTrace);
            }
        }


        public ValidationErrorTrace ErrorTrace { get { return errorTrace; } }
       
       

    }

    public class ValidationErrorTrace
    {
        List<ErrorValidationEvent> errorEvents = new List<ErrorValidationEvent>();
        ValidationErrorTrace causedBy = null;
        internal ValidationErrorTrace() { }
        internal ValidationErrorTrace(ValidationErrorTrace causedBy)
        {
            this.causedBy = causedBy;
        }
        public ValidationErrorTrace CausedBy { get { return causedBy; } }
        public void AddErrorValidationEvent(ErrorValidationEvent errorEvent)
        {
            errorEvents.Add(errorEvent);
        }
        public List<ErrorValidationEvent> ErrorEvents {get{return errorEvents;}}
    }

    
}
