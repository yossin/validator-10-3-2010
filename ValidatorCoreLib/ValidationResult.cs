using System;
using System.Collections.Generic;
using System.Text;
using ValidatorCoreLib;
using ValidatorCoreLib.ValidationErrorEvents;

namespace ValidatorCoreLib
{
    public class ValidationResult
    {
        FlowErrorTrace errorTrace=null;
        List<ErrorValidationEvent> events;
        // number of errors!

        public ValidationResult(FlowErrorTrace errorTrace)
        {
            this.errorTrace = errorTrace;
            events = new List<ErrorValidationEvent>();
            if (errorTrace != null)
            {
                AddErrors(errorTrace, events);
            }
        }

        public bool ContainsError { get { return events.Count>0; } }

        public FlowErrorTrace ErrorTrace { get { return errorTrace; } }

        private static void AddErrors(FlowErrorTrace errorTrace, List<ErrorValidationEvent> events)
        {
            foreach(ErrorValidationEvent e in errorTrace.ErrorEvents)
            {
                events.Add(e);
            }
            foreach (FlowErrorTrace e in errorTrace.FlowErrors)
            {
                AddErrors(e, events);
            }

        }
        public List<ErrorValidationEvent> ErrorValidationEvents
        {
            get { return events; }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            foreach (ErrorValidationEvent e in ErrorValidationEvents)
            {
                builder.Append(e.Message);
                builder.Append(", ");
            }
            builder.Remove(builder.Length - 2,2);
            builder.Append("]");
            return builder.ToString();
        }
    }

    

    
}
