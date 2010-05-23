using System;
using System.Xml.Serialization;
using ValidatorCoreLib;
using ValidatorCoreLib.ValidationErrorEvents;

namespace ValidatorCoreLib
{
    // input:
    //      nRuleID
    //      contextContain  - the path to the compared object in the contextTable[key]
    //      key             - uniqe key to get the object from contextTable
    //      comparedObject  - object to compare with
    //      op              - operator to compare with
    // output: 
    //      by using the validate(contextTable) the rule detarmain if the object data has conflict     
    //
    // the rule is a set of definitions to check object data
    [Serializable(), XmlRoot("ValidationRule", Namespace = "ValidatorCoreLib", IsNullable = false)]
    public class ValidationRule
    {
        public string RuleName { get; set; }

        public PropertySelection Property { get; set; }
        public IComparable ComparedObject { get; set; }

        [System.Xml.Serialization.XmlIgnoreAttribute]
        public IOperator Operator { get; set; }

        public string OperatorName { get; set; }

     //   public int id { get; set; }

        // conflict resolve 
        public ValidationResolve validationResolve{ get; set; }         

        // Used only for xml Serialization
        public ValidationRule() 
        {
            this.Operator = OperatorHelper.GetOperator(this.OperatorName);
        }

        public ValidationRule(string RuleName, PropertySelection selection, IComparable comparedObject, IOperator op, bool AutoResolve, string ResolveStringForUI)
        {
            this.RuleName = RuleName;
            this.Operator = op;
            this.OperatorName = op.GetType().ToString();
            this.Property = selection;
            this.ComparedObject = comparedObject;
            validationResolve = new ValidationResolve(AutoResolve, ResolveStringForUI);
        }

        public ErrorValidationEvent Validate(ValidationRequest request, FlowErrorTrace parent)
        {
            ObjectBinder binder = request.Binder;
            // todo: check IComparable casting is possible
            IComparable comparableComaredObject = null;
            IComparable comparableToCheck = null;
            // bind 1st object
            try {
                comparableToCheck = binder.Bind(Property);
            }catch (Exception e)
            {
                return new UnableToBindEvent(parent, e, this, Property, 1);
            }

            
            if (ComparedObject.GetType().Equals(typeof(PropertySelection)))
            {
                // bind 2nd object
                PropertySelection selected = (PropertySelection)ComparedObject;
                try
                {
                    comparableComaredObject = binder.Bind(selected);
                }
                catch (Exception e)
                {
                    return new UnableToBindEvent(parent, e, this, selected, 2);
                }
            }
            else
            {
                // set 2nd object
                try
                {
                    comparableComaredObject = (IComparable)ComparedObject;
                }
                catch (Exception e)
                {
                    return new RuleRuntimeErrorEvent(parent, e, this, comparableToCheck, comparableComaredObject);
                }
            }

            // evaluate
            try
            {
                if (Operator.Evaluate(comparableToCheck, comparableComaredObject) == false)
                {
                    return new UnsuccessfulRuleCompletionEvent(parent, this, comparableToCheck, comparableComaredObject);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return (new RuleRuntimeErrorEvent(parent, e, this, comparableToCheck, comparableComaredObject));
            }
        }

        /*public override string ToString()
        {
            return "Rule: ContextKey="+this.key+", PropertiesPath="+this.PropertiesPath;
        }*/

    }
}
