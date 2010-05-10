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
    public partial class ColoredObjectRow : UserControl
    {
        private TreeViewItem root_convertionClassItem;
        
        public ColoredObjectRow(string sText, Color colorC, TreeViewItem root_convertionClassItem)
        {
            InitializeComponent();
            SolidColorBrush scb = new SolidColorBrush();
            SolidColorBrush scb2 = new SolidColorBrush();
            scb.Color = colorC;
            scb2.Color = Colors.Black;
            this.Text_.Background = scb;
            this.Text_.Foreground = scb2;
            this.Text_.Text = sText;

            this.root_convertionClassItem = root_convertionClassItem;
        }

        private void onColoredObjectRowRemoveRow(object sender, RoutedEventArgs e)
        {
            RemoveItemFromConvertionPathItems(root_convertionClassItem);
                
        }

        private void RemoveItemFromConvertionPathItems(TreeViewItem tvi)
        {
            ConvertionClassItem cci = (ConvertionClassItem)(tvi.Header);

            ComboBoxItem cbiNew = new ComboBoxItem();
            ColoredObjectRow cor = new ColoredObjectRow("", ConvertionClassItem.GetColorFromType(ConvertionPathItemsContainer.ConvertionCompareItemType.NotFromFlow), tvi);
            cbiNew.Content = cor;
            cci.convertToCombo.Items.Insert(cci.convertToCombo.Items.Count - 1, cbiNew);

            foreach (TreeViewItem tvi_ in tvi.Items)
            {
           //     AddNewItemToConvertionPathItems(sValue, tvi_);
            }
        }
    }
}
