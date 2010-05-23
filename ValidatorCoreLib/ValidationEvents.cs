using System;
using System.Collections.Generic;
using System.Text;
using ValidatorCoreLib;

namespace ValidatorCoreLib
{
    namespace ValidationErrorEvents
    {
        public interface ErrorValidationEvent
        {
            string Message { get; }
            FlowErrorTrace Parent { get;}

        }

        public interface ValidationRuntimeErrorEvent : ErrorValidationEvent
        {
            Exception LinkedException { get; }
        }
        public abstract class BaseValidationEvent : ErrorValidationEvent
        {
            string message;
            public FlowErrorTrace Parent { get; private set; }

            public string Message { get { return message; } }
            internal BaseValidationEvent(FlowErrorTrace parent, string message)
            {
                this.message = message;
                this.Parent = parent;
            }
            public override string ToString()
            {
                return message;
            }
        }
        public abstract class RuleValidationEvent : BaseValidationEvent
        {
            ValidationRule rule = null;
            internal RuleValidationEvent(FlowErrorTrace parent, string message, ValidationRule rule)
                : base(parent, message)
            {
                this.rule = rule;
            }
            public ValidationRule Rule { get { return rule; } }
        }

        public class UnableToBindEvent : RuleValidationEvent, ValidationRuntimeErrorEvent
        {
            Exception linkedException;
            PropertySelection selection = null;
            int argument;
            public UnableToBindEvent(FlowErrorTrace parent, Exception linkedException, ValidationRule rule, PropertySelection selection, int argument) :
                base(parent, "unable to bind object " + selection, rule)
            {
                this.selection = selection;
                this.argument = argument;
                this.linkedException = linkedException;
            }
            public PropertySelection Selection { get { return selection; } }
            public int Argument { get { return argument; } }
            public Exception LinkedException { get { return linkedException; } }
        }

        public class RuleRuntimeErrorEvent : RuleValidationEvent, ValidationRuntimeErrorEvent
        {
            Exception linkedException;
            IComparable object1;
            IComparable object2;
            public RuleRuntimeErrorEvent(FlowErrorTrace parent, Exception linkedException, ValidationRule rule, IComparable object1, IComparable object2) :
                base(parent, "role runtime error event: " + linkedException, rule)
            {

                this.object1 = object1;
                this.object2 = object2;
                this.linkedException = linkedException;
            }
            public IComparable Object1 { get { return object1; } }
            public IComparable Object2 { get { return object2; } }
            public Exception LinkedException { get { return linkedException; } }
        }


 

        public class UnsuccessfulRuleCompletionEvent : RuleValidationEvent,ErrorValidationEvent
        {
            IComparable object1;
            IComparable object2;
            public UnsuccessfulRuleCompletionEvent(FlowErrorTrace parent, ValidationRule rule, IComparable object1, IComparable object2) :
                base(parent, "Unsuccessful role validation", rule)
            {
                this.object1 = object1;
                this.object2 = object2;

            }
            public IComparable Object1 { get { return object1; } }
            public IComparable Object2 { get { return object2; } }
        }

        public class UnsuccessfulFlowCompletionEvent : BaseValidationEvent
        {
            ValidationFlow flow;
            public UnsuccessfulFlowCompletionEvent(FlowErrorTrace parent, ValidationFlow flow) :
                base(parent, "Unsuccessful flow validation")
            {
                this.flow = flow;

            }
            public ValidationFlow Flow { get { return flow; } }
        }


    }
}
