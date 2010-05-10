using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ValidatorCoreLib
{
    [Serializable(), XmlRoot("ConvertionTree", Namespace = "ValidatorCoreLib", IsNullable = false)]
    public class ConvertionTree
    {
        public List<ConvertionTree> convertionItems = new List<ConvertionTree>();
        public string convertionPath { get; set; }
        public string convertionAttribute { get; set; }

        // Used only for xml Serialization
        public ConvertionTree() { }

        public ConvertionTree(string convertionPath, string convertionAttribute)
        {
            this.convertionPath = convertionPath;
            this.convertionAttribute = convertionAttribute;
        }

        public void Add(ConvertionTree convertionItem)
        {
            convertionItems.Add(convertionItem);
        }

        public void Clear()
        {
            convertionItems.Clear();
        }
    }
    
}
