using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ValidatorCoreLib
{
   [Serializable(), XmlRoot("BindingContainer", Namespace = "ValidatorCoreLib", IsNullable = false)]
   public class BindingContainer
    {
        public BindingContainer() 
        {
            contextBinding = new Dictionary<string, string>();
            referenceBinding = new Dictionary<string, string>();
        }

        public BindingContainer(Dictionary<string, string> contextBinding, Dictionary<string, string> referenceBinding)
        {
            this.contextBinding = contextBinding;
            this.referenceBinding = referenceBinding;
        }

        //definitionKey, string runtimeKey
        Dictionary<string, string> contextBinding { get; set; }
        //referenceKey, string propertyChain
        Dictionary<string, string> referenceBinding { get; set; }

        [System.Xml.Serialization.XmlIgnoreAttribute]
        public Dictionary<string, string> ContextBinding { get { return contextBinding; } }
        [System.Xml.Serialization.XmlIgnoreAttribute]
        public Dictionary<string, string> ReferenceBinding { get { return referenceBinding; } }

        public void Clear()
        {
            contextBinding.Clear();
            referenceBinding.Clear();
        }



        [Serializable(), XmlRoot("DictionaryEntry_", Namespace = "ValidatorCoreLib", IsNullable = false)]
        public class DictionaryEntry_
        {
            public string Key{ get; set; }
            public string Value { get; set; }
        }
           
        [XmlArray("BindingContainer")]
        [XmlArrayItem("referenceBinding", Type = typeof(DictionaryEntry_))]
        public DictionaryEntry_[] _x_referenceBinding  
        {  
             get  
             {
                 DictionaryEntry_[] ret = new DictionaryEntry_[ReferenceBinding.Count];  
                 int i=0;
                 DictionaryEntry_ de;  
                 foreach (KeyValuePair<string, string> ReferenceBindingLine in ReferenceBinding)  
                 {
                     de = new DictionaryEntry_();  
                     de.Key = ReferenceBindingLine.Key;  
                     de.Value = ReferenceBindingLine.Value;  
                     ret[i]=de;  
                     i++;  
                 }  
                 return ret;  
             }  
             set  
             {  
                 ReferenceBinding.Clear();  
                 for (int i=0; i<value.Length; i++)  
                 {  
                     ReferenceBinding.Add(value[i].Key, value[i].Value);  
                 }  
             }  
         }


        [XmlArray("BindingContainer")]
        [XmlArrayItem("contextBinding", Type = typeof(DictionaryEntry_))]
        public DictionaryEntry_[] _x_contextBinding
        {
            get
            {
                DictionaryEntry_[] ret = new DictionaryEntry_[ContextBinding.Count];
                int i = 0;
                DictionaryEntry_ de;
                foreach (KeyValuePair<string, string> ContextBindingLine in ContextBinding)
                {
                    de = new DictionaryEntry_();
                    de.Key = ContextBindingLine.Key;
                    de.Value = ContextBindingLine.Value;
                    ret[i] = de;
                    i++;
                }
                return ret;
            }
            set
            {
                ContextBinding.Clear();
                for (int i = 0; i < value.Length; i++)
                {
                    ContextBinding.Add(value[i].Key, value[i].Value);
                }
            }
        } 



   }


     
}
