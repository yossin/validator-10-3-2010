using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

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

        public bool Validate(ContextTable contextTable)
        {
            bool Res = UseAndOperator ? true : false;

            foreach (ValidationFlow flow in flows)
            {
                if (UseAndOperator)
                    Res &= flow.Validate(contextTable);
                else
                    Res |= flow.Validate(contextTable);
            }
            foreach (ValidationRule rule in rules)
            {
                if (UseAndOperator)
                    Res &= rule.Validate(contextTable);
                else
                    Res |= rule.Validate(contextTable);
            }

            return Res;
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
    }
    
}
