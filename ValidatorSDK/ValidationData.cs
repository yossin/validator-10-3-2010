using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValidatorCoreLib;

namespace ValidatorSDK
{
    public class ValidationData
    {
        public ContextTable Contexts { get; set; }
        public ValidationFlow flow { get; set; }

        private static int ii = 1;
            
        public ValidationData()
        {
            this.Contexts = new ContextTable();
            this.flow = new ValidationFlow((ii++).ToString(), true);
        }

        public ValidationData(ContextTable contexts, ValidationFlow flow)
        {
            this.Contexts = contexts;
            this.flow = flow;
        }

        public void Add(string key, Object obj)
        {
            Contexts.Add(key, obj);
        }

        public void Add(ValidationFlow flowToAdd)
        {
            flow.Add(flowToAdd);
        }

        public void Add(ValidationRule rule)
        {
            flow.Add(rule);
        }

        public void Clear()
        {
            Contexts.Clear();
            flow.Clear();

        }
    }

    
}
