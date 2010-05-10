using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ValidatorCoreLib
{
    [Serializable(), XmlRoot("ConvertionPathItemsContainer", Namespace = "ValidatorCoreLib", IsNullable = false)]
    public class ConvertionPathItemsContainer : IComparable
    {
        public enum ConvertionCompareItemType
        {
            FromFlow = 0,
            NotFromFlow,
        };

        public string Item;
        public ConvertionCompareItemType convertionCompareItemType;
        
        // Used only for xml Serialization
        public ConvertionPathItemsContainer(){ }

        public ConvertionPathItemsContainer(string Item, ConvertionCompareItemType convertionCompareItemType)
        {
            this.Item = Item;
            this.convertionCompareItemType = convertionCompareItemType;
        }

        int IComparable.CompareTo(object obj)
        {
            ConvertionPathItemsContainer c = (ConvertionPathItemsContainer)obj;
            return String.Compare(this.Item, c.Item);
        }
    };
}
