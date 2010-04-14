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
        public string contextContain { get; set; }
        public string key { get; set; }
        public Object ConflictedObject { get; set; }
        public int id { get; set; }

        // conflict resolve 
        public bool AutoResolve { get; set; }

        public List<ValidationResolveObject> ResolveObjects = new List<ValidationResolveObject>();

        // Used only for xml Serialization
        public ValidationResolve() 
        {
        }

        public ValidationResolve(int nRuleID, string contextContain, string key, Object conflictedObject, bool AutoResolve)
        {
            this.id = nRuleID;
            this.contextContain = contextContain;
            this.key = key;
            this.ConflictedObject = conflictedObject;
            this.AutoResolve = AutoResolve;
        }

        public void AddResolveObjects(ValidationResolveObject resolveObjects)
        {
            ResolveObjects.Add(resolveObjects);
        }

        public void ClearResolveObjects()
        {
            ResolveObjects.Clear();
        }
    }
}
