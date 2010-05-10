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
        public BindingContainer BindingContainer { get; set; }

        private static int ii = 1;


        public const string sFileFlow = @"Flow1.xml";
        public const string sFileConvertion = @"Convertion.xml";
        public const string sFileConvertionComparedItems = @"ConvertionComparedItems.xml";


        //todo: fix this
        public ValidationData()
        {
            this.Contexts = new ContextTable();
            this.flow = new ValidationFlow((ii++).ToString(), true);
            //this.BindingContainer = new ValidationConvertionItem();
        }

        //todo: fix this
        public ValidationData(ContextTable contexts, ValidationFlow flow, BindingContainer binding)
        {
            this.Contexts = contexts;
            this.flow = flow;
            this.BindingContainer = binding;
            //convertionComparedItems = new ConvertionComparedItems();
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

       
        //todo: fix this
        public void Clear()
        {
            Contexts.Clear();
            flow.Clear();
            //BindingContainer.Clear();
            //convertionComparedItems.Clear();

        }

        public bool LoadValidationData(string sFolder, bool bLoadContexTable)
        {
            if (bLoadContexTable && !LoadContexTable(sFolder))
                return false ;
            return LoadValidationFlow(sFolder) && LoadValidationConvertionItems(sFolder) && LoadConvertionComparedItems(sFolder);
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

        public bool LoadValidationConvertionItems(string sFolder)
        {
            string sFile = sFolder;
            sFile += sFileConvertion;
            if (!File.Exists(sFile))
                return false;

            LoadConvertionData(sFile);

            return true;
        }
        private void LoadConvertionData(string sFile)
        {
            // deSerialize
            TextReader w2 = new StreamReader(sFile);
            XmlSerializer s2 = new XmlSerializer(typeof(ValidatorCoreLib.ValidationConvertionItem));
            //todo: fix this
            //BindingContainer = (ValidatorCoreLib.ValidationConvertionItem)s2.Deserialize(w2);
            w2.Close();
        }

        public bool LoadConvertionComparedItems(string sFolder)
        {
            string sFile = sFolder;
            sFile += sFileConvertionComparedItems;
            if (!File.Exists(sFile))
                return false;

            LoadConvertionComparedItemsData(sFile);

            return true;
        }
        private void LoadConvertionComparedItemsData(string sFile)
        {
            // deSerialize
            TextReader w2 = new StreamReader(sFile);
            XmlSerializer s2 = new XmlSerializer(typeof(ValidatorCoreLib.ConvertionComparedItems));
            //todo: fix this
//            convertionComparedItems = (ValidatorCoreLib.ConvertionComparedItems)s2.Deserialize(w2);
            w2.Close();
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

        public void SaveConvertionData(string sFolder)
        {
            string sFile = sFolder;
            sFile += sFileConvertion;

            // Serialize
            TextWriter w = new StreamWriter(sFile);
            XmlSerializer s = new XmlSerializer(typeof(ValidatorCoreLib.ValidationConvertionItem));
            s.Serialize(w, BindingContainer);
            w.Close();
        }

        public void SaveConvertionComparedItemsData(string sFolder)
        {
            string sFile = sFolder;
            sFile += sFileConvertionComparedItems;

            // Serialize
            TextWriter w = new StreamWriter(sFile);
            XmlSerializer s = new XmlSerializer(typeof(ValidatorCoreLib.ConvertionComparedItems));
            //todo: fix this
//            s.Serialize(w, convertionComparedItems);
            w.Close();
        }

    }

    
}
