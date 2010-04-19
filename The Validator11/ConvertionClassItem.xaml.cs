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
        private ConvertionComparedItems convertionComparedItems;
        private TreeViewItem root_convertionClassItem;

        static Color[] Colors_ = { Colors.Beige, Colors.LightBlue };

        public ConvertionClassItem(string convertionItemName, string convertTo, int nSelectedIndex, ConvertionComparedItems convertionComparedItems, TreeViewItem tvParent, TreeViewItem root_convertionClassItem)
        {
            this.tvItem = tvParent;
            InitializeComponent();

            this.root_convertionClassItem = root_convertionClassItem;
            this.convertionItemName.Text = convertionItemName;
            this.convertionComparedItems = convertionComparedItems;
            Add_ConvertTo_Items();
            SetSelected_ConvertTo(nSelectedIndex);
        }

        private void Add_ConvertTo_Items()
        {
            convertToCombo.Items.Clear();
            convertionComparedItems.SortConvertionComparedItems();
            foreach (ConvertionCompareItemsContainer ccic in convertionComparedItems.ConvertionCompareItems)
            {
                ComboBoxItem cbiNew = new ComboBoxItem();
                ColoredObjectRow cor = new ColoredObjectRow(ccic.Item, GetColorFromType(ccic.convertionCompareItemType));
                cbiNew.Content = cor;
                convertToCombo.Items.Add(cbiNew);
            }

            ComboBoxItem cbiNew2 = new ComboBoxItem();
            ColoredObjectRow_AddNew cor_an = new ColoredObjectRow_AddNew(tvItem, root_convertionClassItem);
            cbiNew2.Content = cor_an;
            convertToCombo.Items.Add(cbiNew2);
        }

        private void SetSelected_ConvertTo(int nSelectedIndex)
        {
            if (nSelectedIndex < 0 || nSelectedIndex >= convertToCombo.Items.Count)
                nSelectedIndex = convertToCombo.Items.Count-1;
            convertToCombo.SelectedIndex = nSelectedIndex;
        }

        private void RemoveConvertionClassItem_Click(object sender, RoutedEventArgs e)
        {
            tvItem.Items.Clear();
            tvItem.Header = null;
        }

        private void AddConvertionClassItem_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem Newitem = new TreeViewItem();
            ConvertionClassItem fr = new ConvertionClassItem(@"New Convertion Item", @"Name", 0, convertionComparedItems, Newitem, root_convertionClassItem);
            Newitem.Header = fr;
            Newitem.IsExpanded = true;
            tvItem.Items.Add(Newitem);
        }

        public ValidatorCoreLib.ValidationConvertionItem GetConvertionItem()
        {
            string convertTo = @"";
            if (convertToCombo.SelectedIndex >= 0 && convertToCombo.SelectedIndex < convertToCombo.Items.Count -1 )
            {
                ComboBoxItem cbi = (ComboBoxItem)convertToCombo.Items.GetItemAt(convertToCombo.SelectedIndex);
                if (cbi.Content.GetType().Equals(typeof(ColoredObjectRow)))
                {
                    convertTo = ((ColoredObjectRow)cbi.Content).Text_.Text;
                }
                                   
            }
            return new ValidatorCoreLib.ValidationConvertionItem(convertionItemName.Text, convertTo);
        }

        private static ConvertionCompareItemsContainer.ConvertionCompareItemType GetTypeFromColor(Color colorC)
        {
            if (colorC.Equals(Colors_[0])) return ConvertionCompareItemsContainer.ConvertionCompareItemType.FromFlow;
            if (colorC.Equals(Colors_[1])) return ConvertionCompareItemsContainer.ConvertionCompareItemType.NotFromFlow;

            return ConvertionCompareItemsContainer.ConvertionCompareItemType.FromFlow;
        }

        public static Color GetColorFromType(ConvertionCompareItemsContainer.ConvertionCompareItemType ccit)
        {
            switch (ccit)
            {
                case ConvertionCompareItemsContainer.ConvertionCompareItemType.FromFlow: return Colors_[0];
                case ConvertionCompareItemsContainer.ConvertionCompareItemType.NotFromFlow: return Colors_[1];
            }
            return Colors_[0];
        }

        public bool GetConvertionComparedItems(ValidatorCoreLib.ConvertionComparedItems convertionComparedItems)
        {
            foreach (ComboBoxItem cbi in convertToCombo.Items)
            {
                if (cbi.Content.GetType().Equals(typeof(ColoredObjectRow)))
                {
                    ColoredObjectRow cor = (ColoredObjectRow)cbi.Content;
                    ConvertionCompareItemsContainer.ConvertionCompareItemType ccit = ConvertionClassItem.GetTypeFromColor(((SolidColorBrush)cor.Text_.Background).Color);
                    convertionComparedItems.Add(cor.Text_.Text, ccit);
                }
            }
            return true;
        }
    }
}
