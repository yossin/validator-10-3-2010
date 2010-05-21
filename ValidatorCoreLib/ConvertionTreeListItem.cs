using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ValidatorCoreLib
{
    [Serializable(), XmlRoot("ConvertionTreeListItem", Namespace = "ValidatorCoreLib", IsNullable = false)]
    public class ConvertionTreeListItem
    {
        public List<ConvertionTree> convertionTreeListItem = new List<ConvertionTree>();

        // Used only for xml Serialization
        public ConvertionTreeListItem() { }

        public void Add(ConvertionTree convertionItem)
        {
            convertionTreeListItem.Add(convertionItem);
        }

        public void Clear()
        {
            convertionTreeListItem.Clear();
        }
    }
    
}
