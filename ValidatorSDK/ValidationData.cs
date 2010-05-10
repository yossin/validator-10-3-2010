using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using ValidatorCoreLib;
    
namespace ValidatorSDK
{
    public class ValidationData
    {
        public ContextTable Contexts { get; set; }
        public ValidationFlow flow { get; set; }
        public BindingContainer bindingContainer { get; set; }

        private static int ii = 1;


        public const string sFileFlow = @"Flow1.xml";
        public const string sFileConvertionTree = @"ConvertionTree.xml";
        public const string sFileConvertionPathItems = @"FileConvertionPathItems.xml";
        public const string sFileConvertionBindingContainer = @"FileConvertionBindingContainer.xml";

        public ValidationData()
        {
            this.Contexts = new ContextTable();
            this.flow = new ValidationFlow((ii++).ToString(), true);
            this.bindingContainer = new BindingContainer();
        }

        public ValidationData(ContextTable contexts, ValidationFlow flow, BindingContainer binding)
        {
            this.Contexts = contexts;
            this.flow = flow;
            this.bindingContainer = binding;
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
            bindingContainer.Clear();
        }

        public bool LoadContexTable(string sFolder)
        {
            return true;
        }

        public bool LoadValidationFlow(string sFolder)
        {
            string sFile = sFolder;
            sFile += sFileFlow;
            if (! File.Exists(sFile))
                return false ;

            LoadFlowData(sFile);
        
            return true;
        }

        private void LoadFlowData(string sFile)
        {
            // deSerialize
            TextReader w2 = new StreamReader(sFile);
            XmlSerializer s2 = new XmlSerializer(typeof(ValidatorCoreLib.ValidationFlow));
            flow = (ValidatorCoreLib.ValidationFlow)s2.Deserialize(w2);
            w2.Close();
        }

        static public ConvertionTree LoadConvertionTree(string sFolder)
        {
            string sFile = sFolder;
            sFile += sFileConvertionTree;
            if (!File.Exists(sFile))
                return null;

            return LoadConvertionTreeData(sFile);
        }
        static private ConvertionTree LoadConvertionTreeData(string sFile)
        {
            // deSerialize
            TextReader w2 = new StreamReader(sFile);
            XmlSerializer s2 = new XmlSerializer(typeof(ValidatorCoreLib.ConvertionTree));
            ConvertionTree ct = (ValidatorCoreLib.ConvertionTree)s2.Deserialize(w2);
            w2.Close();
            return ct;
        }

        public bool LoadBindingContainer(string sFolder)
        {
            string sFile = sFolder;
            sFile += sFileConvertionBindingContainer;
            if (!File.Exists(sFile))
                return false;

            LoadBindingContainerData(sFile);
        
            return true;
        }
        private void LoadBindingContainerData(string sFile)
        {
            // deSerialize
            TextReader w2 = new StreamReader(sFile);
            XmlSerializer s2 = new XmlSerializer(typeof(ValidatorCoreLib.BindingContainer));
            bindingContainer = (ValidatorCoreLib.BindingContainer)s2.Deserialize(w2);
            w2.Close();
        }

        static public ConvertionPathItems LoadConvertionPathItems(string sFolder)
        {
            string sFile = sFolder;
            sFile += sFileConvertionPathItems;
            if (!File.Exists(sFile))
                return null;

            return LoadConvertionPathItemsData(sFile);
        }
        static private ConvertionPathItems LoadConvertionPathItemsData(string sFile)
        {
            // deSerialize
            TextReader w2 = new StreamReader(sFile);
            XmlSerializer s2 = new XmlSerializer(typeof(ValidatorCoreLib.ConvertionPathItems));
            ConvertionPathItems cpi = (ValidatorCoreLib.ConvertionPathItems)s2.Deserialize(w2);
            w2.Close();
            return cpi;
        }


        public void SaveFlowData(string sFolder)
        {
            string sFile = sFolder;
            sFile += sFileFlow;

            // Serialize
            TextWriter w = new StreamWriter(sFile);
            XmlSerializer s = new XmlSerializer(typeof(ValidatorCoreLib.ValidationFlow));
            s.Serialize(w, flow);
            w.Close();
        }

        static public void SaveConvertionTreeData(string sFolder, ConvertionTree ct)
        {
            string sFile = sFolder;
            sFile += sFileConvertionTree;

            // Serialize
            TextWriter w = new StreamWriter(sFile);
            XmlSerializer s = new XmlSerializer(typeof(ValidatorCoreLib.ConvertionTree));
            s.Serialize(w, ct);
            w.Close();
        }

        public void SaveBindingContainerData(string sFolder)
        {
            string sFile = sFolder;
            sFile += sFileConvertionBindingContainer;

            // Serialize
            TextWriter w = new StreamWriter(sFile);
            XmlSerializer s = new XmlSerializer(typeof(ValidatorCoreLib.BindingContainer));
            s.Serialize(w, bindingContainer);
            w.Close();
        }

        static public void SaveConvertionConvertionPathItemsData(string sFolder, ConvertionPathItems cpi)
        {
            string sFile = sFolder;
            sFile += sFileConvertionPathItems;

            // Serialize
            TextWriter w = new StreamWriter(sFile);
            XmlSerializer s = new XmlSerializer(typeof(ValidatorCoreLib.ConvertionPathItems));
            s.Serialize(w, cpi);
            w.Close();
        }

    }

    
}
