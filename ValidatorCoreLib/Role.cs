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
    public class ValidationRule : ValidationProcess
    {
        public string RuleName { get; set; }

        public PropertySelection Property { get; set; }
        public PropertySelection ComparedObject { get; set; }

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

        public ValidationRule(string RuleName, PropertySelection property, PropertySelection comparedObject, IOperator op, bool AutoResolve, string ResolveStringForUI)
        {
            this.RuleName = RuleName;
            this.Operator = op;
            this.OperatorName = op.GetType().ToString();
            this.Property = property;
            this.ComparedObject = comparedObject;
            validationResolve = new ValidationResolve(AutoResolve, ResolveStringForUI);
        }

        public bool Validate(ValidationRequest request, ValidationResult result)
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
                result.AddErrorEvent(new UnableToBindEvent(e, this, Property, 1));
                return false;
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
                    result.AddErrorEvent(new UnableToBindEvent(e, this, selected, 2));
                    return false;
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
                    result.AddErrorEvent(new RuleRuntimeErrorEvent(e, this, comparableToCheck, comparableComaredObject));
                    return false;
                }
            }

            // evaluate
            try
            {
                if (Operator.Evaluate(comparableToCheck, comparableComaredObject) == false)
                {
                    result.AddErrorEvent(new UnsuccessfulRuleCompletionEvent(this, comparableToCheck, comparableComaredObject));
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                result.AddErrorEvent(new RuleRuntimeErrorEvent(e, this, comparableToCheck, comparableComaredObject));
                return false;
            }
        }

        /*public override string ToString()
        {
            return "Rule: ContextKey="+this.key+", PropertiesPath="+this.PropertiesPath;
        }*/

    }
}
