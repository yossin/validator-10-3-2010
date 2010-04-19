using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ValidatorCoreLib
{
    [Serializable(), XmlRoot("ConvertionCompareItemsContainer", Namespace = "ValidatorCoreLib", IsNullable = false)]
    public class ConvertionCompareItemsContainer : IComparable
    {
        public enum ConvertionCompareItemType
        {
            FromFlow = 0,
            NotFromFlow,
        };

        public string Item;
        public ConvertionCompareItemType convertionCompareItemType;
        
        // Used only for xml Serialization
        public ConvertionCompareItemsContainer(){ }

        public ConvertionCompareItemsContainer(string Item, ConvertionCompareItemType convertionCompareItemType)
        {
            this.Item = Item;
            this.convertionCompareItemType = convertionCompareItemType;
        }

        int IComparable.CompareTo(object obj)
        {
            ConvertionCompareItemsContainer c = (ConvertionCompareItemsContainer)obj;
            return String.Compare(this.Item, c.Item);
        }
    };
}
