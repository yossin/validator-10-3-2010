using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using ValidatorCoreLib;

namespace The_Validator11
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ConvertionClassItem : UserControl
    {
        private TreeViewItem tvItem;
        //private ConvertionPathItems convertionPathItems;
        //private TreeViewItem root_convertionClassItem;

        static public String sIgnoredString = @"None";
        //static Color[] Colors_ = { Colors.Beige, Colors.LightBlue, Colors.WhiteSmoke };

        public ConvertionClassItem(string convertionPath, string convertionAttribute,/* ConvertionPathItems convertionPathItems,*/ TreeViewItem tvParent/*, TreeViewItem root_convertionClassItem*/)
        {
            this.tvItem = tvParent;
            InitializeComponent();

            //this.root_convertionClassItem = root_convertionClassItem;
            this.convertionAttribute.Text = convertionAttribute;
            this.convertionPath.Text = convertionPath;
            //Add_ConvertTo_Items();
            //SetSelected_ConvertTo(nSelectedIndex);
        }

        /*
        private void Add_ConvertTo_Items()
        {
            convertToCombo.Items.Clear();

            // add "None" item
            ComboBoxItem cbiNew_ = new ComboBoxItem();
            ColoredObjectRow cor_ = new ColoredObjectRow(sIgnoredString, Colors_[2], root_convertionClassItem);
            cbiNew_.Content = cor_;
            convertToCombo.Items.Add(cbiNew_);

            convertionPathItems.SortConvertionComparedItems();
            foreach (ConvertionPathItemsContainer ccic in convertionPathItems.ConvertionCompareItems)
            {
                ComboBoxItem cbiNew = new ComboBoxItem();
                ColoredObjectRow cor = new ColoredObjectRow(ccic.Item, GetColorFromType(ccic.convertionCompareItemType), root_convertionClassItem);
                cbiNew.Content = cor;
                convertToCombo.Items.Add(cbiNew);
            }

            ComboBoxItem cbiNew2 = new ComboBoxItem();
            ColoredObjectRow_AddNew cor_an = new ColoredObjectRow_AddNew(tvItem, root_convertionClassItem);
            cbiNew2.Content = cor_an;
            convertToCombo.Items.Add(cbiNew2);
        }*/
        /*
        private void SetSelected_ConvertTo(int nSelectedIndex)
        {
            if (nSelectedIndex < 0 || nSelectedIndex >= convertToCombo.Items.Count)
                nSelectedIndex = convertToCombo.Items.Count-1;
            convertToCombo.SelectedIndex = nSelectedIndex;
        }*/
        
        private void RemoveConvertionClassItem_Click(object sender, RoutedEventArgs e)
        {
            tvItem.Items.Clear();
            tvItem.Header = null;
        }

        private void AddConvertionClassItem_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem Newitem = new TreeViewItem();
            ConvertionClassItem fr = new ConvertionClassItem(sIgnoredString, @"Attribute Name", /*convertionPathItems, */Newitem/*, root_convertionClassItem*/);
            Newitem.Header = fr;
            Newitem.IsExpanded = true;
            tvItem.Items.Add(Newitem);
        }

        public ValidatorCoreLib.ConvertionTree GetConvertionItem()
        {
            /*string convertTo = @"";
            if (convertToCombo.SelectedIndex >= 0 && convertToCombo.SelectedIndex < convertToCombo.Items.Count -1 )
            {
                ComboBoxItem cbi = (ComboBoxItem)convertToCombo.Items.GetItemAt(convertToCombo.SelectedIndex);
                if (cbi.Content.GetType().Equals(typeof(ColoredObjectRow)))
                {
                    convertTo = ((ColoredObjectRow)cbi.Content).Text_.Text;
                }
                                   
            }*/
            return new ValidatorCoreLib.ConvertionTree(convertionPath.Text, convertionAttribute.Text);
        }
        /*
        private static ConvertionPathItemsContainer.ConvertionCompareItemType GetTypeFromColor(Color colorC)
        {
            if (colorC.Equals(Colors_[0])) return ConvertionPathItemsContainer.ConvertionCompareItemType.FromFlow;
            if (colorC.Equals(Colors_[1])) return ConvertionPathItemsContainer.ConvertionCompareItemType.NotFromFlow;

            return ConvertionPathItemsContainer.ConvertionCompareItemType.FromFlow;
        }*/
        /*
        public static Color GetColorFromType(ConvertionPathItemsContainer.ConvertionCompareItemType ccit)
        {
            switch (ccit)
            {
                case ConvertionPathItemsContainer.ConvertionCompareItemType.FromFlow: return Colors_[0];
                case ConvertionPathItemsContainer.ConvertionCompareItemType.NotFromFlow: return Colors_[1];
            }
            return Colors_[0];
        }*/

        /*
        public bool GetConvertionPathItems(ValidatorCoreLib.ConvertionPathItems convertionPathItems)
        {
            foreach (ComboBoxItem cbi in convertToCombo.Items)
            {
                if (cbi.Content.GetType().Equals(typeof(ColoredObjectRow)))
                {
                    ColoredObjectRow cor = (ColoredObjectRow)cbi.Content;
                    ConvertionPathItemsContainer.ConvertionCompareItemType ccit = ConvertionClassItem.GetTypeFromColor(((SolidColorBrush)cor.Text_.Background).Color);
                    if (!cor.Text_.Text.Equals(sIgnoredString))
                        convertionPathItems.Add(cor.Text_.Text, ccit);
                }
            }
            return true;
        }*/
    }
}
