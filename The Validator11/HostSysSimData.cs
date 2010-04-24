using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

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
    }
}
