using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using ValidatorCoreLib;
using ValidatorCoreLib.ValidationErrorEvents;

namespace ValidatorCoreLib
{
    // flow is a recursive structure that holds rules and know how to do a recursive validation.
    // a flow has a two operator options - and/or to determain the action validation check inside the current recursive check
    [Serializable(), XmlRoot("ValidationFlow", Namespace = "ValidatorCoreLib",IsNullable = false)]
    public class ValidationFlow
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

        public FlowErrorTrace Validate(ValidationRequest request)
        {
            FlowErrorTrace flowErrorTrace = new FlowErrorTrace(this);

            foreach (ValidationFlow flow in flows)
            {
                FlowErrorTrace lastFlowTrace = flow.Validate(request);
                flowErrorTrace.AddFlowErrorTrace(lastFlowTrace);
            }
            foreach (ValidationRule rule in rules)
            {
                ErrorValidationEvent validationEvent = rule.Validate(request);
                flowErrorTrace.AddErrorValidationEvent(validationEvent);
            }

            if (flowErrorTrace.containsMajorValidationErrors())
            {
                return flowErrorTrace;
            }
            else
            {
                return null;
            }
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
