using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ValidatorCoreLib
{
    [Serializable(), XmlRoot("ConvertionComparedItems", Namespace = "ValidatorCoreLib", IsNullable = false)]
    public class ConvertionComparedItems
    {
        public List<ConvertionCompareItemsContainer> ConvertionCompareItems = new List<ConvertionCompareItemsContainer>();

        // Used only for xml Serialization
        public ConvertionComparedItems() 
        {
        }

        public void Add(string sItem, ConvertionCompareItemsContainer.ConvertionCompareItemType ccit)
        {
            ConvertionCompareItems.Add(new ConvertionCompareItemsContainer(sItem, ccit));
        }

        public void SortConvertionComparedItems()
        {
            ConvertionCompareItems.Sort();
        }

        public void RemoveUnwanted()
        {
            for ( int ii = 0 ; ii < ConvertionCompareItems.Count ; ii ++ )
            {
                int nItemLastindex;
                while ( ( nItemLastindex = FindLastItemIndex(ConvertionCompareItems[ii].Item ) ) >= 0 && ii < nItemLastindex )
                {
                    if (ConvertionCompareItems[ii].convertionCompareItemType.Equals(ConvertionCompareItemsContainer.ConvertionCompareItemType.NotFromFlow) &&  
                        ConvertionCompareItems[nItemLastindex].convertionCompareItemType.Equals(ConvertionCompareItemsContainer.ConvertionCompareItemType.FromFlow) ) 
                    {
                        ConvertionCompareItems[ii].convertionCompareItemType = ConvertionCompareItemsContainer.ConvertionCompareItemType.FromFlow ;
                    }
                    ConvertionCompareItems.RemoveAt(nItemLastindex);
                }
            }
        }

        public void Add(List<string> sListItem, ConvertionCompareItemsContainer.ConvertionCompareItemType ccit)
        {
            RemoveUnwanted();
            foreach (string sItem in sListItem)
            {
                int nItemindex = IsItemExist(sItem);
                if (nItemindex < 0)
                {
                    Add(sItem, ccit);
                }
                else if (ccit.Equals(ConvertionCompareItemsContainer.ConvertionCompareItemType.FromFlow))
                {
                    ConvertionCompareItems[nItemindex].convertionCompareItemType = ccit;
                }
            }
        }

        
        public int FindLastItemIndex(string sItem)
        {
            return ConvertionCompareItems.FindLastIndex(
            delegate(ConvertionCompareItemsContainer ccic) 
            {
                return ccic.Item.Equals(sItem);
            }
            );
        }        

        public int IsItemExist(string sItem)
        {
            return ConvertionCompareItems.FindIndex(
            delegate(ConvertionCompareItemsContainer ccic) 
            {
                return ccic.Item.Equals(sItem);
            }
            );
        }

        public void Clear()
        {
            ConvertionCompareItems.Clear();
        }
    }
    
}
