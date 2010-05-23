using System;
using System.Collections.Generic;
using System.Text;
using ValidatorCoreLib.ValidationErrorEvents;

namespace ValidatorCoreLib
{
    public class FlowErrorTrace
    {
        ValidationFlow flow;
        bool emptyTrace = false;
        internal FlowErrorTrace(ValidationFlow flow)
        {
            this.flow = flow;
        }

        List<ErrorValidationEvent> errorEvents = new List<ErrorValidationEvent>();
        List<FlowErrorTrace> flowErrors = new List<FlowErrorTrace>();

        public void AddErrorValidationEvent(ErrorValidationEvent errorEvent)
        {
            if (errorEvent == null)
            {
                emptyTrace = true;
            }
            else
            {
                errorEvents.Add(errorEvent);
            }
        }
        public void AddFlowErrorTrace(FlowErrorTrace flowError)
        {
            if (flowError == null)
            {
                emptyTrace = true;
            }
            else
            {
                flowErrors.Add(flowError);
            }
        }

        public List<ErrorValidationEvent> ErrorEvents { get { return errorEvents; } }
        public List<FlowErrorTrace> FlowErrors { get { return flowErrors; } }
        public ValidationFlow Flow { get { return flow; } }

        internal bool containsMajorValidationErrors()
        {
            if (flow.UseAndOperator == false && emptyTrace == true)
            {
                return false;
            }
            return (flowErrors.Count > 0) || (errorEvents.Count > 0);
        }
    }
}
