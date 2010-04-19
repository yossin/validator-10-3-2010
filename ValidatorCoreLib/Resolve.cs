using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
    
namespace ValidatorCoreLib
{
    [Serializable(), XmlRoot("ValidationResolve", Namespace = "ValidatorCoreLib", IsNullable = false)]
    public class ValidationResolve
    {
        // ConflictedObject - used to return array of ValidationResolve with the contex to the SDK
        [System.Xml.Serialization.XmlIgnoreAttribute]
        public Object ConflictedObject { get; set; }

        // conflict resolve 
        public bool AutoResolve { get; set; }
        public string ResolveStringForUI { get; set; }
        public List<Object> ResolveObjects = new List<Object>();
        public int ResolveObjectSelected = -1;
        // Used only for xml Serialization
        public ValidationResolve() 
        {
        }

        public ValidationResolve(bool AutoResolve, string ResolveStringForUI)
        {
            this.AutoResolve = AutoResolve;
            this.ResolveStringForUI = ResolveStringForUI;
        }

        public void AddResolveObject(Object resolveObject)
        {
            ResolveObjects.Add(resolveObject);
        }

        public void ClearResolveObjects()
        {
            ResolveObjects.Clear();
        }
    }
}
