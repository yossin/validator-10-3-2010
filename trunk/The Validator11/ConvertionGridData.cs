using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace The_Validator11
{
    [Serializable(), XmlRoot("ConvertionGridData", Namespace = "The_Validator11",IsNullable = false)]
    public class ConvertionGridData
    {
//        public List<ValidationFlow> flows = new List<ValidationFlow>();

        public ConvertionGridData() { }

        public void Add()
        {
 //           rules.Add(rule);
        }

        public void Add(String sFrom, String sTo)
        {
  //          flows.Add(flow);
        }

        public void Clear()
        {
 //           flows.Clear();
        }
    }
    
}
