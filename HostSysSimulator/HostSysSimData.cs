using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using ValidatorSDK;
using ValidatorCoreLib;

namespace HostSysSim
{
    [Serializable(), XmlRoot("HostSysSimData", Namespace = "ValidatorCoreLib", IsNullable = false)]
    public class HostSysSimData
    {
        public List<PropRowData> PropRowsData = new List<PropRowData>();

        // Used only for xml Serialization
        public HostSysSimData() 
        {
        }

        public void AddPropRowData(PropRowData propRowData)
        {
            PropRowsData.Add(propRowData);
        }

        public void ClearPropRowData()
        {
            PropRowsData.Clear();
        }

        public void GetValidationData(ValidationData validationData)
        {
            validationData.Contexts.Clear();
            foreach (PropRowData prd in PropRowsData)
            {
                Object obj = null ;
                switch (prd.RadioBChecked)
                {
                    case 1: obj = (Object)prd.IntData; break;
                    case 2: obj = (Object)prd.FloatData ; break;
                    case 3: 
                        if (prd.ObjectDataComboSelected==0)
                            obj = (Object)(new DummyA(prd.ObjectDataStringData));
                        else     
                            obj = (Object)(new DummyB(prd.ObjectDataStringData));
                        break;
                    case 0: 
                    default:   
                        obj = (Object)prd.StringData ; break;
                }
                validationData.Add(prd.Key, obj);
            }
        }            
    }
}
