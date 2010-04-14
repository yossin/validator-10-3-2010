using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
    
namespace ValidatorCoreLib
{
    [Serializable(), XmlRoot("ValidationResolveObject", Namespace = "ValidatorCoreLib", IsNullable = false)]
    public class ValidationResolveObject
    {
        public string ResolveStringForUI { get; set; }
        public Object ResolveObject { get; set; }

        // Used only for xml Serialization
        public ValidationResolveObject() 
        {
        }

        public ValidationResolveObject(string ResolveStringForUI, Object ResolveObject)
        {
            this.ResolveStringForUI = ResolveStringForUI;
            this.ResolveObject = ResolveObject;
        }
    }
}
