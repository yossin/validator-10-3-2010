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
        [System.Xml.Serialization.XmlIgnoreAttribute]
        public IOperator Operator { get; set; }

        public string OperatorName { get; set; }

        public string PropertiesPath { get; set; }
        public string key { get; set; }
        public Object ComparedObject { get; set; }
        public int id { get; set; }

        // conflict resolve 
        public ValidationResolve validationResolve{ get; set; }         

        // Used only for xml Serialization
        public ValidationRule() 
        {
            this.Operator = OperatorHelper.GetOperator(this.OperatorName);
        }

        public ValidationRule(int nRuleID, string contextContain, string key, Object comparedObject, IOperator op, bool AutoResolve, string ResolveStringForUI)
        {
            this.id = nRuleID;
            this.Operator = op;
            this.OperatorName = op.GetType().ToString();
            this.PropertiesPath = contextContain;
            this.key = key;            
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
                comparableToCheck = binder.Bind(key, PropertiesPath);
            }catch (Exception e)
            {
                result.AddErrorEvent(new UnableToBindEvent(e, this, new PropertySelection(key,PropertiesPath),1));
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

        public override string ToString()
        {
            return "Rule: id="+this.id+", ContextKey="+this.key+", PropertiesPath="+this.PropertiesPath;
        }

    }
}
