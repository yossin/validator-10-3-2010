using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ValidatorCoreLib
{
    [Serializable(), XmlRoot("ValidationConvertionItem", Namespace = "ValidatorCoreLib",IsNullable = false)]
    public class ValidationConvertionItem
    {
        public List<ValidationConvertionItem> convertionItems = new List<ValidationConvertionItem>();
        public string convertionItemName { get; set; }
        public string convertTo { get; set; }

        // Used only for xml Serialization
        public ValidationConvertionItem() { }

        public ValidationConvertionItem(string convertionItemName, string convertTo)
        {
            this.convertionItemName = convertionItemName;
            this.convertTo = convertTo;
        }

        public void Add(ValidationConvertionItem convertionItem)
        {
            convertionItems.Add(convertionItem);
        }

        public void Clear()
        {
            convertionItems.Clear();
        }
    }
    
}
