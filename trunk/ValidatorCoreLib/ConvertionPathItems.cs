using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ValidatorCoreLib
{
    [Serializable(), XmlRoot("ConvertionPathItems", Namespace = "ValidatorCoreLib", IsNullable = false)]
    public class ConvertionPathItems
    {
        public List<ConvertionPathItemsContainer> ConvertionCompareItems = new List<ConvertionPathItemsContainer>();

        // Used only for xml Serialization
        public ConvertionPathItems() 
        {
        }

        public void Add(string sItem, ConvertionPathItemsContainer.ConvertionCompareItemType ccit)
        {
            ConvertionCompareItems.Add(new ConvertionPathItemsContainer(sItem, ccit));
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
                    if (ConvertionCompareItems[ii].convertionCompareItemType.Equals(ConvertionPathItemsContainer.ConvertionCompareItemType.NotFromFlow) &&  
                        ConvertionCompareItems[nItemLastindex].convertionCompareItemType.Equals(ConvertionPathItemsContainer.ConvertionCompareItemType.FromFlow) ) 
                    {
                        ConvertionCompareItems[ii].convertionCompareItemType = ConvertionPathItemsContainer.ConvertionCompareItemType.FromFlow ;
                    }
                    ConvertionCompareItems.RemoveAt(nItemLastindex);
                }
            }
        }

        public void Add(List<string> sListItem, ConvertionPathItemsContainer.ConvertionCompareItemType ccit)
        {
            RemoveUnwanted();
            foreach (string sItem in sListItem)
            {
                int nItemindex = IsItemExist(sItem);
                if (nItemindex < 0)
                {
                    Add(sItem, ccit);
                }
                else if (ccit.Equals(ConvertionPathItemsContainer.ConvertionCompareItemType.FromFlow))
                {
                    ConvertionCompareItems[nItemindex].convertionCompareItemType = ccit;
                }
            }
        }

        
        public int FindLastItemIndex(string sItem)
        {
            return ConvertionCompareItems.FindLastIndex(
            delegate(ConvertionPathItemsContainer ccic) 
            {
                return ccic.Item.Equals(sItem);
            }
            );
        }        

        public int IsItemExist(string sItem)
        {
            return ConvertionCompareItems.FindIndex(
            delegate(ConvertionPathItemsContainer ccic) 
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
