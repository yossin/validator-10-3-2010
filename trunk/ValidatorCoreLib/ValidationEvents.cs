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
        }

        public interface ValidationRuntimeErrorEvent : ErrorValidationEvent
        {
            Exception LinkedException { get; }
        }
        public abstract class BaseValidationEvent : ErrorValidationEvent
        {
            string message;

            public string Message { get { return message; } }
            internal BaseValidationEvent(string message)
            {
                this.message = message;
            }
        }
        public abstract class RuleValidationEvent : BaseValidationEvent
        {
            ValidationRule rule = null;
            internal RuleValidationEvent(string message, ValidationRule rule)
                : base(message)
            {
                this.rule = rule;
            }
            public ValidationRule Rule { get { return rule; } }
        }

        public class UnableToBindEvent : RuleValidationEvent, ValidationRuntimeErrorEvent
        {
            Exception linkedException;
            ObjectSelection selection = null;
            int argument;
            public UnableToBindEvent(Exception linkedException, ValidationRule rule, ObjectSelection selection, int argument) :
                base("unable to bind object " + selection, rule)
            {
                this.selection = selection;
                this.argument = argument;
                this.linkedException = linkedException;
            }
            public ObjectSelection Selection { get { return selection; } }
            public int Argument { get { return argument; } }
            public Exception LinkedException { get { return linkedException; } }
        }

        public class RuleRuntimeErrorEvent : RuleValidationEvent, ValidationRuntimeErrorEvent
        {
            Exception linkedException;
            IComparable object1;
            IComparable object2;
            public RuleRuntimeErrorEvent(Exception linkedException, ValidationRule rule, IComparable object1, IComparable object2) :
                base("role runtime error event: " + linkedException, rule)
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
            public UnsuccessfulRuleCompletionEvent(ValidationRule rule, IComparable object1, IComparable object2) :
                base("Unsuccessful role validation", rule)
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
            public UnsuccessfulFlowCompletionEvent(ValidationFlow flow) :
                base("Unsuccessful flow validation")
            {
                this.flow = flow;

            }
            public ValidationFlow Flow { get { return flow; } }
        }


    }
}
