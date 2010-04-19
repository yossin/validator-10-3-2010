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
        public ValidationConvertionItem convertionItem { get; set; }
        public ConvertionComparedItems convertionComparedItems { get; set; }

        private static int ii = 1;
            
        public ValidationData()
        {
            this.Contexts = new ContextTable();
            this.flow = new ValidationFlow((ii++).ToString(), true);
            this.convertionItem = new ValidationConvertionItem();
            convertionComparedItems = new ConvertionComparedItems();
        }

        public ValidationData(ContextTable contexts, ValidationFlow flow, ValidationConvertionItem convertionItem)
        {
            this.Contexts = contexts;
            this.flow = flow;
            this.convertionItem = convertionItem;
            convertionComparedItems = new ConvertionComparedItems();
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

        public void Add(ValidationConvertionItem convertionItemToAdd)
        {
            convertionItem.Add(convertionItemToAdd);
        }

        public void Clear()
        {
            Contexts.Clear();
            flow.Clear();
            convertionItem.Clear();
            convertionComparedItems.Clear();

        }
    }

    
}
