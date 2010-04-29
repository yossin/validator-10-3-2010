using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using TheValidatorCoreLib;
using TheValidatorCoreLib.ValidationErrorEvents;

namespace ValidatorCoreLib
{
    // flow is a recursive structure that holds rules and know how to do a recursive validation.
    // a flow has a two operator options - and/or to determain the action validation check inside the current recursive check
    [Serializable(), XmlRoot("ValidationFlow", Namespace = "ValidatorCoreLib",IsNullable = false)]
    public class ValidationFlow : ValidationProcess
    {
        public List<ValidationFlow> flows = new List<ValidationFlow>();
        public List<ValidationRule> rules = new List<ValidationRule>();

        public bool UseAndOperator { get; set; }
        public string Name { get; set; }

        // Used only for xml Serialization
        public ValidationFlow() { }

        public ValidationFlow(string Name, bool useAndOperator)
        {
            this.Name = Name;
            this.UseAndOperator = useAndOperator;
        }

        public bool Validate(ValidationRequest request, ValidationResult result)
        {
            bool success = UseAndOperator ? true : false;

            foreach (ValidationFlow flow in flows)
            {
                bool lastValidation = flow.Validate(request, result);
                if (UseAndOperator)
                    success &= lastValidation;
                else
                    success |= lastValidation;
                if (lastValidation == false)
                {
                    result.AddErrorEvent(new UnsuccessfulFlowCompletionEvent(this));
                }
            }
            foreach (ValidationRule rule in rules)
            {
                if (UseAndOperator)
                    success &= rule.Validate(request, result);
                else
                    success |= rule.Validate(request, result);
            }
            
            result.NotifiyFlowValidationEndIteration();
            return success;
        }

        public void Add(ValidationRule rule)
        {
            rules.Add(rule);
        }

        public void Add(ValidationFlow flow)
        {
            flows.Add(flow);
        }

        public void Clear()
        {
            flows.Clear();
            rules.Clear();
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
    
}
