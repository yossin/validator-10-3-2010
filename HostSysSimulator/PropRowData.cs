using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace HostSysSim
{
    [Serializable(), XmlRoot("PropRowData", Namespace = "ValidatorCoreLib", IsNullable = false)]
    public class PropRowData
    {
        // conflict resolve 
        public string Key { get; set; }
        public int RadioBChecked { get; set; }
        
        public string StringData { get; set; }
        public int IntData { get; set; }
        public float FloatData { get; set; }
      
        public int ObjectDataComboSelected{ get; set; }
        public string ObjectDataStringData { get; set; }
        
        // Used for xml Serialization and empty constractor
        public PropRowData() 
        {
            Key = @"New Key";
            RadioBChecked = 0 ;
            
            StringData = @"New String Data";
            IntData = 0 ;
            FloatData = 0.1f;
          
            ObjectDataComboSelected = 0;
            ObjectDataStringData = @"Object String Data";   
        }

        public PropRowData(string Key, int RadioBChecked, string StringData, int IntData, float FloatData, int ObjectDataComboSelected, string ObjectDataStringData)
        {
            this.Key = Key;
            this.RadioBChecked = RadioBChecked ;
            this.StringData = StringData;
            this.IntData = IntData;
            this.FloatData = FloatData;
            this.ObjectDataComboSelected = ObjectDataComboSelected;
            this.ObjectDataStringData = ObjectDataStringData;
        }
    }
}
